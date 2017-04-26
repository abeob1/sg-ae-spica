using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Configuration;

namespace AE_SPICA.DAL
{
    public class clsExpenseEntry
    {
        #region Objects

        public DataAccess oDataAccess = new DataAccess();
        public string sDateFormat = ConfigurationManager.AppSettings["DateFormat"].ToString();
        public string sSQLFormat = ConfigurationManager.AppSettings["SQLFormat"].ToString();
        public string sDefaultDate = ConfigurationManager.AppSettings["DefaultDate"].ToString();
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
        public string strSelect = "-- Select --";

        #endregion

        #region Methods

        public string CreateExpenseEntry(DataTable dt)
        {
            try
            {
                sFuncName = "CreateExpenseEntry()";

                dt.Columns.Remove("Id");
                string sColumnNames = oGenSQL.BuildAllFieldsSQL(dt);
                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                //string sFileReference = dt.Rows[0]["ClubID"].ToString().Substring(0, 3) + GetNumberingSeries() + dt.Rows[0]["year"].ToString() + dt.Rows[0]["ClaimHandler"].ToString();

                //sql = "SELECT * from tbl_FileReference where FileReferenceNo = '" + sFileReference + "' AND ClubReference = '" + dt.Rows[0]["ClubReference"].ToString() + "'";
                //dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
                //if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                //if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);

                //if (dsResult != null && dsResult.Tables[0].Rows.Count == 0)
                //{
                // dt.Rows[0]["FileReferenceNo"].ToString()
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Before converting date " + dt.Rows[0]["Date"].ToString(), sFuncName);
                //DateTime dt1 = Convert.ToDateTime(dt.Rows[0]["Date"].ToString());
                string sDate = Convert.ToDateTime(dt.Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("After converting date " + sDate, sFuncName);
                string sParameter = "'" + sDate + "'," +
                            "'" + dt.Rows[0]["FileReference"].ToString() + "','" + dt.Rows[0]["Expense"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["ChargableAmt"].ToString() + "'," +
                            "'" + dt.Rows[0]["Currency"].ToString() + "','" + dt.Rows[0]["Remarks"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["Attachment"].ToString() + "'," +
                            "'" + dt.Rows[0]["CompanyCode"].ToString() + "','" + dt.Rows[0]["UserCode"].ToString() + "'," +
                            "'" + dt.Rows[0]["ApprovalStatus"].ToString() + "'";

                string sInsertQuery = "insert into tbl_ExpenseEntry(" + sColumnNames + ") values(" + sParameter + ")";

                SqlCommand cmd = new SqlCommand(sInsertQuery, objsqlconn);
                cmd.CommandType = CommandType.Text;
                objsqlconn.Open();
                cmd.ExecuteNonQuery();
                objsqlconn.Close();
                sResult = "INSERT";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);

                //}
                //else
                //{
                //    sResult = "File Reference already Exists";
                //}
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

        public string UpdateExpenseEntry(DataTable dt)
        {
            try
            {
                sFuncName = "UpdateExpenseEntry()";


                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                string sDate = "";
                //DateTime dtime = new DateTime();
                if (dt.Rows[0]["Date"].ToString() != "")
                {
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Before converting date" + dt.Rows[0]["Date"].ToString(), sFuncName);
                    sDate = Convert.ToDateTime(dt.Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("After converting date" + sDate, sFuncName);
                    //dtime = Convert.ToDateTime(dt.Rows[0]["Date"].ToString());
                    //sDate = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                string sUpdateQuery = string.Empty;
                if (dt.Rows[0]["Attachment"].ToString() != string.Empty)
                {
                    sUpdateQuery = "update tbl_ExpenseEntry set Date = '" + sDate + "'," +
                                            "FileReference = '" + dt.Rows[0]["FileReference"].ToString() + "',Expense = '" + dt.Rows[0]["Expense"].ToString().Replace("'", "''") + "',ChargableAmt = '" + dt.Rows[0]["ChargableAmt"].ToString() + "'," +
                                            "Currency = '" + dt.Rows[0]["Currency"].ToString() + "',Remarks = '" + dt.Rows[0]["Remarks"].ToString().Replace("'", "''") + "',Attachment = '" + dt.Rows[0]["Attachment"].ToString() + "' " +
                                            "Where Id = '" + dt.Rows[0]["Id"].ToString() + "'";
                }
                else
                {
                    sUpdateQuery = "update tbl_ExpenseEntry set Date = '" + sDate + "'," +
                                                               "FileReference = '" + dt.Rows[0]["FileReference"].ToString() + "',Expense = '" + dt.Rows[0]["Expense"].ToString() + "',ChargableAmt = '" + dt.Rows[0]["ChargableAmt"].ToString() + "'," +
                                                               "Currency = '" + dt.Rows[0]["Currency"].ToString() + "',Remarks = '" + dt.Rows[0]["Remarks"].ToString().Replace("'", "''") + "' " +
                                                               " Where Id = '" + dt.Rows[0]["Id"].ToString() + "'";
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Update Query : " + sUpdateQuery, sFuncName);

                SqlCommand cmd = new SqlCommand(sUpdateQuery, objsqlconn);
                cmd.CommandType = CommandType.Text;
                objsqlconn.Open();
                cmd.ExecuteNonQuery();
                objsqlconn.Close();
                sResult = "UPDATE";
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

        public string DeleteExpenseEntry(string Id)
        {
            try
            {
                sFuncName = "DeleteExpenseEntry()";


                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                string sDeleteQuery = "Delete from tbl_ExpenseEntry where Id = '" + Id + "'";

                SqlCommand cmd = new SqlCommand(sDeleteQuery, objsqlconn);
                cmd.CommandType = CommandType.Text;
                objsqlconn.Open();
                cmd.ExecuteNonQuery();
                objsqlconn.Close();
                sResult = "DELETE";
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

        public DataSet SearchFileReference(string sVessel, string sClub, string sMember, string sFileReference, string sClaimHandler, string sYear, string sUserCode, string sCompanyCode)
        {
            try
            {
                sFuncName = "SearchFileReference()";

                sClub = (sClub == strSelect.ToString() ? "" : sClub);

                sql = "Declare @RoleId varchar(10); set @RoleId =(select top 1 RoleId from tbl_Users where companycode = '" + sCompanyCode + "' AND UserCode = '" + sUserCode + "' order by RoleId) " +
       "If(@RoleId = 4)" +
       "BEGIN " +
           "SELECT ID,FileReferenceNo,Vessel,(CASE WHEN CONVERT(DATE, IncidentDate) = '1900-01-01' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, 120) END)" +
                    "[IncidentDate],Status,Description FROM tbl_FileReference where (ISNULL('" + sVessel + "','') = '' OR Vessel like '%" + sVessel + "%')" +
                    "AND (ISNULL('" + sClub + "','') = '' OR ClubID like '%" + sClub + "%')" +
                    "AND (ISNULL('" + sMember + "','') = '' OR Member like '%" + sMember + "%')" +
                    "AND (ISNULL('" + sFileReference + "','') = '' OR FileReferenceNo like '%" + sFileReference + "%')" +
                    "AND (ISNULL('" + sClaimHandler + "','') = '' OR ClaimHandler like '%" + sClaimHandler + "%')" +
                    "AND (ISNULL('" + sYear + "','') = '' OR [year] like '%" + sYear + "%')" +
                    "AND FileStatus = 'OPEN' " +
                    "AND UserCode = '" + sUserCode + "' AND CompanyCode = '" + sCompanyCode + "'" +
                    "Order by ID " +
        "END " +
        "ELSE if(@RoleId = 3) " +
        "BEGIN " +
           "SELECT ID,FileReferenceNo,Vessel,(CASE WHEN CONVERT(DATE, IncidentDate) = '1900-01-01' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, 120) END)" +
                    "[IncidentDate],Status,Description FROM tbl_FileReference where (ISNULL('" + sVessel + "','') = '' OR Vessel like '%" + sVessel + "%')" +
                    "AND (ISNULL('" + sClub + "','') = '' OR ClubID like '%" + sClub + "%')" +
                    "AND (ISNULL('" + sMember + "','') = '' OR Member like '%" + sMember + "%')" +
                    "AND (ISNULL('" + sFileReference + "','') = '' OR FileReferenceNo like '%" + sFileReference + "%')" +
                    "AND (ISNULL('" + sClaimHandler + "','') = '' OR ClaimHandler like '%" + sClaimHandler + "%')" +
                    "AND (ISNULL('" + sYear + "','') = '' OR [year] like '%" + sYear + "%')" +
                    "AND FileStatus = 'OPEN' " +
                    "AND CompanyCode = '" + sCompanyCode + "'" +
                    "Order by ID " +
        "END " +
        "ELSE " +
        "BEGIN " +
           "SELECT ID,FileReferenceNo,Vessel,(CASE WHEN CONVERT(DATE, IncidentDate) = '1900-01-01' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, 120) END)" +
                    "[IncidentDate],Status,Description FROM tbl_FileReference where (ISNULL('" + sVessel + "','') = '' OR Vessel like '%" + sVessel + "%')" +
                    "AND (ISNULL('" + sClub + "','') = '' OR ClubID like '%" + sClub + "%')" +
                    "AND (ISNULL('" + sMember + "','') = '' OR Member like '%" + sMember + "%')" +
                    "AND (ISNULL('" + sFileReference + "','') = '' OR FileReferenceNo like '%" + sFileReference + "%')" +
                    "AND (ISNULL('" + sClaimHandler + "','') = '' OR ClaimHandler like '%" + sClaimHandler + "%')" +
                    "AND (ISNULL('" + sYear + "','') = '' OR [year] like '%" + sYear + "%')" +
                    "AND FileStatus = 'OPEN' " +
                    "Order by ID " +
        "END ";
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

        public DataSet GetClubDetails(string sCompanyCode)
        {
            try
            {
                sFuncName = "GetClubDetails()";

                sql = "SELECT '-- Select --' Code, '-- Select --' Name UNION ALL  SELECT ClubCode [Code],ClubName[Name] from tbl_Club where CompanyCode = '" + sCompanyCode + "' and IsNull(IsActive,'N') = 'Y'";
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

        public DataSet GetExpenseDetails()
        {
            try
            {
                sFuncName = "GetExpenseDetails()";

                sql = "SELECT '-- Select --' Code, '-- Select --' Name UNION ALL  SELECT Cast(TaskId as Varchar(50)) [Code],ItemCode [Name] from tbl_Task where IsNull(IsActive,'N') = 'Y' AND TYPE = 'Expense Entry' ORDER BY Code ASC";
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

        public DataSet GetRecords(string sUserCode, string sCompanyCode)
        {
            try
            {
                sFuncName = "GetRecords()";

                sql = "Declare @RoleId varchar(10); set @RoleId =(select top 1 RoleId from tbl_Users where companycode = '"+ sCompanyCode +"' AND UserCode = '"+ sUserCode + "' order by RoleId) " +
                       "If(@RoleId = 4)" +
                       "BEGIN " +
                            @"SELECT ID,Date,FileReference,Expense,ChargableAmt,Currency,Remarks,Case when Attachment != '' then (SELECT RIGHT(Attachment , CHARINDEX ('\' ,REVERSE(Attachment))-1)) else '' END AS Attachment " +
                            " ,CompanyCode,UserCode FROM tbl_ExpenseEntry WHERE CompanyCode = '" + sCompanyCode + "' AND UserCode = '" + sUserCode + "' AND " +
                            " FileReference in (select FileReferenceNo from tbl_FileReference where CompanyCode = '" + sCompanyCode + "' and " +
                            " UserCode = '" + sUserCode + "') AND IsNull(ApprovalStatus,'') != 'APPROVED' ORDER BY ID desc;" +
                        "END " +
                        "ELSE if(@RoleId = 3) " +
                        "BEGIN " +
                            @"SELECT ID,Date,FileReference,Expense,ChargableAmt,Currency,Remarks,Case when Attachment != '' then (SELECT RIGHT(Attachment , CHARINDEX ('\' ,REVERSE(Attachment))-1)) else '' END AS Attachment " +
                            " ,CompanyCode,UserCode FROM tbl_ExpenseEntry WHERE CompanyCode = '" + sCompanyCode + "' AND IsNull(ApprovalStatus,'') != 'APPROVED' AND " +
                            " FileReference in (select FileReferenceNo from tbl_FileReference where CompanyCode = '" + sCompanyCode + "') " +
                            " ORDER BY ID desc;" +
                        "END " +
                        "ELSE " +
                        "BEGIN " +
                            @"SELECT ID,Date,FileReference,Expense,ChargableAmt,Currency,Remarks,Case when Attachment != '' then (SELECT RIGHT(Attachment , CHARINDEX ('\' ,REVERSE(Attachment))-1)) else '' END AS Attachment " +
                            " ,CompanyCode,UserCode FROM tbl_ExpenseEntry WHERE " +
                            " FileReference in (select FileReferenceNo from tbl_FileReference)  AND IsNull(ApprovalStatus,'') != 'APPROVED' ORDER BY ID desc;" +
                        "END ";

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

        #endregion

    }
}
