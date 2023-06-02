using BLL.PropertyClasses.Master;
using DLL;
using System.Data;

namespace BLL.FunctionClasses.Master
{
    public class CustomerMaster
    {
        InterfaceLayer Ope = new InterfaceLayer();
        Validation Val = new Validation();

        #region Params Region

        public int Save(Customer_MasterProperty pClsProperty)
        {
            Request Request = new Request();

            Request.AddParams("@customer_id", pClsProperty.customer_id, DbType.Int64);
            Request.AddParams("@customer_name", pClsProperty.customer_name, DbType.String);
            Request.AddParams("@mobile_no1", pClsProperty.mobile_no1, DbType.String);
            Request.AddParams("@mobile_no2", pClsProperty.mobile_no2, DbType.String);
            Request.AddParams("@refrence_name", pClsProperty.refrence_name, DbType.String);
            Request.AddParams("@refrence_mobile_no", pClsProperty.refrence_mobile_no, DbType.String);
            Request.AddParams("@adhar_card", pClsProperty.adhar_card, DbType.String);
            Request.AddParams("@pancard", pClsProperty.pancard, DbType.String);
            Request.AddParams("@user_id", GlobalDec.gEmployeeProperty.employee_id, DbType.Int32);
            Request.AddParams("@ip_address", GlobalDec.gStrComputerIP, DbType.String);
            Request.AddParams("@entry_date", Val.DBDate(GlobalDec.gStr_SystemDate), DbType.Date);
            Request.AddParams("@entry_time", GlobalDec.gStr_SystemTime, DbType.String);

            Request.CommandText = BLL.TPV.SProc.MST_Customer_Master_Save;
            Request.CommandType = CommandType.StoredProcedure;
            return Ope.ExecuteNonQuery(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, Request);
        }

        public DataTable GetData()
        {
            DataTable DTab = new DataTable();
            Request Request = new Request();

            Request.CommandText = BLL.TPV.SProc.MST_Customer_GetData;
            Request.CommandType = CommandType.StoredProcedure;
            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);
            return DTab;
        }

        #endregion
    }
}

