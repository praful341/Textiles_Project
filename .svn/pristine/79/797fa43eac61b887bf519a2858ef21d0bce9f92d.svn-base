using BLL;
using BLL.FunctionClasses.Master;
using BLL.PropertyClasses.Master;
using System;
using System.Collections.Generic;
using System.Data;
using Textiles_Project.Class;

namespace Textiles_Project.Master
{
    public partial class FrmGoDownMaster : DevExpress.XtraEditors.XtraForm
    {
        BLL.FormEvents objBOFormEvents = new BLL.FormEvents();
        BLL.Validation Val = new BLL.Validation();
        GoDownMaster objGoDown;

        public FrmGoDownMaster()
        {
            InitializeComponent();

            objGoDown = new GoDownMaster();
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
            txtGoDownName.Text = "";
            chkActive.Checked = true;
            txtRemark.Text = "";
            txtAddress.Text = "";
            txtGoDownName.Focus();
        }

        #region Validation


        private bool ValidateDetails()
        {
            bool blnFocus = false;
            List<ListError> lstError = new List<ListError>();
            try
            {
                if (txtGoDownName.Text.Length == 0)
                {
                    Global.Confirm("GoDown Name Is Required");
                    txtGoDownName.Focus();
                    return false;
                }
                if (!objGoDown.ISExists(txtGoDownName.Text, Val.ToInt64(lblMode.Tag)).ToString().Trim().Equals(string.Empty))
                {
                    lstError.Add(new ListError(23, "GoDown Name"));
                    if (!blnFocus)
                    {
                        blnFocus = true;
                        txtGoDownName.Focus();
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

            GoDown_MasterProperty GoDownMasterProperty = new GoDown_MasterProperty();
            GoDownMasterProperty.godown_id = Val.ToInt32(lblMode.Tag);
            GoDownMasterProperty.godown_name = Val.ToString(txtGoDownName.Text).ToUpper();
            GoDownMasterProperty.active = Val.ToBoolean(chkActive.Checked);
            GoDownMasterProperty.remarks = txtRemark.Text;
            GoDownMasterProperty.address = Val.ToString(txtAddress.Text);

            int IntRes = objGoDown.Save(GoDownMasterProperty);
            if (IntRes == -1)
            {
                Global.Confirm("Error In Save GoDown Details");
                txtGoDownName.Focus();
            }
            else
            {
                if (Val.ToInt(lblMode.Tag) == 0)
                {
                    Global.Confirm("GoDown Details Data Save Successfully");
                }
                else
                {
                    Global.Confirm("GoDown Details Data Update Successfully");
                }

                btnClear_Click(sender, e);
                GetData();
            }
            GoDownMasterProperty = null;
        }
        public void GetData()
        {
            DataTable DTab = objGoDown.GetData();
            grdGoDownMaster.DataSource = DTab;
            dgvGoDownMaster.BestFitColumns();
        }

        private void FrmGoDownMaster_Load(object sender, EventArgs e)
        {
            GetData();
            chkActive.Checked = true;
            txtGoDownName.Focus();
        }

        private void dgvGoDownMaster_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Clicks == 2)
                {
                    DataRow Drow = dgvGoDownMaster.GetDataRow(e.RowHandle);
                    lblMode.Text = "Edit Mode";
                    lblMode.Tag = Val.ToInt64(Drow["godown_id"]);
                    txtGoDownName.Text = Val.ToString(Drow["godown_name"]);
                    chkActive.Checked = Val.ToBoolean(Drow["active"]);
                    txtRemark.Text = Val.ToString(Drow["remarks"]);
                    txtAddress.Text = Val.ToString(Drow["address"]);
                    txtGoDownName.Focus();
                }
            }
        }

        private void MNExportExcel_Click(object sender, EventArgs e)
        {
            Global.Export("xlsx", dgvGoDownMaster);
        }

        private void MNExportPDF_Click(object sender, EventArgs e)
        {
            Global.Export("pdf", dgvGoDownMaster);
        }
    }
}
