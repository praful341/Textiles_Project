using BLL;
using BLL.FunctionClasses.Master;
using BLL.PropertyClasses.Master;
using System;
using System.Collections.Generic;
using System.Data;
using Textiles_Project.Class;

namespace Textiles_Project.Master
{
    public partial class FrmTransportMaster : DevExpress.XtraEditors.XtraForm
    {
        BLL.FormEvents objBOFormEvents = new BLL.FormEvents();
        BLL.Validation Val = new BLL.Validation();
        TransportMaster objTransport;

        public FrmTransportMaster()
        {
            InitializeComponent();
            objTransport = new TransportMaster();
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
            txtTransportName.Text = "";
            chkActive.Checked = true;
            txtRemark.Text = "";
            txtTransportName.Focus();
        }

        #region Validation


        private bool ValidateDetails()
        {
            bool blnFocus = false;
            List<ListError> lstError = new List<ListError>();
            try
            {
                if (txtTransportName.Text.Length == 0)
                {
                    Global.Confirm("Transport Name Is Required");
                    txtTransportName.Focus();
                    return false;
                }
                if (!objTransport.ISExists(txtTransportName.Text, Val.ToInt64(lblMode.Tag)).ToString().Trim().Equals(string.Empty))
                {
                    lstError.Add(new ListError(23, "Transport Name"));
                    if (!blnFocus)
                    {
                        blnFocus = true;
                        txtTransportName.Focus();
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

            Transport_MasterProperty TransportMasterProperty = new Transport_MasterProperty();
            TransportMasterProperty.transport_id = Val.ToInt32(lblMode.Tag);
            TransportMasterProperty.transport_name = Val.ToString(txtTransportName.Text).ToUpper();
            TransportMasterProperty.active = Val.ToBoolean(chkActive.Checked);
            TransportMasterProperty.remarks = txtRemark.Text;

            int IntRes = objTransport.Save(TransportMasterProperty);
            if (IntRes == -1)
            {
                Global.Confirm("Error In Save Transport Details");
                txtTransportName.Focus();
            }
            else
            {
                if (Val.ToInt(lblMode.Tag) == 0)
                {
                    Global.Confirm("Transport Details Data Save Successfully");
                }
                else
                {
                    Global.Confirm("Transport Details Data Update Successfully");
                }

                btnClear_Click(sender, e);
                GetData();
            }
            TransportMasterProperty = null;
        }
        public void GetData()
        {
            DataTable DTab = objTransport.GetData();
            grdTransportMaster.DataSource = DTab;
            dgvTransportMaster.BestFitColumns();
        }

        private void dgvTransportMaster_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Clicks == 2)
                {
                    DataRow Drow = dgvTransportMaster.GetDataRow(e.RowHandle);
                    lblMode.Text = "Edit Mode";
                    lblMode.Tag = Val.ToInt64(Drow["transport_id"]);
                    txtTransportName.Text = Val.ToString(Drow["transport_name"]);
                    chkActive.Checked = Val.ToBoolean(Drow["active"]);
                    txtRemark.Text = Val.ToString(Drow["remarks"]);
                    txtTransportName.Focus();
                }
            }
        }

        private void FrmTransportMaster_Load(object sender, EventArgs e)
        {
            GetData();
            chkActive.Checked = true;
            txtTransportName.Focus();
        }

        private void MNExportExcel_Click(object sender, EventArgs e)
        {
            Global.Export("xlsx", dgvTransportMaster);
        }

        private void MNExportPDF_Click(object sender, EventArgs e)
        {
            Global.Export("pdf", dgvTransportMaster);
        }
    }
}
