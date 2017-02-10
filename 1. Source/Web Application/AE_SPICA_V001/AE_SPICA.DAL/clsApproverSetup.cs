using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AE_SPICA.DAL
{
    public class clsApproverSetup
    {
        #region Objects

        public DataAccess oDataAccess = new DataAccess();
        public string sql = string.Empty;
        clsLog oLog = new clsLog();
        public string sResult = string.Empty;
        string sFuncName = string.Empty;
        DataSet dsResult = new DataSet();
        GenerateSQL oGenSQL = new GenerateSQL();

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

        public string CreateApprover(DataTable dt)
        {
            try
            {
                sFuncName = "CreateApprover()";

                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                sql = "SELECT * from tbl_ApprovalSetup where CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "' and Approver = '" + dt.Rows[0]["UserCode"].ToString() + "'";
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);

                if (dsResult != null && dsResult.Tables[0].Rows.Count == 0)
                {

                    string sParameter = "'" + dt.Rows[0]["CompanyCode"].ToString() + "'," +
                    "'" + dt.Rows[0]["UserCode"].ToString() + "'";

                    string sInsertQuery = "insert into tbl_ApprovalSetup(CompanyCode,Approver) values(" + sParameter + ")";

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
                    sResult = "Approval setup already Exists";
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

        public DataSet SearchApprover(string sCompanyCode)
        {
            try
            {
                sFuncName = "SearchApprover()";

                sql = "SELECT ID,CompanyCode,Approver [UserCode],( Left(convert(varchar(7),LastApprovedMonth, 107) ,3) + ' ' + convert(varchar(4),YEAR(LastApprovedMonth))) " +
                "[ApprovedMonth],(select Name from tbl_CompanyData where Code = a.CompanyCode)" +
                "[CompanyName],(select UserName from tbl_Users where UserCode = a.Approver and CompanyCode = a.CompanyCode) [UserName] FROM tbl_approvalSetup a " +
                " WHERE CompanyCode in (select Code from tbl_CompanyData where Name like '%" + sCompanyCode + "%') Order by ID";
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

        public string UpdateApprover(DataTable dt)
        {
            try
            {
                sFuncName = "UpdateApprover()";

                sql = "SELECT * from tbl_ApprovalSetup where CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "' and Approver = '" + dt.Rows[0]["UserCode"].ToString() + "'";
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);

                if (dsResult != null && dsResult.Tables[0].Rows.Count == 0)
                {
                    string sUpdateQuery = "Update tbl_approvalSetup SET CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "' ,Approver = '" + dt.Rows[0]["UserCode"].ToString() + "'" +
                        " where Id = '" + dt.Rows[0]["Id"].ToString() + "'";

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
                }
                else
                {
                    sResult = "Approval setup already Exists";
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

        #endregion
    }
}
