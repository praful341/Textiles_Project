using BLL.PropertyClasses.Master;
using DLL;
using System;
using System.Data;

namespace BLL.FunctionClasses.Master
{
    public class PartyMaster
    {
        InterfaceLayer Ope = new InterfaceLayer();
        Validation Val = new Validation();
        public int Save(Party_MasterProperty pClsProperty)
        {
            int IntRes = 0;
            try
            {
                Request Request = new Request();

                Request.AddParams("@party_id", pClsProperty.party_id, DbType.Int64);
                Request.AddParams("@party_name", pClsProperty.party_name, DbType.String);
                Request.AddParams("@active", pClsProperty.active, DbType.Int32);
                Request.AddParams("@remarks", pClsProperty.remarks, DbType.String);
                Request.AddParams("@address", pClsProperty.address, DbType.String); ;
                Request.AddParams("@company_id", GlobalDec.gEmployeeProperty.company_id, DbType.Int64);
                Request.AddParams("@branch_id", GlobalDec.gEmployeeProperty.branch_id, DbType.Int64);
                Request.AddParams("@location_id", GlobalDec.gEmployeeProperty.location_id, DbType.Int64);
                Request.AddParams("@department_id", GlobalDec.gEmployeeProperty.department_id, DbType.Int64);
                Request.AddParams("@user_id", GlobalDec.gEmployeeProperty.user_id, DbType.Int32);
                Request.AddParams("@ip_address", GlobalDec.gStrComputerIP, DbType.String);
                Request.AddParams("@entry_date", Val.DBDate(GlobalDec.gStr_SystemDate), DbType.Date);
                Request.AddParams("@entry_time", GlobalDec.gStr_SystemTime, DbType.String);
                Request.AddParams("@broker_id", pClsProperty.broker_id, DbType.Int64);
                Request.AddParams("@gst_no", pClsProperty.gst_no, DbType.String);

                Request.AddParams("@mobile_no", pClsProperty.mobile_no, DbType.String, ParameterDirection.Input);
                Request.AddParams("@email_id", pClsProperty.email_id, DbType.String, ParameterDirection.Input);
                Request.AddParams("@country_id", pClsProperty.country_id, DbType.Int32, ParameterDirection.Input);
                Request.AddParams("@state_id", pClsProperty.state_id, DbType.Int32, ParameterDirection.Input);
                Request.AddParams("@city_id", pClsProperty.city_id, DbType.Int32, ParameterDirection.Input);
                Request.AddParams("@credit_period", pClsProperty.credit_period, DbType.Int32);

                Request.CommandText = BLL.TPV.SProc.MST_Party_Save;
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
            Request.AddParams("@company_id", GlobalDec.gEmployeeProperty.company_id, DbType.Int32);
            Request.AddParams("@location_id", GlobalDec.gEmployeeProperty.location_id, DbType.Int32);
            Request.CommandText = BLL.TPV.SProc.MST_Party_GetData;
            Request.CommandType = CommandType.StoredProcedure;

            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);
            return DTab;
        }
        public int Broker_Save(Party_MasterProperty pClsProperty)
        {
            int IntRes = 0;
            try
            {
                Request Request = new Request();

                Request.AddParams("@broker_id", pClsProperty.broker_id, DbType.Int64);
                Request.AddParams("@broker_name", pClsProperty.broker_name, DbType.String);
                Request.AddParams("@active", pClsProperty.active, DbType.Int32);
                Request.AddParams("@remarks", pClsProperty.remarks, DbType.String);
                Request.AddParams("@address", pClsProperty.address, DbType.String);
                Request.AddParams("@user_id", GlobalDec.gEmployeeProperty.user_id, DbType.Int32);
                Request.AddParams("@ip_address", GlobalDec.gStrComputerIP, DbType.String);
                Request.AddParams("@entry_date", Val.DBDate(GlobalDec.gStr_SystemDate), DbType.Date);
                Request.AddParams("@entry_time", GlobalDec.gStr_SystemTime, DbType.String);
                Request.AddParams("@pancard_no", pClsProperty.pancard_no, DbType.String);
                Request.CommandText = BLL.TPV.SProc.MST_Broker_Save;
                Request.CommandType = CommandType.StoredProcedure;

                Ope.ExecuteNonQuery(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, Request);
            }
            catch (Exception ex)
            {
                General.ShowErrors(ex.ToString());
            }
            return IntRes;
        }
        public DataTable Broker_GetData(int active = 0)
        {
            DataTable DTab = new DataTable();
            Request Request = new Request();

            Request.AddParams("@active", active, DbType.Int32);
            Request.CommandText = BLL.TPV.SProc.MST_Broker_GetData;
            Request.CommandType = CommandType.StoredProcedure;

            Ope.GetDataTable(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, DTab, Request);
            return DTab;
        }
        public string ISExists(string Party, Int64 PartyId)
        {
            Validation Val = new Validation();
            return Val.ToString(Ope.FindText(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, "MST_Party", "party_name", "AND party_name = '" + Party + "' AND NOT party_id =" + PartyId));
        }
        public string Broker_ISExists(string Broker, Int64 BrokerId)
        {
            Validation Val = new Validation();
            return Val.ToString(Ope.FindText(BLL.DBConnections.ConnectionString, BLL.DBConnections.ProviderName, "MST_Broker", "broker_name", "AND broker_name = '" + Broker + "' AND NOT broker_id =" + BrokerId));
        }
    }
}
