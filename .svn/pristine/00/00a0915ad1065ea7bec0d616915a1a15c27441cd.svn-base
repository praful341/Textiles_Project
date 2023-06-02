using BLL;
using BLL.FunctionClasses.Master;
using BLL.PropertyClasses.Master;
using System;
using System.Collections.Generic;
using System.Data;
using Textiles_Project.Class;

namespace Textiles_Project.Master
{
    public partial class FrmBrokerMaster : DevExpress.XtraEditors.XtraForm
    {
        BLL.FormEvents objBOFormEvents = new BLL.FormEvents();
        BLL.Validation Val = new BLL.Validation();
        PartyMaster objParty;

        public FrmBrokerMaster()
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
            txtBrokerName.Text = "";
            chkActive.Checked = true;
            txtRemark.Text = "";
            txtAddress.Text = "";
            txtPanNo.Text = "";
            txtBrokerName.Focus();
        }

        #region Validation


        private bool ValidateDetails()
        {
            bool blnFocus = false;
            List<ListError> lstError = new List<ListError>();
            try
            {
                if (txtBrokerName.Text.Length == 0)
                {
                    Global.Confirm("Broker Name Is Required");
                    txtBrokerName.Focus();
                    return false;
                }
                if (!objParty.Broker_ISExists(txtBrokerName.Text, Val.ToInt64(lblMode.Tag)).ToString().Trim().Equals(string.Empty))
                {
                    lstError.Add(new ListError(23, "Broker Name"));
                    if (!blnFocus)
                    {
                        blnFocus = true;
                        txtBrokerName.Focus();
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
            PartyMasterProperty.broker_id = Val.ToInt64(lblMode.Tag);
            PartyMasterProperty.broker_name = Val.ToString(txtBrokerName.Text).ToUpper();
            PartyMasterProperty.active = Val.ToBoolean(chkActive.Checked);
            PartyMasterProperty.remarks = txtRemark.Text;
            PartyMasterProperty.address = Val.ToString(txtAddress.Text);
            PartyMasterProperty.pancard_no = Val.ToString(txtPanNo.Text);

            int IntRes = objParty.Broker_Save(PartyMasterProperty);
            if (IntRes == -1)
            {
                Global.Confirm("Error In Save Broker Details");
                txtBrokerName.Focus();
            }
            else
            {
                if (Val.ToInt(lblMode.Tag) == 0)
                {
                    Global.Confirm("Broker Details Data Save Successfully");
                }
                else
                {
                    Global.Confirm("Broker Details Data Update Successfully");
                }

                btnClear_Click(sender, e);
                GetData();
            }
            PartyMasterProperty = null;
        }
        public void GetData()
        {
            DataTable DTab = objParty.Broker_GetData();
            grdBrokerMaster.DataSource = DTab;
            dgvBrokerMaster.BestFitColumns();
        }
        private void FrmBrokerMaster_Load(object sender, EventArgs e)
        {
            GetData();
            chkActive.Checked = true;
            txtBrokerName.Focus();
        }
        private void dgvLedgerMaster_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Clicks == 2)
                {
                    DataRow Drow = dgvBrokerMaster.GetDataRow(e.RowHandle);
                    lblMode.Text = "Edit Mode";
                    lblMode.Tag = Val.ToInt64(Drow["broker_id"]);
                    txtBrokerName.Text = Val.ToString(Drow["broker_name"]);
                    chkActive.Checked = Val.ToBoolean(Drow["active"]);
                    txtRemark.Text = Val.ToString(Drow["remarks"]);
                    txtAddress.Text = Val.ToString(Drow["address"]);
                    txtPanNo.Text = Val.ToString(Drow["pancard_no"]);
                    txtBrokerName.Focus();
                }
            }
        }

        private void MNExportExcel_Click(object sender, EventArgs e)
        {
            Global.Export("xlsx", dgvBrokerMaster);
        }

        private void MNExportPDF_Click(object sender, EventArgs e)
        {
            Global.Export("pdf", dgvBrokerMaster);
        }
    }
}
