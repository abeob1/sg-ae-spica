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
    public class clsTimeEntry
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

        public string CreateTimeEntry(DataTable dt)
        {
            try
            {
                sFuncName = "CreateTimeEntry()";

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
                string sDate = Convert.ToDateTime(dt.Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                string billAmt = dt.Rows[0]["BillAmount"].ToString() == "" ? "0" : dt.Rows[0]["BillAmount"].ToString();
                string sParameter = "'" + sDate + "'," +
                            "'" + dt.Rows[0]["FileReference"].ToString() + "','" + dt.Rows[0]["Task"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["Description"].ToString().Replace("'", "''") + "'," +
                            "'" + dt.Rows[0]["Duration"].ToString() + "','" + dt.Rows[0]["BillableUnit"].ToString() + "','" + billAmt + "'," +
                            "'" + dt.Rows[0]["CompanyCode"].ToString() + "','" + dt.Rows[0]["UserCode"].ToString() + "','" + dt.Rows[0]["PrivateRemarks"].ToString() + "',"+
                            "'" + dt.Rows[0]["ApprovalStatus"].ToString() + "'";

                string sInsertQuery = "insert into tbl_TimeEntry(" + sColumnNames + ") values(" + sParameter + ")";

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

        public string UpdateTimeEntry(DataTable dt)
        {
            try
            {
                sFuncName = "UpdateTimeEntry()";


                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                string sDate = "";
                DateTime dtime = new DateTime();
                if (dt.Rows[0]["Date"].ToString() != "")
                {
                    dtime = Convert.ToDateTime(dt.Rows[0]["Date"].ToString());
                    sDate = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                }

                string sUpdateQuery = "update tbl_TimeEntry set Date = '" + sDate + "'," +
                                        "FileReference = '" + dt.Rows[0]["FileReference"].ToString() + "',Task = '" + dt.Rows[0]["Task"].ToString().Replace("'", "''") + "',Description = '" + dt.Rows[0]["Description"].ToString().Replace("'", "''") + "'," +
                                        "Duration = '" + dt.Rows[0]["Duration"].ToString() + "',BillableUnit = '" + dt.Rows[0]["BillableUnit"].ToString() + "',BillAmount = '" + dt.Rows[0]["BillAmount"].ToString() + "'," +
                                        "PrivateRemarks = '" + dt.Rows[0]["PrivateRemarks"].ToString() + "' Where Id = '" + dt.Rows[0]["Id"].ToString() + "'";

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

        public string DeleteTimeEntry(string Id)
        {
            try
            {
                sFuncName = "DeleteTimeEntry()";


                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                string sDeleteQuery = "Delete from tbl_TimeEntry where Id = '" + Id + "'";

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
           "SELECT ID,FileReferenceNo,Vessel,(CASE WHEN CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") END)" +
                    "[IncidentDate],Status,Description FROM tbl_FileReference where (ISNULL('" + sVessel + "','') = '' OR Vessel like '%" + sVessel + "%')" +
                    "AND (ISNULL('" + sClub + "','') = '' OR ClubID like '%" + sClub + "%')" +
                    "AND (ISNULL('" + sMember + "','') = '' OR Member like '%'" + sMember + "'%')" +
                    "AND (ISNULL('" + sFileReference + "','') = '' OR FileReferenceNo like '%" + sFileReference + "%')" +
                    "AND (ISNULL('" + sClaimHandler + "','') = '' OR ClaimHandler like '%" + sClaimHandler + "%')" +
                    "AND (ISNULL('" + sYear + "','') = '' OR [year] like '%" + sYear + "%')" +
                    "AND FileStatus = 'OPEN' " +
                    "AND UserCode = '" + sUserCode + "' AND CompanyCode = '" + sCompanyCode + "'" +
                    " Order by ID " +
        "END " +
        "ELSE if(@RoleId = 3) " +
        "BEGIN " +
           "SELECT ID,FileReferenceNo,Vessel,(CASE WHEN CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") END)" +
                    "[IncidentDate],Status,Description FROM tbl_FileReference where (ISNULL('" + sVessel + "','') = '' OR Vessel like '%" + sVessel + "%')" +
                    "AND (ISNULL('" + sClub + "','') = '' OR ClubID like '%" + sClub + "%')" +
                    "AND (ISNULL('" + sMember + "','') = '' OR Member like '%'" + sMember + "'%')" +
                    "AND (ISNULL('" + sFileReference + "','') = '' OR FileReferenceNo like '%" + sFileReference + "%')" +
                    "AND (ISNULL('" + sClaimHandler + "','') = '' OR ClaimHandler like '%" + sClaimHandler + "%')" +
                    "AND (ISNULL('" + sYear + "','') = '' OR [year] like '%" + sYear + "%')" +
                    "AND FileStatus = 'OPEN' " +
                    "AND CompanyCode = '" + sCompanyCode + "'" +
                    " Order by ID " +
        "END " +
        "ELSE " +
        "BEGIN " +
           "SELECT ID,FileReferenceNo,Vessel,(CASE WHEN CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") END)" +
                    "[IncidentDate],Status,Description FROM tbl_FileReference where (ISNULL('" + sVessel + "','') = '' OR Vessel like '%" + sVessel + "%')" +
                    "AND (ISNULL('" + sClub + "','') = '' OR ClubID like '%" + sClub + "%')" +
                    "AND (ISNULL('" + sMember + "','') = '' OR Member like '%'" + sMember + "'%')" +
                    "AND (ISNULL('" + sFileReference + "','') = '' OR FileReferenceNo like '%" + sFileReference + "%')" +
                    "AND (ISNULL('" + sClaimHandler + "','') = '' OR ClaimHandler like '%" + sClaimHandler + "%')" +
                    "AND (ISNULL('" + sYear + "','') = '' OR [year] like '%" + sYear + "%')" +
                    "AND FileStatus = 'OPEN' " +
                    " Order by ID " +
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

        public DataSet GetTaskDetails()
        {
            try
            {
                sFuncName = "GetTaskDetails()";

                sql = "SELECT '-- Select --' Code, '-- Select --' Name UNION ALL  SELECT ItemCode [Code],ItemName [Name] from tbl_Task where IsNull(IsActive,'N') = 'Y' AND TYPE = 'Time Entry'";
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
                //ApprovalStatus != 'APPROVED' and 
                sql = "Declare @RoleId varchar(10); set @RoleId =(select top 1 RoleId from tbl_Users where companycode = '" + sCompanyCode + "' AND UserCode = '" + sUserCode + "' order by RoleId) " +
                       "If(@RoleId = 4)" +
                       "BEGIN " +
                            "SELECT * FROM tbl_TimeEntry WHERE CompanyCode = '" + sCompanyCode + "' AND UserCode = '" + sUserCode + "' AND IsNull(ApprovalStatus,'') != 'APPROVED' AND " +
                            " FileReference in (select FileReferenceNo from tbl_FileReference where CompanyCode = '" + sCompanyCode + "' and " +
                            " UserCode = '" + sUserCode + "') ORDER BY ID desc " +
                        "END " +
                        "ELSE if(@RoleId = 3) " +
                        "BEGIN " +
                            "SELECT * FROM tbl_TimeEntry WHERE CompanyCode = '" + sCompanyCode + "' AND IsNull(ApprovalStatus,'') != 'APPROVED' AND" +
                            " FileReference in (select FileReferenceNo from tbl_FileReference where CompanyCode = '" + sCompanyCode + "') ORDER BY ID desc " +
                        "END " +
                        "ELSE " +
                        "BEGIN " +
                            "SELECT * FROM tbl_TimeEntry  WHERE IsNull(ApprovalStatus,'') != 'APPROVED' AND" +
                            " FileReference in (select FileReferenceNo from tbl_FileReference) ORDER BY ID desc " +
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

        public DataSet GetBillingConfigurationDetails(string sUserCode, string sCompanyCode)
        {
            try
            {
                sFuncName = "GetBillingConfigurationDetails()";

                sql = "SELECT [Time],BillableUnit,HourlyRate FROM tbl_Billing WHERE CompanyCode = '" + sCompanyCode + "' AND UserCode = '" + sUserCode + "'";
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
