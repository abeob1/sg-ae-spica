using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace AE_SPICA.DAL
{
    public class clsLogin
    {
        #region Objects

        public DataAccess oDataAccess = new DataAccess();
        public string sql = string.Empty;
        clsLog oLog = new clsLog();
        DataSet dsResult = new DataSet();
        string sFuncName = string.Empty;
        public Int16 p_iDebugMode = DEBUG_ON;
        public string sResult = string.Empty;

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

        public DataSet LoginValidation(string sUserCode, string sPassword, string sCompany)
        {

            try
            {
                sFuncName = "LoginValidation()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP001_LoginCheck] '" + sUserCode + "','" + sPassword + "','" + sCompany + "'";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling Run_StoredProcedure()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Procedure : " + sProcedureName, sFuncName);
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sProcedureName);
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

        public string ChangePassword(string sNewPassword, string sUserCode, string sCompany)
        {

            try
            {
                sFuncName = "ChangePassword()";
                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                string sUpdateQuery = "update tbl_Users set Password = '" + sNewPassword + "' Where UserCode = '" + sUserCode + "' AND CompanyCode = '" + sCompany + "'";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Update Query : " + sUpdateQuery, sFuncName);

                SqlCommand cmd = new SqlCommand(sUpdateQuery, objsqlconn);
                cmd.CommandType = CommandType.Text;
                objsqlconn.Open();
                cmd.ExecuteNonQuery();
                objsqlconn.Close();
                sResult = "SUCCESS";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw ex;
            }
            return sResult;
        }

        public string GetPassword(string sUserCode, string sCompany)
        {
            string sResult = string.Empty;
            try
            {
                sFuncName = "GetPassword()";

                sql = "select [Password] from tbl_Users where UserCode = '" + sUserCode + "' and CompanyCode = '" + sCompany + "' AND ISNULL(IsActive,'Y') = 'Y'";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);

                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        sResult = dsResult.Tables[0].Rows[0][0].ToString();
                    }
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
            return sResult;
        }

        public DataSet GetCompanyBasedOnUserRole(string sRoleName, string sCompany)
        {

            try
            {
                sFuncName = "GetCompanyBasedOnUserRole()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP012_GetCompanyBasedOnUserRole] '" + sRoleName + "','" + sCompany + "'";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling Run_StoredProcedure()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Procedure : " + sProcedureName, sFuncName);
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sProcedureName);
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

        #endregion
    }
}
