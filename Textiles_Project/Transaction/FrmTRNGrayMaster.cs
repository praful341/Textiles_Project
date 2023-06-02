using BLL;
using BLL.FunctionClasses.Master;
using BLL.FunctionClasses.Transaction;
using BLL.PropertyClasses.Master;
using BLL.PropertyClasses.Transaction;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Textiles_Project.Class;
using Textiles_Project.Master;
using Textiles_Project.Report;

namespace Textiles_Project.Transaction
{
    public partial class FrmTRNGrayMaster : DevExpress.XtraEditors.XtraForm
    {
        BLL.FormEvents objBOFormEvents;
        BLL.FormPer ObjPer;
        BLL.Validation Val;

        TRNGrayMaster ObjTRNGrayMaster = new TRNGrayMaster();
        DataTable DtControlSettings;
        Control _NextEnteredControl;
        private List<Control> _tabControls;
        DataTable DtPending = new DataTable();
        DataTable DTab_Search;
        public FrmTRNGrayMaster()
        {
            InitializeComponent();

            objBOFormEvents = new FormEvents();
            ObjPer = new BLL.FormPer();
            Val = new BLL.Validation();
            DtControlSettings = new DataTable();
            DTab_Search = new DataTable();
            _NextEnteredControl = new Control();
            _tabControls = new List<Control>();
        }
        public void ShowForm()
        {
            ObjPer.FormName = this.Name.ToUpper();
            if (ObjPer.CheckPermission() == false)
            {
                Global.Message(BLL.GlobalDec.gStrPermissionViwMsg);
                return;
            }
            Val.frmGenSet(this);
            AttachFormEvents();

            if (Global.HideFormControls(Val.ToInt(ObjPer.form_id), this) != "")
            {
                Global.Message("Select First User Setting...Please Contact to Administrator...");
                return;
            }

            ControlSettingDT(Val.ToInt(ObjPer.form_id), this);
            AddGotFocusListener(this);
            AddKeyPressListener(this);
            this.KeyPreview = true;

            TabControlsToList(this.Controls);
            _tabControls = _tabControls.OrderBy(x => x.TabIndex).ToList();

            this.Show();
        }
        private void AttachFormEvents()
        {
            objBOFormEvents.CurForm = this;
            objBOFormEvents.FormKeyPress = true;
            objBOFormEvents.FormKeyDown = true;
            objBOFormEvents.FormResize = true;
            objBOFormEvents.FormClosing = true;
            objBOFormEvents.ObjToDispose.Add("");
            objBOFormEvents.ObjToDispose.Add(Val);
            objBOFormEvents.ObjToDispose.Add(objBOFormEvents);
        }

        private void AddGotFocusListener(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                c.GotFocus += new EventHandler(Control_GotFocus);
                if (c.Controls.Count > 0)
                {
                    AddGotFocusListener(c);
                }
            }
        }
        private void Control_GotFocus(object sender, EventArgs e)
        {
            if (!((Control)sender).Name.ToString().Trim().Equals(string.Empty))
            {
                _NextEnteredControl = (Control)sender;
                if ((Control)sender is LookUpEdit)
                {
                    ((LookUpEdit)(Control)sender).ShowPopup();
                }
            }
        }
        private void AddKeyPressListener(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                c.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                if (c.Controls.Count > 0)
                {
                    AddKeyPressListener(c);
                }
            }
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((Control)sender).Name.ToString().Trim().Equals(string.Empty))
            {
                _NextEnteredControl = (Control)sender;
                if ((Control)sender is LookUpEdit)
                {
                    if (e.KeyChar == Convert.ToChar(Keys.Enter))
                    {
                        SendKeys.Send("{TAB}");
                        //((LookUpEdit)(Control)sender).ClosePopup();
                    }

                }
            }
        }
        private void TabControlsToList(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control.TabStop)
                    _tabControls.Add(control);
                if (control.HasChildren)
                    TabControlsToList(control.Controls);
            }
        }
        private void ControlSettingDT(int FormCode, Form pForm)
        {
            BLL.Validation Val = new BLL.Validation();
            Single_Setting ObjSingleSettings = new Single_Setting();
            Single_SettingProperty Property = new Single_SettingProperty();

            Property.role_id = Val.ToInt(BLL.GlobalDec.gEmployeeProperty.role_id);
            Property.form_id = Val.ToInt(FormCode);
            DataTable DtColSetting = ObjSingleSettings.GetData(Property);

            DataTable DtFilterColSetting = (from DataRow dr in DtColSetting.Rows
                                            where Val.ToBooleanToInt(dr["is_control"]) == 1
                                            && dr["column_type"].ToString() != "LABEL"
                                            select dr).CopyToDataTable();
            DevExpress.XtraLayout.LayoutControl l = new DevExpress.XtraLayout.LayoutControl();
            l.OptionsFocus.EnableAutoTabOrder = false;

            if (DtFilterColSetting.Rows.Count > 0)
            {
                DtControlSettings = DtFilterColSetting;
                foreach (Control item1 in pForm.Controls)
                {
                    ControllSettings(item1, DtFilterColSetting);
                }
            }
        }
        private static void ControllSettings(Control item2, DataTable DTab)
        {
            BLL.Validation Val = new BLL.Validation();

            //else
            {
                var VarControlSetting = (from DataRow dr in DTab.Rows
                                         where dr["column_name"].ToString() == item2.Name.ToString()
                                         select dr);

                if (VarControlSetting.Count() > 0)
                {
                    DataRow DRow = VarControlSetting.CopyToDataTable().Rows[0];
                    if (item2.Name.ToString() == Val.ToString(DRow["column_name"]))
                    {
                        if (!(item2 is TextEdit))
                        {
                            if (!(item2 is DateTimePicker))
                            {
                                if (!(item2 is DevExpress.XtraEditors.TextEdit))
                                {
                                    item2.Text = (Val.ToBooleanToInt(DRow["is_compulsory"]).Equals(0) ? Val.ToString(DRow["caption"]) : "* " + Val.ToString(DRow["caption"]));
                                }
                            }
                        }
                        if (Val.ToInt(DRow["tab_index"]) >= 0)
                        {
                            if (item2.CanSelect)
                                item2.TabStop = true;
                        }
                        else
                            item2.TabStop = false;
                        if (Val.ToBooleanToInt(DRow["is_visible"]).Equals(1))
                        {
                            item2.Visible = true;
                            item2.TabStop = true;
                        }
                        else
                        {
                            item2.Visible = false;
                            item2.TabStop = false;
                        }

                        item2.TabIndex = Val.ToInt(DRow["tab_index"]);
                        if (item2.TabIndex == 1)
                        {
                            item2.Select();
                            item2.Focus();
                        }
                        if (Val.ToBooleanToInt(DRow["is_editable"]).Equals(1))
                        {
                            item2.Enabled = true;
                        }
                        else
                        {
                            item2.Enabled = false;
                        }
                    }
                }
                else
                {
                    item2.TabStop = false;
                }
            }
            if (item2.Controls.Count > 0)
            {
                foreach (Control item1 in item2.Controls)
                {
                    ControllSettings(item1, DTab);
                }
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLRNo.Text.Trim()))
            {
                Global.Confirm("LR No Is Required");
                txtLRNo.Focus();
                return;
            }

            if (string.IsNullOrEmpty(DTPReceiveDate.Text.Trim()))
            {
                Global.Confirm("Receive Date Is Required");
                DTPReceiveDate.Focus();
                return;
            }

            if (string.IsNullOrEmpty(LookupParty.Text.Trim()))
            {
                Global.Confirm("Party Name Is Required");
                LookupParty.Focus();
                return;
            }

            PanelShow.Enabled = false;
            PanelSave.Enabled = true;
            GrdGrayList.Enabled = true;
            GetGrayDetail();
            dgvGrayList.BestFitColumns();
            dgvGrayList.Focus();
        }

        public TRNGrayMaster_Property SaveGrayMaster()
        {
            TRNGrayMaster_Property TRNGrayMaster_Property = new TRNGrayMaster_Property();

            TRNGrayMaster_Property.gray_id = Val.ToInt32(lblMode.Tag);
            TRNGrayMaster_Property.voucher_id = Val.ToInt64(txtVoucherNo.Text);
            TRNGrayMaster_Property.receive_date = Val.DBDate(DTPReceiveDate.Text);

            TRNGrayMaster_Property.check_date = Val.DBDate(DTPCheckedDate.Text);

            TRNGrayMaster_Property.party_id = Val.ToInt64(LookupParty.EditValue);
            TRNGrayMaster_Property.party_challan_no = Val.ToString(txtPartyChallanNo.Text);
            TRNGrayMaster_Property.party_challan_date = Val.DBDate(DTPPartyDate.Text);
            TRNGrayMaster_Property.transport_id = Val.ToInt64(LookupTransport.EditValue);
            TRNGrayMaster_Property.lr_no = Val.ToString(txtLRNo.Text);
            TRNGrayMaster_Property.transport_date = Val.DBDate(DtpTransportDate.Text);

            TRNGrayMaster_Property.fold = Val.ToDecimal(txtFold.Text);
            TRNGrayMaster_Property.declare_pcs = Val.ToDecimal(txtDeclarePcs.Text);
            TRNGrayMaster_Property.declare_meter = Val.ToDecimal(txtDeclareMtr.Text);
            TRNGrayMaster_Property.netamount = Val.ToDecimal(0);
            TRNGrayMaster_Property.remarks = Val.ToString(txtRemark.Text);

            return TRNGrayMaster_Property;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValSave() == false)
            {
                return;
            }

            if (Global.Confirm("Are You Sure To Save ?", "Textile Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            Int64 IntRes = 0;
            TRNGrayMaster_Property TRNGrayMaster_PropertyNEW = new TRNGrayMaster_Property();

            TRNGrayMaster_PropertyNEW = SaveGrayMaster();
            TRNGrayMaster_PropertyNEW = ObjTRNGrayMaster.SaveGrayMaster(TRNGrayMaster_PropertyNEW);
            Int64 Newmstid = Val.ToInt64(TRNGrayMaster_PropertyNEW.gray_id.ToString());

            TRNGrayMaster_PropertyNEW = null;

            ArrayList AL = new ArrayList();

            DataTable DTab = (System.Data.DataTable)GrdGrayList.DataSource;
            DTab.AcceptChanges();

            foreach (DataRow DRow in DTab.Rows)
            {
                if (Val.Val(DRow["item_id"]) == 0)
                {
                    continue;
                }

                TRNGrayMaster_Property TRNGrayMaster_Property = new TRNGrayMaster_Property();

                Int64 GrayMasterID = Val.ToInt64(DRow["gray_id"]);
                if (GrayMasterID == 0)
                {
                    TRNGrayMaster_Property.gray_id = Val.ToInt64(Newmstid);
                    TRNGrayMaster_Property.gray_detail_id = Val.ToInt64(DRow["gray_detail_id"]);
                }
                else
                {
                    TRNGrayMaster_Property.gray_id = Val.ToInt64(DRow["gray_id"]);
                    TRNGrayMaster_Property.gray_detail_id = Val.ToInt64(DRow["gray_detail_id"]);
                }

                TRNGrayMaster_Property.voucher_id = Val.ToInt64(txtVoucherNo.Text);
                TRNGrayMaster_Property.item_id = Val.ToInt64(DRow["item_id"]);
                TRNGrayMaster_Property.t_pcs = Val.ToDecimal(DRow["t_pcs"]);
                TRNGrayMaster_Property.rec_meter = Val.ToDecimal(DRow["rec_meter"]);
                TRNGrayMaster_Property.acc_meter = Val.ToDecimal(DRow["acc_meter"]);
                TRNGrayMaster_Property.weight = Val.ToDecimal(DRow["weight"]);
                TRNGrayMaster_Property.total_weight = Val.ToDecimal(DRow["total_weight"]);
                TRNGrayMaster_Property.rate = Val.ToDecimal(DRow["rate"]);
                TRNGrayMaster_Property.amount = Val.ToDecimal(DRow["amount"]);
                TRNGrayMaster_Property.godown_id = Val.ToInt64(DRow["godown_id"]);
                TRNGrayMaster_Property.remarks = Val.ToString(DRow["remarks"]);

                AL.Add(TRNGrayMaster_Property);
            }
            IntRes = ObjTRNGrayMaster.SavePurchaseDetail(AL);

            //if (IntRes != 0)
            //{
            //    Global.Confirm("Save Data Successfully");
            //    GetData();
            //    btnClear_Click(null, null);
            //}
            //else
            //{
            //    Global.Confirm("Error in Data Save");
            //    DTPReceiveDate.Focus();
            //}
            if (IntRes != 0)
            {
                if (Val.ToInt(lblMode.Tag) == 0)
                {
                    Global.Confirm("Gray Details Data Save Successfully");
                    GetData();
                    btnClear_Click(null, null);
                }
                else
                {
                    Global.Confirm("Gray Details Data Update Successfully");
                    GetData();
                    btnClear_Click(null, null);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblMode.Tag = 0;
            lblMode.Text = "Add Mode";

            txtVoucherNo.Text = "";
            DTPReceiveDate.Text = "";
            LookupParty.EditValue = null;
            LookupTransport.EditValue = null;
            txtRemark.Text = "";

            DTPPartyDate.Properties.Mask.Culture = new System.Globalization.CultureInfo("en-US");
            DTPPartyDate.Properties.Mask.EditMask = "dd/MMM/yyyy";
            DTPPartyDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            DTPPartyDate.Properties.CharacterCasing = CharacterCasing.Upper;

            DTPReceiveDate.Properties.Mask.Culture = new System.Globalization.CultureInfo("en-US");
            DTPReceiveDate.Properties.Mask.EditMask = "dd/MMM/yyyy";
            DTPReceiveDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            DTPReceiveDate.Properties.CharacterCasing = CharacterCasing.Upper;

            DtpTransportDate.Properties.Mask.Culture = new System.Globalization.CultureInfo("en-US");
            DtpTransportDate.Properties.Mask.EditMask = "dd/MMM/yyyy";
            DtpTransportDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            DtpTransportDate.Properties.CharacterCasing = CharacterCasing.Upper;

            DTPCheckedDate.Properties.Mask.Culture = new System.Globalization.CultureInfo("en-US");
            DTPCheckedDate.Properties.Mask.EditMask = "dd/MMM/yyyy";
            DTPCheckedDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            DTPCheckedDate.Properties.CharacterCasing = CharacterCasing.Upper;

            DTPPartyDate.EditValue = DateTime.Now;
            DTPReceiveDate.EditValue = DateTime.Now;
            DtpTransportDate.EditValue = DateTime.Now;
            DTPCheckedDate.EditValue = DateTime.Now;

            CalculateGridAmount(dgvGrayList.FocusedRowHandle);

            txtVoucherNo.Text = ObjTRNGrayMaster.FindNewID().ToString();
            txtFold.Text = string.Empty;
            txtDeclarePcs.Text = string.Empty;
            txtDeclareMtr.Text = string.Empty;
            txtLRNo.Text = string.Empty;
            txtPartyChallanNo.Text = string.Empty;

            PanelShow.Enabled = true;
            GrdGrayList.Enabled = false;
            PanelSave.Enabled = false;
            GrdGrayList.DataSource = null;
            txtPassword.Text = "";
            GetData();
            DTPReceiveDate.Focus();
        }
        public DataTable GetData()
        {
            TRNGrayMaster_Property TRNGrayMaster_Property = new TRNGrayMaster_Property();
            TRNGrayMaster_Property.from_date = Val.DBDate(dtpSearchFromDate.Text);
            TRNGrayMaster_Property.to_date = Val.DBDate(dtpSearchToDate.Text);

            DTab_Search = ObjTRNGrayMaster.GetData(TRNGrayMaster_Property);

            if (DTab_Search.Columns.Contains("SEL") == false)
            {
                if (DTab_Search.Columns.Contains("SEL") == false)
                {
                    DataColumn Col = new DataColumn();
                    Col.ColumnName = "SEL";
                    Col.DataType = typeof(bool);
                    Col.DefaultValue = false;
                    DTab_Search.Columns.Add(Col);
                }
            }
            DTab_Search.Columns["SEL"].SetOrdinal(0);

            GrdGrayMaster.DataSource = DTab_Search;
            GrdGrayMaster.RefreshDataSource();
            dgvGrayMaster.BestFitColumns();
            TRNGrayMaster_Property = null;
            GetGrayDetail();
            return DTab_Search;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region functions

        private bool ValSave()
        {
            if (string.IsNullOrEmpty(txtLRNo.Text.Trim()))
            {
                Global.Confirm("Invoice No Is Required");
                txtLRNo.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(DTPReceiveDate.Text.Trim()))
            {
                Global.Confirm("Invoice Date Is Required");
                DTPReceiveDate.Focus();
                return false;
            }

            if (Val.ToString(LookupParty.Text.Trim()) == "")
            {
                Global.Confirm("From Party Is Required");
                LookupParty.Focus();
                return false;
            }
            DataTable DTab = (DataTable)GrdGrayList.DataSource;
            if (DTab != null)
            {
                if (DTab.Rows.Count <= 0)
                {
                    Global.Confirm("Fill Purchase Items list");
                    txtLRNo.Focus();
                    return false;
                }
            }
            return true;
        }

        private void GetGrayDetail()
        {
            this.Cursor = Cursors.WaitCursor;
            GrdGrayList.DataSource = null;
            DataTable DTab = ObjTRNGrayMaster.GetGrayDetail(Val.ToInt64(txtVoucherNo.Text));
            DTab.Rows.Add();
            GrdGrayList.DataSource = DTab;

            this.Cursor = Cursors.Default;
        }

        #endregion

        private void LookupFromParty_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                //FrmLedgerMaster frmCnt = new FrmLedgerMaster();
                //frmCnt.ShowDialog();
                //Ledger_MasterProperty Party = new Ledger_MasterProperty();
                //Party.Party_Type = "";
                //Global.LOOKUPParty(LookupFromParty, Party);
                //Party = null;
            }
        }

        private void FrmItemPurchaseMaster_Shown(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PanelShow.Enabled = true;
            GrdGrayList.Enabled = false;
            PanelSave.Enabled = false;
            GrdGrayList.DataSource = null;
        }

        private Boolean ValDelete()
        {
            if (Val.Val(txtLRNo.Text) == 0)
            {
                Global.Message("Invoice No Is Required");
                txtLRNo.Focus();
                return false;
            }
            return true;
        }

        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Clicks == 2)
                {
                    DataRow DRow = dgvGrayMaster.GetDataRow(e.RowHandle);

                    lblMode.Text = "Edit Mode";
                    lblMode.Tag = Val.ToInt64(DRow["gray_id"]);
                    txtVoucherNo.Text = Val.ToString(DRow["voucher_id"]);
                    DTPReceiveDate.EditValue = Val.DBDate(DRow["receive_date"].ToString());
                    DTPCheckedDate.EditValue = Val.DBDate(DRow["check_date"].ToString());
                    LookupParty.EditValue = Val.ToInt32(DRow["party_id"]);
                    txtPartyChallanNo.Text = Val.ToString(DRow["party_challan_no"]);
                    DTPPartyDate.EditValue = Val.DBDate(DRow["party_challan_date"].ToString());
                    LookupTransport.EditValue = Val.ToInt32(DRow["transport_id"]);
                    txtLRNo.Text = Val.ToString(DRow["lr_no"]);
                    DtpTransportDate.EditValue = Val.DBDate(DRow["transport_date"].ToString());
                    txtFold.Text = Val.ToDecimal(DRow["fold"]).ToString();
                    txtDeclarePcs.Text = Val.ToDecimal(DRow["declare_pcs"]).ToString();
                    txtDeclareMtr.Text = Val.ToDecimal(DRow["declare_meter"]).ToString();
                    txtRemark.Text = Val.ToString(DRow["remarks"]);
                    //txtNetAmt.Text = Val.ToString(DRow["net_amount"]);
                    GetGrayDetail();
                    PanelShow.Enabled = true;
                    GrdGrayList.Enabled = true;
                    PanelSave.Enabled = true;
                    CalculateGridAmount(dgvGrayList.FocusedRowHandle);
                    dgvGrayList.BestFitColumns();
                    txtLRNo.Focus();
                }
            }
        }

        private void RepLookUpItem_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmItemMaster frmCnt = new FrmItemMaster();
                frmCnt.ShowDialog();
                Global.LOOKUPItemRep(RepLookUpItem);
            }
        }

        //public DataTable GetData()
        //{
        //Invoice_EntryProperty Invoice_EntryProperty = new Invoice_EntryProperty();
        //Invoice_EntryProperty.From_Date = Val.DBDate(dtpSearchFromDate.Text);
        //Invoice_EntryProperty.To_Date = Val.DBDate(dtpSearchToDate.Text);
        //DataTable DTab = ObjInvoiceEntry.GetData(Invoice_EntryProperty, Form_Type);
        //gridControl2.DataSource = DTab;
        //gridControl2.RefreshDataSource();
        //gridView2.BestFitColumns();
        //Invoice_EntryProperty = null;
        //GetPurchaseDetail();
        //return DTab;
        //}

        private void RepLookUpItem_Validating(object sender, CancelEventArgs e)
        {
            //LookUpEdit type = sender as LookUpEdit;

            //dgvGrayList.SetRowCellValue(dgvGrayList.FocusedRowHandle, "Unit_Name", type.GetColumnValue("UNIT_NAME"));
            //dgvGrayList.SetRowCellValue(dgvGrayList.FocusedRowHandle, "HSN_ID", type.GetColumnValue("HSN_ID"));
            //dgvGrayList.SetRowCellValue(dgvGrayList.FocusedRowHandle, "Rate", type.GetColumnValue("LAST_PURCHASE_RATE"));
            //dgvGrayList.SetRowCellValue(dgvGrayList.FocusedRowHandle, "SGST_Rate", type.GetColumnValue("SGST_RATE"));
            //dgvGrayList.SetRowCellValue(dgvGrayList.FocusedRowHandle, "CGST_Rate", type.GetColumnValue("CGST_RATE"));
            //dgvGrayList.SetRowCellValue(dgvGrayList.FocusedRowHandle, "IGST_Rate", type.GetColumnValue("IGST_RATE"));
        }
        private void RepHSNCode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                //FrmItemHSNMaster frmCnt = new FrmItemHSNMaster();
                //frmCnt.ShowDialog();
                //Global.LOOKUPItemHSNRep(RepHSNCode);
            }
        }
        private void CalculateGridAmount(int rowindex)
        {
            try
            {
                dgvGrayList.SetRowCellValue(rowindex, "total_weight", Math.Round((Val.ToDouble(dgvGrayList.GetRowCellValue(rowindex, "weight")) / 100) * Val.ToDouble(dgvGrayList.GetRowCellValue(rowindex, "acc_meter")), 2));
                dgvGrayList.SetRowCellValue(rowindex, "amount", Math.Round(Val.ToDouble(dgvGrayList.GetRowCellValue(rowindex, "acc_meter")) * Val.ToDouble(dgvGrayList.GetRowCellValue(rowindex, "rate")), 2));
                //double NetAmt = Val.ToDouble(dgvGrayList.Columns["amount"].SummaryText);
                //txtNetAmt.Text = NetAmt.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void dgvPurchase_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            CalculateGridAmount(dgvGrayList.FocusedRowHandle);
        }
        private void dgvPurchase_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            CalculateGridAmount(e.PrevFocusedRowHandle);
        }
        private void dgvPurchase_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            CalculateGridAmount(dgvGrayList.FocusedRowHandle);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            DTab_Search.AcceptChanges();
            if (DTab_Search.Rows.Count > 0 && DTab_Search.Select("SEL = true").Length > 0)
            {
                DataTable CheckDT = new DataTable();
                CheckDT = DTab_Search.Select("SEL <> False").CopyToDataTable();


                string StrVoucherID = "";
                string StrGrayDetailID = "";


                DataTable dtCheckedData = (DataTable)GrdGrayMaster.DataSource;
                dtCheckedData.AcceptChanges();

                for (int i = 0; i < dgvGrayMaster.DataRowCount; i++)
                {
                    if (dgvGrayMaster.GetRowCellValue(i, "SEL").ToString().ToUpper() == "TRUE")
                    {
                        if (StrVoucherID.Length > 0)
                        {
                            StrVoucherID = StrVoucherID + "," + dgvGrayMaster.GetRowCellValue(i, "voucher_id").ToString();
                            StrGrayDetailID = StrGrayDetailID + "," + dgvGrayMaster.GetRowCellValue(i, "gray_detail_id").ToString();
                        }
                        else
                        {
                            StrVoucherID = dgvGrayMaster.GetRowCellValue(i, "voucher_id").ToString();
                            StrGrayDetailID = dgvGrayMaster.GetRowCellValue(i, "gray_detail_id").ToString();
                        }
                    }
                }

                //DialogResult result = MessageBox.Show("Do you want to Print Data?", "Confirmation", MessageBoxButtons.YesNoCancel);
                //if (result != DialogResult.Yes)
                //{
                //    return;
                //}

                panelProgress.Visible = true;
                TRNGrayMaster_Property TRNGrayMaster_Property = new TRNGrayMaster_Property();
                ObjTRNGrayMaster = new TRNGrayMaster();

                TRNGrayMaster_Property.voucher_name = Val.ToString(StrVoucherID);
                TRNGrayMaster_Property.gray_detail_name = Val.ToString(StrGrayDetailID);

                DataTable dtpur = new DataTable();
                dtpur = ObjTRNGrayMaster.GetGrayReceipt_PrintData(TRNGrayMaster_Property);

                if (dtpur.Rows.Count > 0)
                {
                    FrmReportViewer rep = new FrmReportViewer();
                    FrmReportViewer FrmReportViewer = new FrmReportViewer();
                    FrmReportViewer.DS.Tables.Add(dtpur);
                    FrmReportViewer.GroupBy = "";
                    FrmReportViewer.RepName = "";
                    FrmReportViewer.RepPara = "";
                    this.Cursor = Cursors.Default;
                    FrmReportViewer.AllowSetFormula = true;

                    FrmReportViewer.ShowForm("Gray_Receive_Note", 120, FrmReportViewer.ReportFolder.ACCOUNT);

                    TRNGrayMaster_Property = null;
                    dtpur = null;
                    FrmReportViewer.DS.Tables.Clear();
                    FrmReportViewer.DS.Clear();
                    FrmReportViewer = null;
                    panelProgress.Visible = false;
                }
            }
            else
            {
                General.ShowErrors("Atleast 1 Lot must be select in grid.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure To Delete ?", "Textiles Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            int IntRes = 0;
            TRNGrayMaster_Property TRNGrayMaster_PropertyNew = new TRNGrayMaster_Property();
            ObjTRNGrayMaster = new TRNGrayMaster();
            TRNGrayMaster_PropertyNew.voucher_id = Val.ToInt64(txtVoucherNo.Text);
            IntRes = ObjTRNGrayMaster.DeleteGrayEntryMaster(TRNGrayMaster_PropertyNew);

            if (IntRes != 0)
            {
                Global.Confirm("Data Deleted Successfully");
                GetData();
                btnClear_Click(null, null);
            }
            else
            {
                Global.Confirm("Error in Data Delete");
                DTPReceiveDate.Focus();
            }
        }

        private void GrdPurchase_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F9)
            //{
            //    if (Global.Confirm("Are you sure delete selected row?", "STORE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            //    {
            //        //dgvPurchase.DeleteRow(dgvPurchase.GetRowHandle(dgvPurchase.FocusedRowHandle));
            //        Invoice_EntryProperty Invoice_EntryProperty = new Invoice_EntryProperty();
            //        int IntRes = 0;
            //        Int64 ItemPurchaseMasterID = Val.ToInt64(dgvPurchase.GetFocusedRowCellValue("ItemPurchaseMasterID").ToString());
            //        Invoice_EntryProperty.ItemPurchaseMasterID = Val.ToInt64(ItemPurchaseMasterID);
            //        Invoice_EntryProperty.ItemPurchaseDetailID = Val.ToInt64(dgvPurchase.GetFocusedRowCellValue("ItemPurchaseDetailID").ToString());

            //        if (ItemPurchaseMasterID == 0)
            //        {
            //            dgvPurchase.DeleteRow(dgvPurchase.GetRowHandle(dgvPurchase.FocusedRowHandle));
            //        }
            //        else
            //        {
            //            IntRes = ObjInvoiceEntry.DeletePurchaseDetail(Invoice_EntryProperty, Form_Type);
            //            dgvPurchase.DeleteRow(dgvPurchase.GetRowHandle(dgvPurchase.FocusedRowHandle));
            //        }

            //        if (IntRes == -1)
            //        {
            //            Global.Confirm("Error in Detail Deleted Data.");
            //            Invoice_EntryProperty = null;
            //        }
            //        else
            //        {
            //            Global.Confirm("Detail Deleted successfully...");
            //            Invoice_EntryProperty = null;
            //        }
            //    }
            //}
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void FrmTRNGrayMaster_Load(object sender, EventArgs e)
        {
            Global.LOOKUPItemRep(RepLookUpItem);
            Global.LOOKUPGoDownRep(RepLookUpGoDown);
            btnClear_Click(btnClear, null);
            Ledger_MasterProperty Party = new Ledger_MasterProperty();

            // Global.LOOKUPParty(LookupFromParty, Party);

            Global.LOOKUPParty(LookupParty);
            Global.LOOKUPTransport(LookupTransport);

            DTPCheckedDate.Properties.Mask.Culture = new System.Globalization.CultureInfo("en-US");
            DTPCheckedDate.Properties.Mask.EditMask = "dd/MMM/yyyy";
            DTPCheckedDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            DTPCheckedDate.Properties.CharacterCasing = CharacterCasing.Upper;

            dtpSearchFromDate.Properties.Mask.Culture = new System.Globalization.CultureInfo("en-US");
            dtpSearchFromDate.Properties.Mask.EditMask = "dd/MMM/yyyy";
            dtpSearchFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            dtpSearchFromDate.Properties.CharacterCasing = CharacterCasing.Upper;

            dtpSearchToDate.Properties.Mask.Culture = new System.Globalization.CultureInfo("en-US");
            dtpSearchToDate.Properties.Mask.EditMask = "dd/MMM/yyyy";
            dtpSearchToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            dtpSearchToDate.Properties.CharacterCasing = CharacterCasing.Upper;

            dtpSearchFromDate.EditValue = DateTime.Now;
            dtpSearchToDate.EditValue = DateTime.Now;
            DTPCheckedDate.EditValue = DateTime.Now;
            //GetData();
            btnClear_Click(btnClear, null);
        }

        private void LookupParty_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmPartyMaster frmParty = new FrmPartyMaster();
                frmParty.ShowDialog();
                Global.LOOKUPParty(LookupParty);
            }
        }

        private void LookupTransport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmTransportMaster frmTransport = new FrmTransportMaster();
                frmTransport.ShowDialog();
                Global.LOOKUPTransport(LookupTransport);
            }
        }

        private void RepLookUpGoDown_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmGoDownMaster frmCnt = new FrmGoDownMaster();
                frmCnt.ShowDialog();
                Global.LOOKUPGoDownRep(RepLookUpGoDown);
            }
        }

        private void RepBtnDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (Global.Confirm("Are you sure delete selected row?", "STOCK", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                TRNGrayMaster_Property TRNGrayMaster_Property = new TRNGrayMaster_Property();
                int IntRes = 0;
                TRNGrayMaster_Property.gray_id = Val.ToInt64(dgvGrayList.GetFocusedRowCellValue("gray_id").ToString());
                TRNGrayMaster_Property.gray_detail_id = Val.ToInt64(dgvGrayList.GetFocusedRowCellValue("gray_detail_id").ToString());

                if (TRNGrayMaster_Property.gray_id == 0)
                {
                    dgvGrayList.DeleteRow(dgvGrayList.GetRowHandle(dgvGrayList.FocusedRowHandle));
                }
                else
                {
                    IntRes = ObjTRNGrayMaster.DeleteGrayDetail(TRNGrayMaster_Property);
                    dgvGrayList.DeleteRow(dgvGrayList.GetRowHandle(dgvGrayList.FocusedRowHandle));
                }

                if (IntRes == -1)
                {
                    Global.Confirm("Error in Detail Deleted Data.");
                    TRNGrayMaster_Property = null;
                }
                else
                {
                    Global.Confirm("Detail Deleted successfully...");
                    TRNGrayMaster_Property = null;
                }
            }
        }

        private void txtPassword_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text != "")
            {
                if (Val.ToString(txtPassword.Text) == "123")
                {
                    btnDelete.Visible = true;
                }
                else
                {
                    btnDelete.Visible = false;
                }
            }
            else
            {
                btnDelete.Visible = false;
            }
        }

        private void RepBtnPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            TRNGrayMaster objTRNGrayMaster = new TRNGrayMaster();
            TRNGrayMaster_Property objTRNGrayMaster_Property = new TRNGrayMaster_Property();
            objTRNGrayMaster_Property.item_id = Val.ToInt64(dgvGrayList.GetFocusedRowCellValue("item_id").ToString());
            objTRNGrayMaster_Property.voucher_id = Val.ToInt64(txtVoucherNo.Text);
            objTRNGrayMaster_Property.gray_detail_id = Val.ToInt64(dgvGrayList.GetFocusedRowCellValue("gray_detail_id").ToString());

            DtPending = objTRNGrayMaster.GetGrayItemDetail(objTRNGrayMaster_Property);

            FrmMFGGrayItemDetail FrmMFGGrayItemDetail = new FrmMFGGrayItemDetail();
            FrmMFGGrayItemDetail.FrmTRNGrayMaster = this;
            FrmMFGGrayItemDetail.DTab = DtPending;
            FrmMFGGrayItemDetail.ShowForm(this, Val.ToInt64(objTRNGrayMaster_Property.item_id), Val.ToInt64(txtVoucherNo.Text), Val.ToInt64(objTRNGrayMaster_Property.gray_detail_id));
        }

        public void GetItemData(decimal Rec_Meter, decimal Acc_Meter, Int64 Item_ID)
        {
            try
            {
                DataTable DTab_Data = (DataTable)GrdGrayList.DataSource;
                DTab_Data.AcceptChanges();

                if (DTab_Data != null)
                {
                    for (int i = 0; i < DTab_Data.Rows.Count; i++)
                    {
                        if (Val.Val(DTab_Data.Rows[i]["item_id"]) == 0)
                        {
                            continue;
                        }
                        Int64 Item_ID_Data = Val.ToInt64(txtVoucherNo.Text);

                        if (Item_ID_Data == Item_ID)
                        {
                            DTab_Data.Rows[i]["rec_meter"] = Val.ToDecimal(Rec_Meter);
                            DTab_Data.Rows[i]["acc_meter"] = Val.ToDecimal(Acc_Meter);
                        }
                    }
                }
                dgvGrayList.BestFitColumns();
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
            }
        }

        private void MNExportExcel_Click(object sender, EventArgs e)
        {
            Global.Export("xlsx", dgvGrayMaster);
        }

        private void MNExportPDF_Click(object sender, EventArgs e)
        {
            Global.Export("pdf", dgvGrayMaster);
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dgvGrayMaster.RowCount; i++)
                {
                    dgvGrayMaster.SetRowCellValue(i, "SEL", chkAll.Checked);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
                return;
            }
        }

        private void btnGrayCheckingPrint_Click(object sender, EventArgs e)
        {
            DTab_Search.AcceptChanges();
            if (DTab_Search.Rows.Count > 0 && DTab_Search.Select("SEL = true").Length > 0)
            {
                DataTable CheckDT = new DataTable();
                CheckDT = DTab_Search.Select("SEL <> False").CopyToDataTable();


                string StrVoucherID = "";
                string StrGrayDetailID = "";


                DataTable dtCheckedData = (DataTable)GrdGrayMaster.DataSource;
                dtCheckedData.AcceptChanges();

                for (int i = 0; i < dgvGrayMaster.DataRowCount; i++)
                {
                    if (dgvGrayMaster.GetRowCellValue(i, "SEL").ToString().ToUpper() == "TRUE")
                    {
                        if (StrVoucherID.Length > 0)
                        {
                            StrVoucherID = StrVoucherID + "," + dgvGrayMaster.GetRowCellValue(i, "voucher_id").ToString();
                            StrGrayDetailID = StrGrayDetailID + "," + dgvGrayMaster.GetRowCellValue(i, "gray_detail_id").ToString();
                        }
                        else
                        {
                            StrVoucherID = dgvGrayMaster.GetRowCellValue(i, "voucher_id").ToString();
                            StrGrayDetailID = dgvGrayMaster.GetRowCellValue(i, "gray_detail_id").ToString();
                        }
                    }
                }

                panelProgress.Visible = true;
                TRNGrayMaster_Property TRNGrayMaster_Property = new TRNGrayMaster_Property();
                ObjTRNGrayMaster = new TRNGrayMaster();

                TRNGrayMaster_Property.voucher_name = Val.ToString(StrVoucherID);
                TRNGrayMaster_Property.gray_detail_name = Val.ToString(StrGrayDetailID);

                DataSet dtpur = new DataSet();
                dtpur = ObjTRNGrayMaster.GetGrayChecking_PrintData(TRNGrayMaster_Property);


                int Part1_Count = dtpur.Tables[2].Rows.Count;
                for (int i = Part1_Count; i < 36; i++)
                {
                    if (i <= 36)
                    {
                        dtpur.Tables[2].Rows.Add();
                    }
                }


                int Part2_Count = dtpur.Tables[3].Rows.Count;
                for (int i = Part2_Count; i < 36; i++)
                {
                    if (i <= 36)
                    {
                        dtpur.Tables[3].Rows.Add();
                    }
                }


                int Part3_Count = dtpur.Tables[4].Rows.Count;
                for (int i = Part3_Count; i < 36; i++)
                {
                    if (i <= 36)
                    {
                        dtpur.Tables[4].Rows.Add();
                    }
                }

                //FrmReportViewer rep = new FrmReportViewer();
                //FrmReportViewer FrmReportViewer = new FrmReportViewer();
                //FrmReportViewer.DS.Tables.Add(dtpur);
                //FrmReportViewer.GroupBy = "";
                //FrmReportViewer.RepName = "";
                //FrmReportViewer.RepPara = "";
                //this.Cursor = Cursors.Default;
                //FrmReportViewer.AllowSetFormula = true;

                FrmReportViewer FrmReportViewer = new FrmReportViewer();
                foreach (DataTable DTab in dtpur.Tables)
                    FrmReportViewer.DS.Tables.Add(DTab.Copy());
                FrmReportViewer.GroupBy = "";
                FrmReportViewer.RepName = "";
                FrmReportViewer.RepPara = "";
                this.Cursor = Cursors.Default;
                FrmReportViewer.AllowSetFormula = true;

                //FrmReportViewer.ShowForm("Receive_Note_Print", 120, FrmReportViewer.ReportFolder.ACCOUNT);
                FrmReportViewer.ShowForm_SubReport("Receive_Note_Print", 120, FrmReportViewer.ReportFolder.ACCOUNT);

                TRNGrayMaster_Property = null;
                dtpur = null;
                FrmReportViewer.DS.Tables.Clear();
                FrmReportViewer.DS.Clear();
                FrmReportViewer = null;
                panelProgress.Visible = false;

            }
            else
            {
                General.ShowErrors("Atleast 1 Lot must be select in grid.");
            }
        }

        private void BtnPrintGrid_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();

                PrinterSettingsUsing pst = new PrinterSettingsUsing();

                PrintSystem.PageSettings.AssignDefaultPrinterSettings(pst);


                PrintableComponentLink link = new PrintableComponentLink(PrintSystem);

                link.Component = GrdGrayMaster;
                link.Landscape = true;

                dgvGrayMaster.OptionsPrint.AutoWidth = true;

                link.Margins.Left = 40;
                link.Margins.Right = 40;
                link.Margins.Bottom = 40;
                link.Margins.Top = 130;
                link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
                link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
                link.CreateDocument();
                link.ShowPreview();
                link.PrintDlg();
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }
        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title
            TextBrick BrickTitle = e.Graph.DrawString("Gray Detail List", System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 400, 0));
            TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + DateTime.Now, System.Drawing.Color.Navy, new RectangleF(IntX, 25, 400, 18), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitledate.Font = new Font("Tahoma", 8, FontStyle.Bold);
            BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitledate.ForeColor = Color.Black;
        }
        public void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 100, 0));

            PageInfoBrick BrickPageNo = e.Graph.DrawPageInfo(PageInfo.NumberOfTotal, "Page {0} of {1}", System.Drawing.Color.Navy, new RectangleF(IntX, 0, 100, 15), DevExpress.XtraPrinting.BorderSide.None);
            BrickPageNo.LineAlignment = BrickAlignment.Far;
            BrickPageNo.Alignment = BrickAlignment.Far;
            BrickPageNo.Font = new Font("Tahoma", 8, FontStyle.Bold); ;
            BrickPageNo.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickPageNo.VertAlignment = DevExpress.Utils.VertAlignment.Center;
        }

        private void MNExportTEXT_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();

                PrinterSettingsUsing pst = new PrinterSettingsUsing();

                PrintSystem.PageSettings.AssignDefaultPrinterSettings(pst);


                PrintableComponentLink link = new PrintableComponentLink(PrintSystem);

                link.Component = GrdGrayMaster;
                link.Landscape = true;

                dgvGrayMaster.OptionsPrint.AutoWidth = true;

                link.Margins.Left = 40;
                link.Margins.Right = 40;
                link.Margins.Bottom = 40;
                link.Margins.Top = 130;
                link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
                link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
                link.CreateDocument();
                link.ShowPreview();
                link.PrintDlg();
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }
    }
}