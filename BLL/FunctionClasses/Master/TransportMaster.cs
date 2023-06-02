using BLL.PropertyClasses.Master;
using DLL;
using System;
using System.Data;

namespace BLL.FunctionClasses.Master
{
    public class TransportMaster
    {
        InterfaceLayer Ope = new InterfaceLayer();
        Validation Val = new Validation();
        public int Save(Transport_MasterProperty pClsProperty)
        {
            int IntRes = 0;
            try
            {
                Request Request = new Request();

                Request.AddParams("@transport_id", pClsProperty.transport_id, DbType.Int64);
                Request.AddParams("@transport_name", pClsProperty.transport_name, DbType.String);
                Request.AddParams("@active", pClsProperty.active, DbType.Int32);
                Request.AddParams("@remarks", pClsProperty.remarks, DbType.String);
                Request.AddParams("@user_id", GlobalDec.gEmployeeProperty.user_id, DbType.Int32);
                Request.AddParams("@ip_address", GlobalDec.gStrComputerIP, DbType.String);
                Request.AddParams("@entry_date", Val.DBDate(GlobalDec.gStr_SystemDate), DbType.Date);
                Request.AddParams("@entry_time", GlobalDec.gStr_SystemTime, DbType.String);
                Request.CommandText = BLL.TPV.SProc.MST_Transport_Save;
                Request.CommandType = CommandType.StoredProcedure;

                Ope.ExecuteNonQuery(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, Request);
            }
            catch (Exception ex)
            {
                General.ShowErrors(ex.ToString());
            }
            return IntRes;
        }

        public DataTable GetData(int active = 0, int IsRej = 0)
        {
            DataTable DTab = new DataTable();
            Request Request = new Request();

            Request.AddParams("@active", active, DbType.Int32);
            Request.CommandText = BLL.TPV.SProc.MST_Transport_GetData;
            Request.CommandType = CommandType.StoredProcedure;

            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);
            return DTab;
        }
        public string ISExists(string Transport, Int64 TransportId)
        {
            Validation Val = new Validation();
            return Val.ToString(Ope.FindText(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, "MST_Transport", "transport_name", "AND transport_name = '" + Transport + "' AND NOT transport_id =" + TransportId));
        }
    }
}
