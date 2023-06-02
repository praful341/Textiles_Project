using BLL.PropertyClasses.Transaction;
using DLL;
using System;
using System.Collections;
using System.Data;

namespace BLL.FunctionClasses.Transaction
{
    public class TRNGrayMaster
    {
        #region "Data Member"
        InterfaceLayer Ope = new InterfaceLayer();
        BLL.Validation Val = new BLL.Validation();

        #endregion "Data Member"        

        #region "Functions" 
        public TRNGrayMaster_Property SaveGrayMaster(TRNGrayMaster_Property pClsProperty)
        {
            Request Request = new Request();

            Request.AddParams("@gray_id", pClsProperty.gray_id, DbType.Int64);
            Request.AddParams("@voucher_id", pClsProperty.voucher_id, DbType.Int64);
            Request.AddParams("@receive_date", pClsProperty.receive_date, DbType.Date);
            Request.AddParams("@check_date", pClsProperty.check_date, DbType.Date);
            Request.AddParams("@party_id", pClsProperty.party_id, DbType.Int64);
            Request.AddParams("@party_challan_no", pClsProperty.party_challan_no, DbType.String);
            Request.AddParams("@party_challan_date", pClsProperty.party_challan_date, DbType.Date);
            Request.AddParams("@transport_id", pClsProperty.transport_id, DbType.Int64);
            Request.AddParams("@lr_no", pClsProperty.lr_no, DbType.String);
            Request.AddParams("@transport_date", pClsProperty.transport_date, DbType.Date);
            Request.AddParams("@fold", pClsProperty.fold, DbType.Decimal);
            Request.AddParams("@declare_pcs", pClsProperty.declare_pcs, DbType.Decimal);
            Request.AddParams("@declare_meter", pClsProperty.declare_meter, DbType.Decimal);
            Request.AddParams("@netamount", pClsProperty.netamount, DbType.Decimal);
            Request.AddParams("@remarks", pClsProperty.remarks, DbType.String);
            Request.AddParams("@user_id", GlobalDec.gEmployeeProperty.user_id, DbType.Int32);
            Request.AddParams("@ip_address", GlobalDec.gStrComputerIP, DbType.String);
            Request.AddParams("@entry_date", Val.DBDate(GlobalDec.gStr_SystemDate), DbType.Date);
            Request.AddParams("@entry_time", GlobalDec.gStr_SystemTime, DbType.String);
            Request.AddParams("@company_id", GlobalDec.gEmployeeProperty.company_id, DbType.String);
            Request.AddParams("@branch_id", GlobalDec.gEmployeeProperty.branch_id, DbType.Int32);

            Request.CommandText = BLL.TPV.SProc.Gray_Master_Save;
            Request.CommandType = CommandType.StoredProcedure;

            DataTable DTAB = new DataTable();
            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTAB, Request);

            if (DTAB != null)
            {
                if (DTAB.Rows.Count > 0)
                {
                    pClsProperty.gray_id = Convert.ToInt64(DTAB.Rows[0][0]);
                }
            }
            else
            {
                pClsProperty.gray_id = 0;
            }

            return pClsProperty;
        }
        public Request SaveGrayDetail(TRNGrayMaster_Property pClsProperty)
        {
            Request Request = new Request();
            Request.AddParams("@gray_detail_id", pClsProperty.gray_detail_id, DbType.Int64);
            Request.AddParams("@gray_id", pClsProperty.gray_id, DbType.Int64);
            Request.AddParams("@voucher_id", pClsProperty.voucher_id, DbType.Int64);
            Request.AddParams("@item_id", pClsProperty.item_id, DbType.Int64);
            Request.AddParams("@t_pcs", pClsProperty.t_pcs, DbType.Decimal);
            Request.AddParams("@rec_meter", pClsProperty.rec_meter, DbType.Decimal);
            Request.AddParams("@acc_meter", pClsProperty.acc_meter, DbType.Decimal);
            Request.AddParams("@weight", pClsProperty.weight, DbType.Decimal);
            Request.AddParams("@total_weight", pClsProperty.total_weight, DbType.Decimal);
            Request.AddParams("@rate", pClsProperty.rate, DbType.Decimal);
            Request.AddParams("@amount", pClsProperty.amount, DbType.Decimal);
            Request.AddParams("@godown_id", pClsProperty.godown_id, DbType.Int64);
            Request.AddParams("@remarks", pClsProperty.remarks, DbType.String);
            Request.AddParams("@user_id", GlobalDec.gEmployeeProperty.user_id, DbType.Int32);
            Request.AddParams("@ip_address", GlobalDec.gStrComputerIP, DbType.String);
            Request.AddParams("@entry_date", Val.DBDate(GlobalDec.gStr_SystemDate), DbType.Date);
            Request.AddParams("@entry_time", GlobalDec.gStr_SystemTime, DbType.String);

            return Request;
        }
        public int SavePurchaseDetail(ArrayList AL)
        {
            int IntRes = 0;
            Request Request = new Request();

            foreach (TRNGrayMaster_Property Obj in AL)
            {
                Request = SaveGrayDetail(Obj);
                Request.CommandText = BLL.TPV.SProc.Gray_Detail_SAVE;
                Request.CommandType = CommandType.StoredProcedure;
                IntRes += Ope.ExecuteNonQuery(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, Request);
            }
            return IntRes;
        }
        public DataTable GetGrayDetail(Int64 pIntItemCode = 0)
        {
            Request Request = new Request();
            DataTable DTab = new DataTable();
            Request.AddParams("@voucher_id", pIntItemCode, DbType.Int64);

            Request.CommandText = BLL.TPV.SProc.Gray_Detail_GetData;

            Request.CommandType = CommandType.StoredProcedure;
            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);
            return DTab;
        }
        public int FindNewID()
        {
            int IntRes = 0;
            IntRes = Ope.FindNewID(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, "Gray_Master", "isnull(MAX(gray_id),0)", "");
            return IntRes;
        }
        public DataTable GetData(TRNGrayMaster_Property Property)
        {
            DataTable DTab = new DataTable();
            Request Request = new Request();

            Request.AddParams("@from_date", Property.from_date, DbType.Date);
            Request.AddParams("@to_date", Property.to_date, DbType.Date);

            Request.CommandText = BLL.TPV.SProc.Gray_Master_GetData;
            Request.CommandType = CommandType.StoredProcedure;
            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);
            return DTab;
        }
        public int DeleteGrayDetail(TRNGrayMaster_Property pClsProperty)
        {
            int IntRes = 0;
            Request Request = new Request();

            Request.AddParams("@gray_id", pClsProperty.gray_id, DbType.Int64);
            Request.AddParams("@gray_detail_id", pClsProperty.gray_detail_id, DbType.Int64);

            Request.CommandText = BLL.TPV.SProc.Gray_Master_Detail_Delete;
            Request.CommandType = CommandType.StoredProcedure;
            IntRes += Ope.ExecuteNonQuery(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, Request);

            return IntRes;
        }
        public int DeleteGrayEntryMaster(TRNGrayMaster_Property pClsProperty)
        {
            Request Request = new Request();
            Request.AddParams("@voucher_id", pClsProperty.voucher_id, DbType.Int64);
            Request.CommandText = BLL.TPV.SProc.GrayMaster_Delete_GetData;
            Request.CommandType = CommandType.StoredProcedure;
            return Ope.ExecuteNonQuery(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, Request);
        }

        public DataTable GetGrayItemDetail(TRNGrayMaster_Property PClsProperty)
        {
            DataTable DTabVal = new DataTable();
            Request Request = new Request();

            Request.AddParams("@item_id", PClsProperty.item_id, DbType.Int32);
            Request.AddParams("@voucher_id", PClsProperty.voucher_id, DbType.Int32);
            Request.AddParams("@gray_detail_id", PClsProperty.gray_detail_id, DbType.Int32);

            Request.CommandText = BLL.TPV.SProc.TRN_Gray_ItemDetail_GetData;
            Request.CommandType = CommandType.StoredProcedure;
            Ope.GetDataTable(DBConnections.ConnectionString, DBConnections.ProviderName, DTabVal, Request);

            return DTabVal;
        }

        public int GrayItemDetail_Save(TRNGrayMaster_Property pClsProperty)
        {
            int IntRes = 0;
            Request Request = new Request();

            Request.AddParams("@gray_item_detail_id", pClsProperty.gray_item_detail_id, DbType.Int64);
            Request.AddParams("@voucher_id", pClsProperty.voucher_id, DbType.Int64);
            Request.AddParams("@gray_detail_id", pClsProperty.gray_detail_id, DbType.Int64);
            Request.AddParams("@item_id", pClsProperty.item_id, DbType.Int64);
            Request.AddParams("@taka_no", pClsProperty.taka_no, DbType.Int64);
            Request.AddParams("@rec_meter", pClsProperty.rec_meter, DbType.Decimal);
            Request.AddParams("@acc_meter", pClsProperty.acc_meter, DbType.Decimal);
            Request.AddParams("@pcs", pClsProperty.pcs, DbType.String);
            Request.AddParams("@remarks", pClsProperty.remarks, DbType.String);

            Request.CommandText = BLL.TPV.SProc.Gray_Item_Detail_SAVE;
            Request.CommandType = CommandType.StoredProcedure;
            IntRes += Ope.ExecuteNonQuery(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, Request);

            return IntRes;
        }
        public DataTable GetGrayReceipt_PrintData(TRNGrayMaster_Property pClsProperty)
        {
            DataTable DTab = new DataTable();
            Request Request = new Request();

            //Request.AddParams("@voucher_id", pClsProperty.voucher_id, DbType.Int64);
            Request.AddParams("@voucher_name", pClsProperty.voucher_name, DbType.String);
            Request.AddParams("@gray_detail_name", pClsProperty.gray_detail_name, DbType.String);

            Request.CommandText = BLL.TPV.SProc.Gray_Received_Note_RPT;
            Request.CommandType = CommandType.StoredProcedure;

            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);

            return DTab;
        }
        public DataSet GetGrayChecking_PrintData(TRNGrayMaster_Property pClsProperty)
        {
            DataSet DTab = new DataSet();
            Request Request = new Request();

            Request.AddParams("@voucher_name", pClsProperty.voucher_name, DbType.String);
            Request.AddParams("@gray_detail_name", pClsProperty.gray_detail_name, DbType.String);

            Request.CommandText = BLL.TPV.SProc.Gray_Received_NotPrint_RPT;
            Request.CommandType = CommandType.StoredProcedure;

            Ope.GetDataSet(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, "", Request);

            return DTab;
        }

        #endregion"Functions"
    }
}
