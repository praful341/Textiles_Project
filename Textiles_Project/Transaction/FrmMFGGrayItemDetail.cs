using BLL;
using BLL.FunctionClasses.Transaction;
using BLL.PropertyClasses.Transaction;
using System;
using System.Data;
using System.Windows.Forms;
using Textiles_Project.Class;
using static Textiles_Project.Class.Global;

namespace Textiles_Project.Transaction
{
    public partial class FrmMFGGrayItemDetail : DevExpress.XtraEditors.XtraForm
    {
        #region Declaration


        Validation Val = new Validation();

        DataTable dtPrint = new DataTable();

        public DataTable DTab = new DataTable();
        public string ColumnsToHide = "";
        public bool AllowMultiSelect = false;
        public string ColumnHeaderCaptions = "";
        public string SearchText = "";
        public string SearchField = "";
        public string ValueMember = "";
        public string SelectedValue = "";

        FormEvents objBOFormEvents = new FormEvents();
        public FrmTRNGrayMaster FrmTRNGrayMaster = new FrmTRNGrayMaster();
        string FormName = "";
        Int64 Item_id_Det = 0;
        Int64 Voucher_ID_Det = 0;
        Int64 Gray_Detail_ID_Det = 0;

        #endregion

        #region Constructor
        public FrmMFGGrayItemDetail()
        {
            InitializeComponent();
        }
        public void ShowForm(FrmTRNGrayMaster ObjForm, Int64 Item_id, Int64 Voucher_ID, Int64 Gray_Detail_ID)
        {
            FrmTRNGrayMaster = new FrmTRNGrayMaster();
            FrmTRNGrayMaster = ObjForm;
            FormName = "FrmTRNGrayMaster";

            Item_id_Det = Item_id;
            Voucher_ID_Det = Voucher_ID;
            Gray_Detail_ID_Det = Gray_Detail_ID;

            Val.frmGenSetForPopup(this);
            AttachFormEvents();
            this.ShowDialog();
        }

        private void AttachFormEvents()
        {
            objBOFormEvents.CurForm = this;
            objBOFormEvents.FormKeyPress = true;
            objBOFormEvents.FormResize = true;
            objBOFormEvents.FormClosing = true;
            objBOFormEvents.ObjToDispose.Add(Val);
            objBOFormEvents.ObjToDispose.Add(objBOFormEvents);
        }

        #endregion

        #region Form Events
        private void FrmJangedConfirm_Load(object sender, EventArgs e)
        {
            try
            {
                MainGrid.DataSource = DTab;
                MainGrid.RefreshDataSource();
                GrdDet.BestFitColumns();
            }
            catch (Exception ex)
            {
                Global.ErrorMessage(ex.Message);
            }
        }
        private void FrmJangedConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        #endregion

        #region Events

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Other Function

        private void Export(string format, string dlgHeader, string dlgFilter)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = format;
                svDialog.Title = dlgHeader;
                svDialog.FileName = "Report";
                svDialog.Filter = dlgFilter;
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    string Filepath = svDialog.FileName;

                    switch (format)
                    {
                        case "pdf":
                            GrdDet.ExportToPdf(Filepath);
                            break;
                        case "xls":
                            GrdDet.ExportToXls(Filepath);
                            break;
                        case "xlsx":
                            GrdDet.ExportToXlsx(Filepath);
                            break;
                        case "rtf":
                            GrdDet.ExportToRtf(Filepath);
                            break;
                        case "txt":
                            GrdDet.ExportToText(Filepath);
                            break;
                        case "html":
                            GrdDet.ExportToHtml(Filepath);
                            break;
                        case "csv":
                            GrdDet.ExportToCsv(Filepath);
                            break;
                    }

                    if (format.Equals(Exports.xlsx.ToString()))
                    {
                        if (Global.Confirm("Export Done\n\nYou Want To Open Excel File ?", "DERP", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(Filepath);
                        }
                    }
                    else if (format.Equals(Exports.pdf.ToString()))
                    {
                        if (Global.Confirm("Export Done\n\nYou Want To Open PDF File ?", "DERP", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(Filepath);
                        }
                    }
                    else
                    {
                        if (Global.Confirm("Export Done\n\nYou Want To Open File ?", "DERP", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(Filepath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString(), "Error in Export");
            }
        }
        #endregion

        #region Repository Events

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure To Item Detail Save ?", "Textile Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            decimal rec_meter = 0;
            decimal acc_meter = 0;
            Int64 item_id = 0;
            if (FormName == "FrmTRNGrayMaster")
            {
                int IntRes = 0;
                DTab.AcceptChanges();
                DTab = (DataTable)MainGrid.DataSource;

                foreach (DataRow DRow in DTab.Rows)
                {
                    TRNGrayMaster objTRNGrayMaster = new TRNGrayMaster();
                    TRNGrayMaster_Property objTRNGrayMaster_Property = new TRNGrayMaster_Property();
                    objTRNGrayMaster_Property.gray_item_detail_id = Val.ToInt64(DRow["gray_item_detail_id"]);
                    objTRNGrayMaster_Property.item_id = Val.ToInt64(Item_id_Det);
                    objTRNGrayMaster_Property.voucher_id = Val.ToInt64(Voucher_ID_Det);
                    objTRNGrayMaster_Property.gray_detail_id = Val.ToInt64(Gray_Detail_ID_Det);
                    objTRNGrayMaster_Property.taka_no = Val.ToInt64(DRow["taka_no"]);
                    objTRNGrayMaster_Property.rec_meter = Val.ToDecimal(DRow["rec_meter"]);
                    objTRNGrayMaster_Property.acc_meter = Val.ToDecimal(DRow["acc_meter"]);
                    objTRNGrayMaster_Property.pcs = Val.ToString(DRow["pcs"]);
                    objTRNGrayMaster_Property.remarks = Val.ToString(DRow["remarks"]);

                    IntRes = objTRNGrayMaster.GrayItemDetail_Save(objTRNGrayMaster_Property);
                }

                if (IntRes != 0)
                {
                    Global.Confirm("Item Detail Data Save Successfully");
                }
                else
                {
                    Global.Confirm("Error in Data Save");
                }

                if (DTab.Rows.Count > 0)
                {
                    rec_meter = Val.ToDecimal(GrdDet.Columns["rec_meter"].SummaryText);
                    acc_meter = Val.ToDecimal(GrdDet.Columns["acc_meter"].SummaryText);
                    item_id = Val.ToInt64(Voucher_ID_Det);
                }

                this.Close();

                FrmTRNGrayMaster.GetItemData(rec_meter, acc_meter, item_id);
            }
        }

        #endregion

        #region Export Grid
        private void MNExportExcel_Click(object sender, EventArgs e)
        {
            Export("xlsx", "Export to Excel", "Excel files 97-2003 (Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*");
        }
        private void MNExportPDF_Click(object sender, EventArgs e)
        {
            Export("pdf", "Export Report to PDF", "PDF (*.PDF)|*.PDF");
        }
        private void MNExportTEXT_Click(object sender, EventArgs e)
        {
            Export("txt", "Export to Text", "Text files (*.txt)|*.txt|All files (*.*)|*.*");
        }

        private void MNExportHTML_Click(object sender, EventArgs e)
        {
            Export("html", "Export to HTML", "Html files (*.html)|*.html|Htm files (*.htm)|*.htm");
        }

        private void MNExportRTF_Click(object sender, EventArgs e)
        {
            Export("rtf", "Export to RTF", "Word (*.doc) |*.doc;*.rtf|(*.txt) |*.txt|(*.*) |*.*");
        }

        private void MNExportCSV_Click(object sender, EventArgs e)
        {
            Export("csv", "Export Report to CSVB", "csv (*.csv)|*.csv");
        }

        #endregion

        private void GrdDet_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                DataTable dtAmount = new DataTable();
                dtAmount = (DataTable)MainGrid.DataSource;
                int percentage = 0;
                int column = 0;
                for (int j = 0; j <= dtAmount.Rows.Count - 1; j++)
                {
                    if (dtAmount.Rows[j]["pcs"].ToString() != "")
                    {
                        column = column + 1;
                    }
                    if (column > 0)
                    {
                        percentage = column;
                        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                            e.TotalValue = percentage;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.General.ShowErrors(ex);
            }
        }
    }
}