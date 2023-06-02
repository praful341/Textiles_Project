using BLL;
using BLL.FunctionClasses.Master;
using BLL.PropertyClasses.Master;
using System;
using System.Collections.Generic;
using System.Data;
using Textiles_Project.Class;

namespace Textiles_Project.Master
{
    public partial class FrmItemMaster : DevExpress.XtraEditors.XtraForm
    {
        BLL.FormEvents objBOFormEvents = new BLL.FormEvents();
        BLL.Validation Val = new BLL.Validation();
        ItemMaster objItem;

        public FrmItemMaster()
        {
            InitializeComponent();

            objItem = new ItemMaster();
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
            chkActive.Checked = true;
            txtItemName.Text = "";
            txtHSNCode.Text = "";
            txtRemark.Text = "";
            txtItemName.Focus();
        }

        #region Validation


        private bool ValidateDetails()
        {
            bool blnFocus = false;
            List<ListError> lstError = new List<ListError>();
            try
            {
                if (txtItemName.Text.Length == 0)
                {
                    Global.Confirm("Item Name Is Required");
                    txtItemName.Focus();
                    return false;
                }
                if (!objItem.ISExists(txtItemName.Text, Val.ToInt64(lblMode.Tag)).ToString().Trim().Equals(string.Empty))
                {
                    lstError.Add(new ListError(23, "Item Name"));
                    if (!blnFocus)
                    {
                        blnFocus = true;
                        txtItemName.Focus();
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

            Item_MasterProperty ItemMasterProperty = new Item_MasterProperty();
            ItemMasterProperty.item_id = Val.ToInt32(lblMode.Tag);
            ItemMasterProperty.item_name = Val.ToString(txtItemName.Text).ToUpper();
            ItemMasterProperty.active = Val.ToBoolean(chkActive.Checked);
            ItemMasterProperty.remarks = txtRemark.Text;
            ItemMasterProperty.hsn_code = Val.ToString(txtHSNCode.Text);

            int IntRes = objItem.Save(ItemMasterProperty);
            if (IntRes == -1)
            {
                Global.Confirm("Error In Save Item Details");
                txtItemName.Focus();
            }
            else
            {
                if (Val.ToInt(lblMode.Tag) == 0)
                {
                    Global.Confirm("Item Details Data Save Successfully");
                }
                else
                {
                    Global.Confirm("Item Details Data Update Successfully");
                }

                btnClear_Click(sender, e);
                GetData();
            }
            ItemMasterProperty = null;
        }
        public void GetData()
        {
            DataTable DTab = objItem.GetData();
            grdItemMaster.DataSource = DTab;
            dgvItemMaster.BestFitColumns();
        }

        private void FrmGoDownMaster_Load(object sender, EventArgs e)
        {
            GetData();
            chkActive.Checked = true;
            txtItemName.Focus();
        }

        private void dgvItemMaster_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Clicks == 2)
                {
                    DataRow Drow = dgvItemMaster.GetDataRow(e.RowHandle);
                    lblMode.Text = "Edit Mode";
                    lblMode.Tag = Val.ToInt64(Drow["item_id"]);
                    txtItemName.Text = Val.ToString(Drow["item_name"]);
                    chkActive.Checked = Val.ToBoolean(Drow["active"]);
                    txtRemark.Text = Val.ToString(Drow["remarks"]);
                    txtHSNCode.Text = Val.ToString(Drow["hsn_code"]);
                    txtItemName.Focus();
                }
            }
        }

        private void MNExportExcel_Click(object sender, EventArgs e)
        {
            Global.Export("xlsx", dgvItemMaster);
        }

        private void MNExportPDF_Click(object sender, EventArgs e)
        {
            Global.Export("pdf", dgvItemMaster);
        }
    }
}
