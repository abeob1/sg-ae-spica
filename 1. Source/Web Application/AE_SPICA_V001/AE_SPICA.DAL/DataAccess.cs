using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace AE_SPICA.DAL
{
    public class DataAccess
    {
        public string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
        public string SAPconnection = ConfigurationManager.ConnectionStrings["sapConnection"].ToString();
        public string sql = string.Empty;
        public SqlDataAdapter oSQLAdapter = new SqlDataAdapter();
        public SqlCommand oSQLCommand = new SqlCommand();
        public SqlConnection oConnection = new SqlConnection();
        public DataSet oDataset = new DataSet();
        public string sQueryString = string.Empty;
        clsLog oLog = new clsLog();

        public Int16 p_iDebugMode = DEBUG_ON;

        public const Int16 RTN_SUCCESS = 1;
        public const Int16 RTN_ERROR = 0;
        public const Int16 DEBUG_ON = 1;
        public const Int16 DEBUG_OFF = 0;
        public string sErrDesc = string.Empty;

        public void InsertUpdateDeleteSQLString(string sqlstring)
        {
            try
            {
                SqlConnection objsqlconn = new SqlConnection(connection);
                objsqlconn.Open();
                SqlCommand objcmd = new SqlCommand(sqlstring, objsqlconn);
                objcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteSqlString(string sqlstring)
        {
            try
            {
                SqlConnection objsqlconn = new SqlConnection(connection);
                objsqlconn.Open();
                DataSet ds = new DataSet();
                SqlCommand objcmd = new SqlCommand(sqlstring, objsqlconn);
                SqlDataAdapter objAdp = new SqlDataAdapter(objcmd);
                objAdp.Fill(ds);
                objsqlconn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet Run_StoredProcedure(string sProcedureName)
        {

            oConnection = new SqlConnection(connection);
            oDataset = new DataSet();
            sQueryString = sProcedureName;
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "Run_StoredProcedure()";

                //if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Starting Function ", sFuncName);

                //if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sProcedureName, sFuncName);

                if (oConnection.State == ConnectionState.Closed)
                    oConnection.Open();

                oSQLCommand = new SqlCommand(sQueryString, oConnection);
                oSQLAdapter.SelectCommand = oSQLCommand;
                oSQLCommand.CommandTimeout = 0;
                oSQLAdapter.Fill(oDataset);
                oSQLAdapter.Dispose();
                oSQLCommand.Dispose();
                oConnection.Close();

                //if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With SUCCESS ", sFuncName);
                return oDataset;

            }
            catch (Exception Ex)
            {

                sErrDesc = Ex.Message.ToString();
                //if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                //if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw Ex;

            }
            finally
            {
                oConnection = null;
            }
        }

        public object ExecuteSAPSqlString(string sqlstring, string sSAPDBName)
        {
            try
            {
                string[] sConnString = SAPconnection.Split(';');
                string sModifiedString = sConnString[0] + ";" + sSAPDBName + ";" + sConnString[2] + ";" + sConnString[3] + ";" + sConnString[4] + ";" + sConnString[5] + ";" + sConnString[6] + ";" + sConnString[7];

                SqlConnection objsqlconn = new SqlConnection(sModifiedString);
                objsqlconn.Open();
                DataSet ds = new DataSet();
                SqlCommand objcmd = new SqlCommand(sqlstring, objsqlconn);
                SqlDataAdapter objAdp = new SqlDataAdapter(objcmd);
                objAdp.Fill(ds);
                objsqlconn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet Run_QueryString(string sQuery, string sSAPDBName)
        {
            string sConstr = string.Empty;
            string[] sArray = SAPconnection.Split(';');
            string sSplitDB = sArray[1].Split('=').First();

            sConstr = sArray[0] + ";" + sSplitDB + "=" + sSAPDBName + ";" + sArray[2] + ";" + sArray[3] + ";";

            oConnection = new SqlConnection(sConstr);
            sQueryString = sQuery;
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "Run_QueryString()";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sQuery, sFuncName);

                if (oConnection.State == ConnectionState.Closed)
                    oConnection.Open();

                oSQLCommand = new SqlCommand(sQueryString, oConnection);
                oSQLAdapter.SelectCommand = oSQLCommand;
                oSQLCommand.CommandTimeout = 0;
                oSQLAdapter.Fill(oDataset);
                oSQLAdapter.Dispose();
                oSQLCommand.Dispose();
                oConnection.Close();

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With SUCCESS ", sFuncName);

                return oDataset;

            }
            catch (Exception Ex)
            {

                sErrDesc = Ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
            finally
            {
                oConnection = null;
            }
        }

        public void openConnection()
        {
            SqlConnection objsqlconn = new SqlConnection(connection);
            objsqlconn.Open();
        }

        public void closeConnection()
        {
            SqlConnection objsqlconn = new SqlConnection(connection);
            objsqlconn.Close();
        }
    }
}
