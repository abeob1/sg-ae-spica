using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace AE_SPICA.DAL
{
    public class clsApproval
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
        public string strSelect = "-- Select --";

        #endregion

        #region Methods

        public DataSet GetApprovalDetails(string sCompanyCode, string sUserCode)
        {

            try
            {
                sFuncName = "GetApprovalDetails()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP005_ApprovalDetails] '" + sCompanyCode + "','" + sUserCode + "'";

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

        public string ApproveSelectedVessel(string sMonth, string ids, string sFileRefNos, string sCompanyCode, string sUserCode)
        {
            string sResult = string.Empty;
            try
            {
                sFuncName = "ApproveSelectedVessel()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP006_ApproveSelectedVessel] '" + sMonth + "','" + ids + "','" + sFileRefNos + "','" + sCompanyCode + "','" + sUserCode + "'";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling Run_StoredProcedure()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Procedure : " + sProcedureName, sFuncName);
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sProcedureName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
                sResult = "SUCCESS";
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

        public DataSet GetLastApprovedMonth(string sCompanyCode, string sUserCode)
        {
            string sResult = string.Empty;
            try
            {
                sFuncName = "GetLastApprovedMonth()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP007_GetLastApprovedMonth] '" + sCompanyCode + "','" + sUserCode + "'";

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

        public string RejectSelectedVessel(string ids, string sFileRefNos, string sCompanyCode, string sUserCode)
        {
            string sResult = string.Empty;
            try
            {
                sFuncName = "RejectSelectedVessel()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP008_RejectSelectedVessel] '" + ids + "','" + sFileRefNos + "','" + sCompanyCode + "','" + sUserCode + "'";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Calling Run_StoredProcedure()", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Procedure : " + sProcedureName, sFuncName);
                dsResult = (DataSet)oDataAccess.ExecuteSqlString(sProcedureName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName);
                sResult = "UPDATE";
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

        public DataSet GetTimeEntryRecords(string sCompanyCode, string sUserCode)
        {

            try
            {
                sFuncName = "GetTimeEntryRecords()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP009_GetTimeEntryRecords] '" + sCompanyCode + "','" + sUserCode + "'";

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

        public DataSet GetExpenseEntryRecords(string sCompanyCode, string sUserCode)
        {

            try
            {
                sFuncName = "GetExpenseEntryRecords()";
                string sProcedureName = string.Empty;
                sProcedureName = "EXEC [AE_SP010_GetExpenseEntryRecords] '" + sCompanyCode + "','" + sUserCode + "'";

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
