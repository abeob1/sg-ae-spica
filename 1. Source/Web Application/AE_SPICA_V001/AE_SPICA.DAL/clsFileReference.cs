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
    public class clsFileReference
    {

        #region Objects
        public string sDateFormat = ConfigurationManager.AppSettings["DateFormat"].ToString();
        public string sSQLFormat = ConfigurationManager.AppSettings["SQLFormat"].ToString();
        public string sDefaultDate = ConfigurationManager.AppSettings["DefaultDate"].ToString();
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
        public string strSelect = "-- Select --";

        #endregion

        #region Methods

        public string CreateFileReference(DataTable dt)
        {
            string sFileReference = string.Empty;
            try
            {
                sFuncName = "CreateFileReference()";

                dt.Columns.Remove("Id");
                string sColumnNames = oGenSQL.BuildAllFieldsSQL(dt);
                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                string sNumberingSeries = GetNumberingSeries(dt.Rows[0]["CompanyCode"].ToString(), dt.Rows[0]["ClubID"].ToString());
                if (sNumberingSeries != string.Empty)
                {
                    sFileReference = dt.Rows[0]["ClubID"] + "/" + sNumberingSeries + "/" + dt.Rows[0]["year"].ToString() + "/" + dt.Rows[0]["ClaimHandler"].ToString();

                    sql = "SELECT * from tbl_FileReference where FileReferenceNo = '" + sFileReference + "' AND ClubReference = '" + dt.Rows[0]["ClubReference"].ToString() + "'";
                    dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);

                    if (dsResult != null && dsResult.Tables[0].Rows.Count == 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Before converting date " + dt.Rows[0]["IncidentDate"].ToString(), sFuncName);
                        string sDate = string.Empty;
                        if (dt.Rows[0]["IncidentDate"].ToString() != string.Empty)
                        {
                            sDate = Convert.ToDateTime(dt.Rows[0]["IncidentDate"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                        string sDateFrom = string.Empty;
                        string sDateTo = string.Empty;
                        if (dt.Rows[0]["PeriodDateFrom"].ToString() != string.Empty)
                        {
                            sDateFrom = Convert.ToDateTime(dt.Rows[0]["PeriodDateFrom"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (dt.Rows[0]["PeriodDateTo"].ToString() != string.Empty)
                        {
                            sDateTo = Convert.ToDateTime(dt.Rows[0]["PeriodDateTo"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                        string sCloseOnDate = string.Empty;
                        if (dt.Rows[0]["CloseOn"].ToString() != string.Empty)
                        {
                            sCloseOnDate = Convert.ToDateTime(dt.Rows[0]["CloseOn"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("After converting date " + sDate, sFuncName);
                        // dt.Rows[0]["FileReferenceNo"].ToString(),'" + dt.Rows[0]["BillingAddress"].ToString() + "','" + dt.Rows[0]["Address"].ToString() + "','" + dt.Rows[0]["BillTo"].ToString() + "'
                        string sParameter = "'" + sDate + "'," +
                                "'" + dt.Rows[0]["Vessel"].ToString() + "','" + dt.Rows[0]["ClubID"].ToString() + "','" + dt.Rows[0]["ClubBP"].ToString() + "','" + dt.Rows[0]["ClubBPContact"].ToString() + "','" + dt.Rows[0]["Member"].ToString() + "'," +
                                "'" + dt.Rows[0]["MemberAdd1"].ToString() + "','" + dt.Rows[0]["MemberAdd2"].ToString() + "','" + dt.Rows[0]["MemberAdd3"].ToString() + "'," +
                                "'" + dt.Rows[0]["MemberAdd4"].ToString() + "','" +
                                sFileReference + "'," + "'" + dt.Rows[0]["ClaimHandler"].ToString() + "','" + dt.Rows[0]["year"].ToString() + "'," +
                                "'" + dt.Rows[0]["CompanyCode"].ToString() + "'," +
                                "'" + dt.Rows[0]["ClubReference"].ToString() + "','" + dt.Rows[0]["CO"].ToString().Replace("'", "''") + "'," +
                                "'" + dt.Rows[0]["ContactName"].ToString() + "','" + dt.Rows[0]["Email"].ToString() + "','" + dt.Rows[0]["VATNumber"].ToString() + "'," +
                                "'" + dt.Rows[0]["Description"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["IncidentPlace"].ToString() + "','" + dt.Rows[0]["FileStatus"].ToString() + "'," +
                                "'" + dt.Rows[0]["UserCode"].ToString() + "','" + sDateFrom + "','" + sDateTo + "','" + dt.Rows[0]["VoyageNumber"].ToString() + "'," +
                                "'" + sCloseOnDate + "','" + dt.Rows[0]["LocationStore"].ToString() + "'";
                        //'" + dt.Rows[0]["Status"].ToString() + "',
                        string sInsertQuery = "insert into tbl_FileReference(" + sColumnNames + ") values(" + sParameter + ")";

                        SqlCommand cmd = new SqlCommand(sInsertQuery, objsqlconn);
                        cmd.CommandType = CommandType.Text;
                        objsqlconn.Open();
                        cmd.ExecuteNonQuery();
                        objsqlconn.Close();
                        sResult = "INSERT | " + sFileReference;
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);

                        sql = "declare @id varchar(3);Set @id= (select Top 1 [No] from tbl_numberingSeries where [YEAR] >= '" + DateTime.Now.Date.ToString("yyyyMMdd") +
                               "' AND CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "' AND ClubCode = '" + dt.Rows[0]["ClubID"].ToString() + "') + 1 " +
                               "if (@id <= 009) SET @id = ('00' + @id);else if (@id > 009 and @id <= 099) SET @id = ('0' + @id); else if(@id > 099) SET @id = @id;" +
                               "Update tbl_numberingSeries set [No] = @id " +
                               " WHERE [YEAR] = (select Top 1 [YEAR] from tbl_numberingSeries where [YEAR] >= '" + DateTime.Now.Date.ToString("yyyyMMdd") + "' " +
                               " AND CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "' AND ClubCode = '" + dt.Rows[0]["ClubID"].ToString() + "') AND CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "' AND ClubCode = '" + dt.Rows[0]["ClubID"].ToString() + "'";

                        //sql = "declare @id varchar(8);Set @id= (SELECT REPLICATE('0', (8-LEN(ISNULL(MAX(SUBSTRING(No ,3,LEN(No )))+1,1)))) " +
                        //      "+ CONVERT(VARCHAR,ISNULL(MAX(SUBSTRING(No,3,LEN(No)))+1,1)) from tbl_NumberingSeries where Year([YEAR]) = '" + DateTime.Now.Year + "')" +
                        //        "Update tbl_numberingSeries set [No] = @id " +
                        //        " WHERE [YEAR] = (select Top 1 [YEAR] from tbl_numberingSeries where [YEAR] >= '" + DateTime.Now.Date.ToString("yyyyMMdd") + "')";
                        SqlCommand cmd1 = new SqlCommand(sql, objsqlconn);
                        cmd1.CommandType = CommandType.Text;
                        objsqlconn.Open();
                        cmd1.ExecuteNonQuery();
                        objsqlconn.Close();
                    }
                    else
                    {
                        sResult = "File Reference already Exists";
                    }
                }
                else
                {
                    sResult = "Kindly Define the Numbering Series to create File Reference";
                }
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

        public string UpdateFileReference(DataTable dt)
        {
            try
            {
                sFuncName = "UpdateFileReference()";


                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                string sDate = "";
                string sFromDate = "";
                string sToDate = "";
                string sCloseOnDate = "";
                DateTime dtime = new DateTime();
                if (dt.Rows[0]["IncidentDate"].ToString() != "")
                {
                    dtime = Convert.ToDateTime(dt.Rows[0]["IncidentDate"].ToString());
                    sDate = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (dt.Rows[0]["PeriodDateFrom"].ToString() != "")
                {
                    dtime = Convert.ToDateTime(dt.Rows[0]["PeriodDateFrom"].ToString());
                    sFromDate = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (dt.Rows[0]["PeriodDateTo"].ToString() != "")
                {
                    dtime = Convert.ToDateTime(dt.Rows[0]["PeriodDateTo"].ToString());
                    sToDate = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (dt.Rows[0]["CloseOn"].ToString() != "")
                {
                    dtime = Convert.ToDateTime(dt.Rows[0]["CloseOn"].ToString());
                    sCloseOnDate = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                }

                //"BillingAddress = '" + dt.Rows[0]["BillingAddress"].ToString() + "'," + ,Address = '" + dt.Rows[0]["Address"].ToString() + "',BillTo = '" + dt.Rows[0]["BillTo"].ToString() + "'

                string sUpdateQuery = "update tbl_FileReference set IncidentDate = '" + sDate + "'," +
                                        "Vessel = '" + dt.Rows[0]["Vessel"].ToString() + "',ClubID = '" + dt.Rows[0]["ClubID"].ToString() + "'," +
                                        " ClubBP = '" + dt.Rows[0]["ClubBP"].ToString() + "',ClubBPContact = '" + dt.Rows[0]["ClubBPContact"].ToString() + "'," +
                                        " Member = '" + dt.Rows[0]["Member"].ToString() + "'," +
                                        "MemberAdd1 = '" + dt.Rows[0]["MemberAdd1"].ToString() + "',MemberAdd2 = '" + dt.Rows[0]["MemberAdd2"].ToString() + "',MemberAdd3 = '" + dt.Rows[0]["MemberAdd3"].ToString() + "'," +
                                        "MemberAdd4 = '" + dt.Rows[0]["MemberAdd4"].ToString() + "'," +
                                        "ClaimHandler = '" + dt.Rows[0]["ClaimHandler"].ToString() + "',year = '" + dt.Rows[0]["year"].ToString() + "',ClubReference = '" + dt.Rows[0]["ClubReference"].ToString() + "'," +
                                        "CO = '" + dt.Rows[0]["CO"].ToString().Replace("'", "''") + "',ContactName = '" + dt.Rows[0]["ContactName"].ToString() + "'," +
                                        "Email = '" + dt.Rows[0]["Email"].ToString() + "',VATNumber = '" + dt.Rows[0]["VATNumber"].ToString() + "',Description = '" + dt.Rows[0]["Description"].ToString().Replace("'", "''") + "'," +
                                        "IncidentPlace = '" + dt.Rows[0]["IncidentPlace"].ToString() + "',FileStatus = '" + dt.Rows[0]["FileStatus"].ToString() + "', PeriodDateFrom = '" + sFromDate + "', PeriodDateTo = '" + sToDate + "', VoyageNumber = '" + dt.Rows[0]["VoyageNumber"].ToString() + "', CloseOn = '" + sCloseOnDate + "', LocationStore = '" + dt.Rows[0]["LocationStore"].ToString() + "' " +
                                        "Where Id = '" + dt.Rows[0]["Id"].ToString() + "'";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Update Query : " + sUpdateQuery, sFuncName);

                SqlCommand cmd = new SqlCommand(sUpdateQuery, objsqlconn);
                cmd.CommandType = CommandType.Text;
                objsqlconn.Open();
                cmd.ExecuteNonQuery();
                objsqlconn.Close();
                sResult = "UPDATE | " + dt.Rows[0]["FileReferenceNo"].ToString();
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

        public DataSet SearchFileReference(string sVessel, string sClub, string sMember, string sFileReference, string sClaimHandler, string sYear, string sFileStatus, string sUserCode, string sCompanyCode)
        {
            try
            {
                sFuncName = "SearchFileReference()";

                sClub = (sClub == strSelect.ToString() ? "" : sClub);
                sFileStatus = (sFileStatus == strSelect.ToString() ? "" : sFileStatus);

                sql = "Declare @RoleId varchar(10); set @RoleId =(select top 1 RoleId from tbl_Users where companycode = '" + sCompanyCode + "' AND UserCode = '" + sUserCode + "' order by RoleId) " +
       "If(@RoleId = 4)" +
       "BEGIN " +
           "SELECT ID,FileReferenceNo,Vessel,(CASE WHEN CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") END)" +
                    "[IncidentDate],Status FROM tbl_FileReference where (ISNULL('" + sVessel + "','') = '' OR Vessel like '%" + sVessel + "%')" +
                    "AND (ISNULL('" + sClub + "','') = '' OR ClubID like '%" + sClub + "%')" +
                    "AND (ISNULL('" + sMember + "','') = '' OR Member like '%'" + sMember + "'%')" +
                    "AND (ISNULL('" + sFileReference + "','') = '' OR FileReferenceNo like '%" + sFileReference + "%')" +
                    "AND (ISNULL('" + sClaimHandler + "','') = '' OR ClaimHandler like '%" + sClaimHandler + "%')" +
                    "AND (ISNULL('" + sYear + "','') = '' OR [year] like '%" + sYear + "%')" +
                    "AND (ISNULL('" + sFileStatus + "','') = '' OR [FileStatus] like '%" + sFileStatus + "%')" +
                    "AND UserCode = '" + sUserCode + "' AND CompanyCode = '" + sCompanyCode + "'" +
                    " Order by ID " +
        "END " +
        "ELSE if(@RoleId = 3) " +
        "BEGIN " +
           "SELECT ID,FileReferenceNo,Vessel,(CASE WHEN CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") END)" +
                    "[IncidentDate],Status FROM tbl_FileReference where (ISNULL('" + sVessel + "','') = '' OR Vessel like '%" + sVessel + "%')" +
                    "AND (ISNULL('" + sClub + "','') = '' OR ClubID like '%" + sClub + "%')" +
                    "AND (ISNULL('" + sMember + "','') = '' OR Member like '%'" + sMember + "'%')" +
                    "AND (ISNULL('" + sFileReference + "','') = '' OR FileReferenceNo like '%" + sFileReference + "%')" +
                    "AND (ISNULL('" + sClaimHandler + "','') = '' OR ClaimHandler like '%" + sClaimHandler + "%')" +
                    "AND (ISNULL('" + sYear + "','') = '' OR [year] like '%" + sYear + "%')" +
                    "AND (ISNULL('" + sFileStatus + "','') = '' OR [FileStatus] like '%" + sFileStatus + "%')" +
                    " AND CompanyCode = '" + sCompanyCode + "'" +
                    " Order by ID " +
        "END " +
        "ELSE " +
        "BEGIN " +
           "SELECT ID,FileReferenceNo,Vessel,(CASE WHEN CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") END)" +
                    "[IncidentDate],Status FROM tbl_FileReference where (ISNULL('" + sVessel + "','') = '' OR Vessel like '%" + sVessel + "%')" +
                    "AND (ISNULL('" + sClub + "','') = '' OR ClubID like '%" + sClub + "%')" +
                    "AND (ISNULL('" + sMember + "','') = '' OR Member like '%'" + sMember + "'%')" +
                    "AND (ISNULL('" + sFileReference + "','') = '' OR FileReferenceNo like '%" + sFileReference + "%')" +
                    "AND (ISNULL('" + sClaimHandler + "','') = '' OR ClaimHandler like '%" + sClaimHandler + "%')" +
                    "AND (ISNULL('" + sYear + "','') = '' OR [year] like '%" + sYear + "%')" +
                    "AND (ISNULL('" + sFileStatus + "','') = '' OR [FileStatus] like '%" + sFileStatus + "%')" +
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

        public string GetNumberingSeries(string sCompanyCode, string sClubCode)
        {
            try
            {
                string sNo = string.Empty;
                sFuncName = "GetNumberingSeries()";

                sql = "select Top 1 [No],[YEAR] from tbl_numberingSeries where [YEAR] >= '" + DateTime.Now.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "' AND IsActive = 1 AND CompanyCode = '" + sCompanyCode + "' AND ClubCode = '" + sClubCode + "'";

                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);

                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        sNo = dsResult.Tables[0].Rows[0]["No"].ToString();
                    }
                }
                return sNo;
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
                throw ex;
            }
        }

        public DataSet GetRecords(string id)
        {
            try
            {
                sFuncName = "GetRecords()";

                sql = "SELECT *,(CASE WHEN CONVERT(DATE, PeriodDateFrom) = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), PeriodDateFrom, " + sSQLFormat + ") END)[ConvPeriodDateFrom], " +
                         " (CASE WHEN CONVERT(DATE, PeriodDateTo) = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), PeriodDateTo, " + sSQLFormat + ") END)[ConvPeriodDateTo], " +
                    "(CASE WHEN CONVERT(DATE, IncidentDate) = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), IncidentDate, " + sSQLFormat + ") END)[ConvIncidentDate], " +
                    "(CASE WHEN CONVERT(DATE, CloseOn) = '" + sDefaultDate + "' THEN '' ELSE CONVERT(CHAR(10), CloseOn, " + sSQLFormat + ") END)[ConvCloseOn] from tbl_FileReference where Id = '" + id + "'";
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

        public DataSet GetInvBalanceandPaymentDate(string sCompanyCode, string sFileRefNo)
        {

            try
            {
                sFuncName = "GetInvBalanceandPaymentDate()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP013_GetInvBalanceandPaymentDate] '" + sCompanyCode + "','" + sFileRefNo + "'";

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

        public DataSet GetTimeEntryRecords(string sCompanyCode, string sUserCode, string sFileRefNo)
        {

            try
            {
                sFuncName = "GetTimeEntryRecords()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP002_GetTimeEntryRecords] '" + sCompanyCode + "','" + sUserCode + "','" + sFileRefNo + "'";

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

        public DataSet GetExpenseEntryRecords(string sCompanyCode, string sUserCode, string sFileRefNo)
        {

            try
            {
                sFuncName = "GetExpenseEntryRecords()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP003_GetExpenseEntryRecords] '" + sCompanyCode + "','" + sUserCode + "','" + sFileRefNo + "'";

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

        public string SendToApproval(string sUserCode, string sCompanyCode, string sTypeofBill, string sFileRefNo, string sBillingDetails, string sLumpSum)
        {
            try
            {
                sFuncName = "SendToApproval()";
                string sResult = string.Empty;
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP004_SendToApproval] '" + sCompanyCode + "','" + sUserCode + "','" + sTypeofBill + "','" + sFileRefNo + "','" + sBillingDetails.ToString().Replace("'", "''") + "', '" + sLumpSum + "'";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling Run_StoredProcedure()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Procedure : " + sProcedureName, sFuncName);
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sProcedureName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    sResult = dsResult.Tables[0].Rows[0][0].ToString();
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

        public DataSet GetBPCode(string sSAPDBName, string sClubCode)
        {
            try
            {
                sFuncName = "GetBPCode()";

                sql = "select '-- Select --' Code, '-- Select --' Name UNION ALL SELECT CardCode [Code],CardCode [Name] from OCRD order by Code asc";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);
                dsResult = (DataSet)oDataAccess.Run_QueryString(sql, sSAPDBName);
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

        public string GetDefaultClubBP(string sClubCode, string sCompanyCode)
        {
            string sClubBP = strSelect.ToString();
            try
            {

                sFuncName = "GetDefaultClubBP()";

                sql = "select Top 1 ClubBP from tbl_Club where ClubCode = '" + sClubCode + "' and  CompanyCode = '" + sCompanyCode + "'";

                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);

                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        sClubBP = dsResult.Tables[0].Rows[0]["ClubBP"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed With ERROR  ", sFuncName);
            }
            return sClubBP;
        }

        public DataSet GetBPContact(string sSAPDBName, string sClubCode)
        {
            try
            {
                sFuncName = "GetBPContact()";

                sql = "select '-- Select --' BPContact, '-- Select --' DefaultBPContact UNION ALL SELECT T1.Name [BPContact],T0.CntctPrsn [DefaultBPContact] from OCRD T0 with (nolock) " +
                    " LEFT JOIN OCPR T1 with (nolock) on T0.CardCode = T1.CardCode where T0.CardCode  = '" + sClubCode + "' order by BPContact asc";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);
                dsResult = (DataSet)oDataAccess.Run_QueryString(sql, sSAPDBName);
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

        public DataSet ExtractHoldingInv(string sCompanyCode, string sFileRefNo, string sSQLFormat)
        {

            try
            {
                sFuncName = "ExtractHoldingInv()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP014_ExtractHoldingInv] '" + sCompanyCode + "','" + sFileRefNo + "','" + sSQLFormat + "'";

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
