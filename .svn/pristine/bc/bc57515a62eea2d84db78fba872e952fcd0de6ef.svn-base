using BLL.PropertyClasses.Master;
using DLL;
using System.Data;

namespace BLL.FunctionClasses.Master
{
    public class CreditCardMaster
    {
        InterfaceLayer Ope = new InterfaceLayer();
        Validation Val = new Validation();

        #region Params Region

        public int Save(CreditCard_MasterProperty pClsProperty)
        {
            Request Request = new Request();

            Request.AddParams("@credit_card_id", pClsProperty.credit_card_id, DbType.Int64);
            Request.AddParams("@bank_name", pClsProperty.bank_name, DbType.String);
            Request.AddParams("@credit_card_name", pClsProperty.credit_card_name, DbType.String);
            Request.AddParams("@credit_card_no", pClsProperty.credit_card_no, DbType.String);
            Request.AddParams("@credit_card_pin", pClsProperty.credit_card_pin, DbType.String);
            Request.AddParams("@adhar_card", pClsProperty.adhar_card, DbType.String);
            Request.AddParams("@pancard", pClsProperty.pancard, DbType.String);
            Request.AddParams("@user_id", GlobalDec.gEmployeeProperty.employee_id, DbType.Int32);
            Request.AddParams("@ip_address", GlobalDec.gStrComputerIP, DbType.String);
            Request.AddParams("@entry_date", Val.DBDate(GlobalDec.gStr_SystemDate), DbType.Date);
            Request.AddParams("@entry_time", GlobalDec.gStr_SystemTime, DbType.String);
            Request.AddParams("@customer_id", pClsProperty.customer_id, DbType.Int64);

            Request.CommandText = BLL.TPV.SProc.MST_CreditCard_Master_Save;
            Request.CommandType = CommandType.StoredProcedure;
            return Ope.ExecuteNonQuery(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, Request);
        }

        public DataTable GetData()
        {
            DataTable DTab = new DataTable();
            Request Request = new Request();

            Request.CommandText = BLL.TPV.SProc.MST_CreditCard_GetData;
            Request.CommandType = CommandType.StoredProcedure;
            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);
            return DTab;
        }

        public DataTable Distinct_Bank_GetData()
        {
            DataTable DTab = new DataTable();
            Request Request = new Request();

            Request.CommandText = BLL.TPV.SProc.MST_Distinct_Bank_GetData;
            Request.CommandType = CommandType.StoredProcedure;
            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);
            return DTab;
        }

        #endregion
    }
}

