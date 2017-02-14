using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AE_SPICA.DAL
{
    public class clsClub
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

        public DataSet GetUserRole()
        {
            try
            {
                sFuncName = "GetUserRole()";

                sql = "SELECT '-- Select --' Code, '-- Select --' Name UNION ALL SELECT Cast(RoleId as VARCHAR(50)) [Code],RoleName [Name] FROM tbl_Roles";
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

        public string CreateClub(DataTable dt, string sCmpyCode)
        {
            try
            {
                sFuncName = "CreateClub()";
                DateTime dtDateTime = new DateTime();
                string sColumnNames = oGenSQL.BuildAllFieldsSQL(dt);
                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                sql = "SELECT * from tbl_Club where ClubCode = '" + dt.Rows[0]["ClubCode"].ToString() + "' and CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "'";
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);

                if (dsResult != null && dsResult.Tables[0].Rows.Count == 0)
                {
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);
                    sql = "SELECT * from tbl_Club where ClubName = '" + dt.Rows[0]["ClubName"].ToString().Replace("'", "''") + "' and CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "'";
                    DataSet ds1 = (DataSet)oDataAccess.ExecuteSqlString(sql);
                    if (ds1 != null && ds1.Tables[0].Rows.Count == 0)
                    {
                        string sParameter = "'" + dt.Rows[0]["CompanyCode"].ToString() + "','" + dt.Rows[0]["ClubCode"].ToString() + "'," +
                        "'" + dt.Rows[0]["ClubName"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["Address"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["ClubBP"].ToString() + "'";

                        string sInsertQuery = "insert into tbl_Club(" + sColumnNames + ") values(" + sParameter + ")";

                        SqlCommand cmd = new SqlCommand(sInsertQuery, objsqlconn);
                        cmd.CommandType = CommandType.Text;
                        objsqlconn.Open();
                        cmd.ExecuteNonQuery();
                        objsqlconn.Close();
                        sResult = "INSERT";
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Before Inserting the Records in Numbering Series Table", sFuncName);
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Date Time Month : " + DateTime.Now.Month, sFuncName);
                        if (DateTime.Now.Month <= 2)
                        {
                            string sDate = 02 + "/" + 19 + "/" + DateTime.Now.Year;
                            string[] format = { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "dd.MM.yyyy", "yyyyMMdd", "MMddYYYY", "M/dd/yyyy", "MM/dd/YYYY" };
                            System.DateTime.TryParseExact(sDate, format, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out dtDateTime);
                        }
                        else
                        {
                            string sDate = 02 + "/" + 19 + "/" + DateTime.Now.Date.AddYears(1).Year;
                            string[] format = {"dd/MM/yyyy","d/M/yyyy", "dd-MM-yyyy","dd.MM.yyyy","yyyyMMdd", "MMddYYYY","M/dd/yyyy", "MM/dd/YYYY" };
                            System.DateTime.TryParseExact(sDate, format, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out dtDateTime);
                        }
                        string sQuery = "insert into tbl_NumberingSeries (No,Year,CompanyCode,ClubCode,IsActive) values('001','" + dtDateTime.Date.ToString("yyyy-MM-dd") + "','" + sCmpyCode + "','" + dt.Rows[0]["ClubCode"].ToString() + "','1')";
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query:" + sQuery, sFuncName);

                        SqlCommand cmd1 = new SqlCommand(sQuery, objsqlconn);
                        cmd1.CommandType = CommandType.Text;
                        objsqlconn.Open();
                        cmd1.ExecuteNonQuery();
                        objsqlconn.Close();
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("After Inserting the Records in Numbering Series Table", sFuncName);
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
                    }
                    else
                    {
                        sResult = "Club Name already exists.";
                    }
                }
                else
                {
                    sResult = "Club Code already exists.";
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

        public DataSet SearchClub(string sClubName, string sCompany)
        {
            try
            {
                sFuncName = "SearchClub()";

                if (sClubName == string.Empty)
                {
                    sql = "SELECT * FROM tbl_Club where CompanyCode = '" + sCompany + "'";
                }
                else
                {
                    sql = "SELECT * FROM tbl_Club WHERE ClubName like '%" + sClubName.Replace("'", "''") + "%' and  CompanyCode = '" + sCompany + "'";
                }
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

        public string UpdateClub(DataTable dt)
        {
            try
            {
                sFuncName = "UpdateClub()";

                string sUpdateQuery = "Update tbl_Club SET ClubName = '" + dt.Rows[0]["ClubName"].ToString().Replace("'", "''") + "' ,Address = '" + dt.Rows[0]["Address"].ToString().Replace("'", "''") + "', ClubBP = '" + dt.Rows[0]["ClubBP"].ToString() + "'" +
                    " where ClubCode = '" + dt.Rows[0]["ClubCode"].ToString() + "' and CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "'";

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

        public DataSet GetBPCode(string sSAPDBName)
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

        public DataSet GetBPDetails(string sSAPDBName)
        {
            try
            {
                sFuncName = "GetBPDetails()";

                //sql = "SELECT rank() OVER (ORDER BY CardCode) Id,Country, City ,CardCode [Code],CardName [Name] from OCRD with (nolock) order by Code asc";
                sql = "SELECT rank() OVER (ORDER BY CardCode) Id,T1.Name Country, City ,CardCode [Code],CardName [Name] from OCRD T0 with (nolock) Left Join OCRY T1 ON T1.Code = T0.Country order by T0.CardCode asc";
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

        public DataSet GetBPName(string sSAPDBName, string sBPCode)
        {
            try
            {
                sFuncName = "GetBPName()";

                sql = "SELECT CardCode [Code],CardName [Name] from OCRD with (nolock) where CardCode = '" + sBPCode + "' order by Code asc ";
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

        #endregion
    }
}
