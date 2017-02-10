using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AE_SPICA.DAL
{
    public class clsBillingRate
    {
        #region Objects

        public DataAccess oDataAccess = new DataAccess();
        public string sql = string.Empty;
        clsLog oLog = new clsLog();
        public string sResult = string.Empty;
        string sFuncName = string.Empty;
        DataSet dsResult = new DataSet();
        GenerateSQL oGenSQL = new GenerateSQL();
        string sCurrency = string.Empty;

        public Int16 p_iDebugMode = DEBUG_ON;

        public const Int16 RTN_SUCCESS = 1;
        public const Int16 RTN_ERROR = 0;
        public const Int16 DEBUG_ON = 1;
        public const Int16 DEBUG_OFF = 0;
        public string sErrDesc = string.Empty;

        #endregion

        #region Methods

        public DataSet GetCompanyDetails()
        {
            try
            {
                sFuncName = "GetCompanyDetails()";

                sql = "SELECT '-- Select --' Code, '-- Select --' Name UNION ALL SELECT Code, Name FROM tbl_CompanyData WHERE ISNULL(IsActive,'Y') = 'Y'";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw ex;
            }
            return dsResult;
        }

        public DataSet GetUsers(string sCompany)
        {
            try
            {
                sFuncName = "GetUsers()";

                sql = "SELECT '-- Select --' Code, '-- Select --' Name UNION ALL SELECT UserCode [Code], UserName [Name] FROM tbl_Users WHERE CompanyCode = '" + sCompany + "' AND ISNULL(IsActive,'Y') = 'Y'";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw ex;
            }
            return dsResult;
        }

        public string GetCurrency(string sCompany)
        {
            try
            {
                sFuncName = "GetCurrency()";

                sql = "SELECT Currency FROM tbl_CompanyData WHERE Code = '" + sCompany + "' AND ISNULL(IsActive,'Y') = 'Y'";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);
                DataSet dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
                if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                {
                    sCurrency = dsResult.Tables[0].Rows[0]["Currency"].ToString();
                }
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw ex;
            }
            return sCurrency;
        }

        public string CreateBillingRate(DataTable dt)
        {
            try
            {
                sFuncName = "CreateBillingRate()";

                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                sql = "SELECT * from tbl_Billing where CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "' and UserCode = '" + dt.Rows[0]["UserCode"].ToString() + "'";
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);

                if (dsResult != null && dsResult.Tables[0].Rows.Count == 0)
                {

                    string sParameter = "'" + dt.Rows[0]["UserCode"].ToString() + "','" + dt.Rows[0]["CompanyCode"].ToString() + "','" + dt.Rows[0]["Time"].ToString() + "'," +
                    "'" + dt.Rows[0]["BillableUnit"].ToString() + "','" + dt.Rows[0]["HourlyRate"].ToString() + "','" + dt.Rows[0]["Currency"].ToString() + "'";

                    string sInsertQuery = "insert into tbl_Billing(UserCode,CompanyCode,Time,BillableUnit,HourlyRate,Currency) values(" + sParameter + ")";

                    SqlCommand cmd = new SqlCommand(sInsertQuery, objsqlconn);
                    cmd.CommandType = CommandType.Text;
                    objsqlconn.Open();
                    cmd.ExecuteNonQuery();
                    objsqlconn.Close();
                    sResult = "INSERT";
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
                }
                else
                {
                    sResult = "Billing Rate configuration already Exists";
                }
                return sResult;
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw ex;
            }
        }

        public DataSet SearchUser(string sUserName)
        {
            try
            {
                sFuncName = "SearchUser()";

                sql = "SELECT ID,CompanyCode,UserCode [UserCode],(select Name from tbl_CompanyData where Code = a.CompanyCode) " +
                      "[CompanyName],(select UserName from tbl_Users where UserCode = a.UserCode and CompanyCode = a.CompanyCode) [UserName], " +
                      " [Time],BillableUnit,HourlyRate,Currency FROM tbl_Billing a " +
                      " WHERE UserCode in (select UserCode from tbl_Users where UserName like '%" + sUserName + "%') Order by ID";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw ex;
            }
            return dsResult;
        }

        public string UpdateBillingRate(DataTable dt)
        {
            try
            {
                sFuncName = "UpdateBillingRate()";

                //sql = "SELECT * from tbl_Billing where CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "' and UserCode = '" + dt.Rows[0]["UserCode"].ToString() + "'";
                //dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);

                //if (dsResult != null && dsResult.Tables[0].Rows.Count == 0)
                //{
                string sUpdateQuery = "Update tbl_Billing SET CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "' ,UserCode = '" + dt.Rows[0]["UserCode"].ToString() + "'" +
                    ",Time = '" + dt.Rows[0]["Time"].ToString() + "',BillableUnit = '" + dt.Rows[0]["BillableUnit"].ToString() + "'" +
                    ",HourlyRate = '" + dt.Rows[0]["HourlyRate"].ToString() + "',Currency = '" + dt.Rows[0]["Currency"].ToString() + "' where Id = '" + dt.Rows[0]["Id"].ToString() + "'";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sUpdateQuery, sFuncName);

                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);
                SqlCommand cmd = new SqlCommand(sUpdateQuery, objsqlconn);
                cmd.CommandType = CommandType.Text;
                objsqlconn.Open();
                cmd.ExecuteNonQuery();
                objsqlconn.Close();
                sResult = "UPDATE";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
                //}
                //else
                //{
                //    sResult = "Billing Rate configuration already Exists";
                //}
                return sResult;
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw ex;
            }
        }

        #endregion
    }
}
