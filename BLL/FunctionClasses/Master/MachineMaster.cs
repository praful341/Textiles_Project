using BLL.PropertyClasses.Master;
using DLL;
using System;
using System.Data;
namespace BLL.FunctionClasses.Master
{
    public class MachineMaster
    {
        InterfaceLayer Ope = new InterfaceLayer();
        Validation Val = new Validation();

        #region Other Function

        public int Save(Machine_MasterProperty pClsProperty)
        {
            Request Request = new Request();

            Request.AddParams("@machine_id", pClsProperty.machine_id, DbType.Int64);
            Request.AddParams("@machine_name", pClsProperty.machine_name, DbType.String);
            Request.AddParams("@machine_per", pClsProperty.machine_per, DbType.String);
            Request.AddParams("@user_id", GlobalDec.gEmployeeProperty.employee_id, DbType.Int32);
            Request.AddParams("@ip_address", GlobalDec.gStrComputerIP, DbType.String);
            Request.AddParams("@entry_date", Val.DBDate(GlobalDec.gStr_SystemDate), DbType.Date);
            Request.AddParams("@entry_time", GlobalDec.gStr_SystemTime, DbType.String);

            Request.CommandText = BLL.TPV.SProc.MST_Machine_Save;
            Request.CommandType = CommandType.StoredProcedure;
            return Ope.ExecuteNonQuery(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, Request);
        }

        public DataTable GetData_Search()
        {
            DataTable DTab = new DataTable();
            Request Request = new Request();

            Request.CommandText = BLL.TPV.SProc.MST_Machine_GetData;
            Request.CommandType = CommandType.StoredProcedure;
            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);
            return DTab;
        }

        public string ISExists(string Machine_Name, Int64 Machine_ID)
        {
            Validation Val = new Validation();
            return Val.ToString(Ope.FindText(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, "MST_Machine", "machine_name", "AND machine_name = '" + Machine_Name + "' AND NOT machine_id =" + Machine_ID));
        }

        #endregion
    }
}
