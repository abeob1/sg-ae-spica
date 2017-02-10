using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AE_SPICA.DAL
{
    public class clsUser
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
                sFuncName = "GetCompanyDetails()";

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

        public string CreateUser(DataTable dt)
        {
            try
            {
                sFuncName = "CreateUser()";

                string sColumnNames = oGenSQL.BuildAllFieldsSQL(dt);
                SqlConnection objsqlconn = new SqlConnection(oDataAccess.connection);

                sql = "SELECT * from tbl_Users where USERCODE = '" + dt.Rows[0]["UserCode"].ToString() + "' AND CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "'";
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sql);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);

                if (dsResult != null && dsResult.Tables[0].Rows.Count == 0)
                {
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling ExecuteSqlString()", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Query : " + sql, sFuncName);
                    //sql = "SELECT * from tbl_Users where USERNAME = '" + dt.Rows[0]["UserName"].ToString() + "' AND CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "'";
                    //DataSet ds1 = (DataSet)oDataAccess.ExecuteSqlString(sql);
                    //if (ds1 != null && ds1.Tables[0].Rows.Count == 0)
                    //{
                    string sParameter = "'" + dt.Rows[0]["UserCode"].ToString() + "'," +
                    "'" + dt.Rows[0]["UserName"].ToString() + "','" + dt.Rows[0]["Password"].ToString() + "','" + dt.Rows[0]["RoleId"].ToString() + "'," +
                    "'" + dt.Rows[0]["Email"].ToString() + "','" + dt.Rows[0]["CompanyCode"].ToString() + "','" + dt.Rows[0]["IsActive"].ToString() + "'";

                    string sInsertQuery = "insert into tbl_Users(" + sColumnNames + ") values(" + sParameter + ")";

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
                    //    sResult = "UserName already exists.";
                    //}
                }
                else
                {
                    sResult = "UserCode already exists.";
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

        public DataSet SearchUser(string sUserCode, string sCompany)
        {
            try
            {
                sFuncName = "SearchUser()";

                sql = "SELECT usr.*,cmp.Name [CompanyName] FROM tbl_Users usr INNER JOIN tbl_CompanyData cmp ON cmp.Code = usr.CompanyCode WHERE UserCode = '" + sUserCode + "'";
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

        public string UpdateUser(DataTable dt, string oldCompanyCode)
        {
            try
            {
                sFuncName = "UpdateUser()";

                string sUpdateQuery = "Update tbl_Users SET UserName = '" + dt.Rows[0]["UserName"].ToString() + "' ,Password = '" + dt.Rows[0]["Password"].ToString() + "'" +
                " ,RoleId = '" + dt.Rows[0]["RoleId"].ToString() + "' ,Email = '" + dt.Rows[0]["Email"].ToString() + "' ,IsActive = '" + dt.Rows[0]["IsActive"].ToString() + "'" +
                " ,CompanyCode = '" + dt.Rows[0]["CompanyCode"].ToString() + "'" +
                " where UserCode = '" + dt.Rows[0]["UserCode"].ToString() + "' and CompanyCode = '" + oldCompanyCode + "'";

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

        #endregion

    }
}
