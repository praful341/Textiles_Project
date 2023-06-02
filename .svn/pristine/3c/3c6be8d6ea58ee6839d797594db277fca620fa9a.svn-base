using BLL;
using BLL.FunctionClasses.Master;
using BLL.PropertyClasses.Master;
using System;
using System.Collections.Generic;
using System.Data;
using Textiles_Project.Class;

namespace Textiles_Project.Master
{
    public partial class FrmPartyMaster : DevExpress.XtraEditors.XtraForm
    {
        BLL.FormEvents objBOFormEvents = new BLL.FormEvents();
        BLL.Validation Val = new BLL.Validation();
        PartyMaster objParty;

        public FrmPartyMaster()
        {
            InitializeComponent();

            objParty = new PartyMaster();
        }
        public void ShowForm()
        {
            Val.frmGenSet(this);
            AttachFormEvents();
            this.Show();
        }
        private void AttachFormEvents()
        {
            objBOFormEvents.CurForm = this;
            objBOFormEvents.FormKeyPress = true;
            objBOFormEvents.FormKeyDown = true;
            objBOFormEvents.FormResize = true;
            objBOFormEvents.FormClosing = true;
            objBOFormEvents.ObjToDispose.Add(Val);
            objBOFormEvents.ObjToDispose.Add(objBOFormEvents);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblMode.Tag = 0;
            lblMode.Text = "Add Mode";
            txtPartyName.Text = "";
            LookupBroker.EditValue = null;
            txtGstNo.Text = string.Empty;
            chkActive.Checked = true;
            txtRemark.Text = "";
            txtAddress.Text = "";
            txtEmailID.Text = "";
            txtMobileNo.Text = "";
            txtCreditPeriod.Text = "";
            LookupCity.EditValue = null;
            LookupState.EditValue = null;
            LookupCountry.EditValue = null;
            txtPartyName.Focus();
        }

        #region Validation


        private bool ValidateDetails()
        {
            bool blnFocus = false;
            List<ListError> lstError = new List<ListError>();
            try
            {
                if (txtPartyName.Text.Length == 0)
                {
                    Global.Confirm("Party Name Is Required");
                    txtPartyName.Focus();
                    return false;
                }
                if (!objParty.ISExists(txtPartyName.Text, Val.ToInt64(lblMode.Tag)).ToString().Trim().Equals(string.Empty))
                {
                    lstError.Add(new ListError(23, "Party Name"));
                    if (!blnFocus)
                    {
                        blnFocus = true;
                        txtPartyName.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                lstError.Add(new ListError(ex));
            }
            return (!(BLL.General.ShowErrors(lstError)));
        }


        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateDetails())
            {
                return;
            }

            Party_MasterProperty PartyMasterProperty = new Party_MasterProperty();
            PartyMasterProperty.party_id = Val.ToInt32(lblMode.Tag);
            PartyMasterProperty.party_name = Val.ToString(txtPartyName.Text).ToUpper();
            PartyMasterProperty.active = Val.ToBoolean(chkActive.Checked);
            PartyMasterProperty.remarks = txtRemark.Text;
            PartyMasterProperty.address = Val.ToString(txtAddress.Text);
            PartyMasterProperty.broker_id = Val.ToInt64(LookupBroker.EditValue);
            PartyMasterProperty.gst_no = Val.ToString(txtGstNo.Text);
            PartyMasterProperty.mobile_no = Val.ToString(txtMobileNo.Text);
            PartyMasterProperty.email_id = Val.ToString(txtEmailID.Text);
            PartyMasterProperty.country_id = Val.ToInt32(LookupCountry.EditValue);
            PartyMasterProperty.state_id = Val.ToInt32(LookupState.EditValue);
            PartyMasterProperty.city_id = Val.ToInt32(LookupCity.EditValue);
            PartyMasterProperty.credit_period = Val.ToInt32(txtCreditPeriod.Text);

            int IntRes = objParty.Save(PartyMasterProperty);
            if (IntRes == -1)
            {
                Global.Confirm("Error In Save Party Details");
                txtPartyName.Focus();
            }
            else
            {
                if (Val.ToInt(lblMode.Tag) == 0)
                {
                    Global.Confirm("Party Details Data Save Successfully");
                }
                else
                {
                    Global.Confirm("Party Details Data Update Successfully");
                }

                btnClear_Click(sender, e);
                GetData();
            }
            PartyMasterProperty = null;
        }
        public void GetData()
        {
            DataTable DTab = objParty.GetData();
            grdPartyMaster.DataSource = DTab;
            dgvPartyMaster.BestFitColumns();
        }
        private void FrmPartyMaster_Load(object sender, EventArgs e)
        {
            Global.LOOKUPBroker(LookupBroker);
            Global.LOOKUPCountry(LookupCountry);
            Global.LOOKUPState(LookupState);
            Global.LOOKUPCity(LookupCity);
            GetData();
            chkActive.Checked = true;
            txtPartyName.Focus();
        }

        private void LookupBroker_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmBrokerMaster frmBroker = new FrmBrokerMaster();
                frmBroker.ShowDialog();
                Global.LOOKUPBroker(LookupBroker);
            }
        }

        private void dgvPartyMaster_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Clicks == 2)
                {
                    DataRow Drow = dgvPartyMaster.GetDataRow(e.RowHandle);
                    lblMode.Text = "Edit Mode";
                    lblMode.Tag = Val.ToInt64(Drow["party_id"]);
                    txtPartyName.Text = Val.ToString(Drow["party_name"]);
                    chkActive.Checked = Val.ToBoolean(Drow["active"]);
                    txtRemark.Text = Val.ToString(Drow["remarks"]);
                    txtAddress.Text = Val.ToString(Drow["address"]);
                    LookupBroker.EditValue = Val.ToInt32(Drow["broker_id"]);
                    txtGstNo.Text = Val.ToString(Drow["gst_in"]);
                    txtMobileNo.Text = Val.ToString(Drow["mobile_no"]);
                    txtEmailID.Text = Val.ToString(Drow["email_id"]);
                    LookupCountry.EditValue = Val.ToInt32(Drow["country_id"]);
                    LookupState.EditValue = Val.ToInt32(Drow["state_id"]);
                    LookupCity.EditValue = Val.ToInt32(Drow["city_id"]);
                    txtCreditPeriod.Text = Val.ToString(Drow["credit_period"]);

                    txtPartyName.Focus();
                }
            }
        }

        private void LookupState_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmStateMaster frmCnt = new FrmStateMaster();
                frmCnt.ShowDialog();
                Global.LOOKUPState(LookupState);
            }
        }

        private void LookupCountry_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmCountryMaster frmCnt = new FrmCountryMaster();
                frmCnt.ShowDialog();
                Global.LOOKUPCountry(LookupCountry);
            }
        }

        private void LookupCity_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmCityMaster frmCnt = new FrmCityMaster();
                frmCnt.ShowDialog();
                Global.LOOKUPCity(LookupCity);
            }
        }
        private void LookupCity_EditValueChanged(object sender, EventArgs e)
        {
            LookupState.EditValue = LookupCity.GetColumnValue("state_id");
            LookupCountry.EditValue = LookupCity.GetColumnValue("country_id");
        }
        private void LookupState_EditValueChanged(object sender, EventArgs e)
        {
            LookupCountry.EditValue = LookupState.GetColumnValue("country_id");
        }
        private void MNExportExcel_Click(object sender, EventArgs e)
        {
            Global.Export("xlsx", dgvPartyMaster);
        }
        private void MNExportPDF_Click(object sender, EventArgs e)
        {
            Global.Export("pdf", dgvPartyMaster);
        }

        private void txtCreditPeriod_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
