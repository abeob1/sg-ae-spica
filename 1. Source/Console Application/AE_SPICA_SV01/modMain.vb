Module modMain

#Region "Varibles Declarations"

    'Company Default Structure

    Public Structure CompanyDefault

       Public sServer As String
        Public sLicenseServer As String
        Public sDBName As String
        Public sServerType As String
        Public sSAPUser As String
        Public sSAPPwd As String
        Public sSAPDBName As String
        Public sDBUser As String
        Public sDBPwd As String

        Public sDebug As String
        Public sFilepath As String

    End Structure


    ' Return Value Variable Control
    Public Const RTN_SUCCESS As Int16 = 1
    Public Const RTN_ERROR As Int16 = 0
    ' Debug Value Variable Control
    Public Const DEBUG_ON As Int16 = 1
    Public Const DEBUG_OFF As Int16 = 0

    ' Global variables group
    Public p_iDebugMode As Int16 = DEBUG_ON
    Public p_iErrDispMethod As Int16
    Public p_iDeleteDebugLog As Int16
    Public p_oCompDef As CompanyDefault
    Public p_dProcessing As DateTime
    Public p_oDtSuccess As DataTable
    Public p_oDtError As DataTable
    Public p_SyncDateTime As String
    Public p_HoCompany As SAPbobsCOM.Company

    Public p_sSAPEntityName As String = String.Empty
    Public p_sSAPUName As String = String.Empty
    Public p_sSAPUPass As String = String.Empty

    'Global DataTable

    Public p_oEntitesDetails As DataTable
    Public p_oGLAccount As DataTable
    Public p_oSTOLDCODE As DataTable
    Public p_oDTCompanyData As DataTable
    Public p_oDTImportStatistics As DataTable
    Public p_oDTPeriodCode As DataTable
    Public p_oDTiPowerPeriod As DataTable
    Public p_oDTSAPPeriod As DataTable
    Public P_sQueryString As String = String.Empty
    Public oDT_OUCODE As DataTable = Nothing
    Public oDT_StatisticsRowCount As DataTable = Nothing


#End Region

    Sub Main()

        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Dim strConnection As Array
        Dim sSQL As String = String.Empty
        Dim sApp As String = String.Empty
        Dim oDT_BranchEntities As DataTable = Nothing
        Dim oDT_BPCode As DataTable = Nothing
        Dim oRset_HL As SAPbobsCOM.Recordset = Nothing
        Dim sCode As String = String.Empty
        Dim oBranchCompany() As SAPbobsCOM.Company = Nothing
        Dim imjs As Integer = 0
        Dim sBPCode As String = String.Empty
        Dim oDT_Log As DataTable = Nothing
        Try
            sFuncName = "Main"

            Console.WriteLine("Starting Function", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oDT_Log = New DataTable()
            oDT_Log.Columns.Add("Code", GetType(String))
            oDT_Log.Columns.Add("Status", GetType(String))
            oDT_Log.Columns.Add("Msg", GetType(String))
            p_HoCompany = New SAPbobsCOM.Company
            Console.WriteLine("Calling GetSystemIntializeInfo() ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSystemIntializeInfo()", sFuncName)
            If GetSystemIntializeInfo(p_oCompDef, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            sApp = p_oCompDef.sSAPDBName & ";" & p_oCompDef.sSAPUser & ";" & p_oCompDef.sSAPPwd & ";" & p_oCompDef.sServer & ";" & p_oCompDef.sDBUser & ";" & p_oCompDef.sDBPwd & ";" & p_oCompDef.sLicenseServer & ";" & p_oCompDef.sServerType
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Connection " & sApp, sFuncName)
            strConnection = sApp.Split(";")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function ConnectToCompany()", sFuncName)
            Console.WriteLine("Calling Function ConnectToCompany() ", sFuncName)
            If (ConnectToCompany(strConnection, p_HoCompany, sErrDesc) <> 0) Then
                Throw New Exception(sErrDesc)
            End If

            oRset_HL = p_HoCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sSQL = "SELECT T0.[Code], T0.[Name], T0.[U_AE_Connection] FROM [dbo].[@BRANCH_MASTER]  T0"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Branch Master " & sSQL, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ExecuteSQLQuery_DT() ", sFuncName)
            oDT_BranchEntities = ExecuteSQLQuery_DT(sSQL, sErrDesc)
            If Not String.IsNullOrEmpty(sErrDesc) Then Throw New ArgumentException(sErrDesc)

            sSQL = "SELECT Top(6)  T0.[Code], T0.[Name], T0.[U_BPCode] FROM [dbo].[@AB_BP_SYNC]  T0 WHERE T0.[U_Locked]  = 'N' and  ISNULL(T0.[U_Status],'') <> 'SUCCESS'"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("BP SYNC " & sSQL, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ExecuteSQLQuery_DT() ", sFuncName)
            oDT_BPCode = ExecuteSQLQuery_DT(sSQL, sErrDesc)
            If Not String.IsNullOrEmpty(sErrDesc) Then Throw New ArgumentException(sErrDesc)
            If oDT_BPCode.Rows.Count > 0 Then
                For Each odr As DataRow In oDT_BPCode.Rows
                    sCode += "'" & odr("Code") & "',"
                Next
                sCode = Left(sCode, sCode.Length - 1)
                sSQL = "update [@AB_BP_SYNC]  set U_Locked = 'Y' where Code in (" & sCode & ")"
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Locking the BP Code " & sSQL, sFuncName)
                oRset_HL.DoQuery(sSQL)
                ''------------- Branch Entities 
                If oDT_BranchEntities.Rows.Count > 0 Then
                    ReDim oBranchCompany(oDT_BranchEntities.Rows.Count)
                    For Each oEntities As DataRow In oDT_BranchEntities.Rows
                        oBranchCompany(imjs) = New SAPbobsCOM.Company
                        strConnection = oEntities("U_AE_Connection").ToString.Split(";")
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function ConnectToCompany()", sFuncName)
                        '' Console.WriteLine("Connecting to the Branch DB " & oBranchCompany(imjs).CompanyName, sFuncName)
                        If (ConnectToCompany(strConnection, oBranchCompany(imjs), sErrDesc) <> 0) Then
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(sErrDesc, sFuncName)
                            Call WriteToLogFile(sErrDesc, sFuncName)
                        End If
                        ''''-------------  BP Code
                        For Each oBPCode As DataRow In oDT_BPCode.Rows
                            sBPCode = oBPCode("U_BPCode").ToString.Trim()
                            Console.WriteLine("Updating BP " & oBranchCompany(imjs).CompanyName, sFuncName)
                            If UpdateBPtoAllBranches(sBPCode, p_HoCompany, oBranchCompany(imjs), sErrDesc) <> RTN_SUCCESS Then
                                WriteToLogFile_Sync(sBPCode.PadRight(20, " "c) & "  " & Now.ToLongDateString.PadRight(30, " "c) & "FAIL   " & "  " & sErrDesc)
                                oDT_Log.Rows.Add(oBPCode("Code").ToString.Trim(), "FAIL", sErrDesc)
                            Else
                                WriteToLogFile_Sync(sBPCode.PadRight(20, " "c) & "  " & Now.ToLongDateString.PadRight(30, " "c) & "SUCCESS" & "  " & sErrDesc)
                            End If
                        Next
                        imjs += 1
                    Next
                End If
                sSQL = "update [@AB_BP_SYNC]  set U_Locked = 'N', U_Status = 'SUCCESS', U_SYNCDate = GETDATE(), U_SYNCTime = '" & Now.ToShortTimeString & "' , U_ErrMsg = '' where Code in (" & sCode & ")"
                oRset_HL.DoQuery(sSQL)
                sSQL = ""
                If oDT_Log.Rows.Count > 0 Then
                    For Each olog As DataRow In oDT_Log.Rows
                        sSQL += "update [@AB_BP_SYNC]  set U_Status = 'FAIL', U_ErrMsg = '" & olog("Msg") & "' where Code = '" & olog("Code") & "'"
                    Next
                    oRset_HL.DoQuery(sSQL)
                End If
            Else
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("No BP Information for SYNC ", sFuncName)
            End If

            Console.WriteLine("Completed With SUCCESS ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS ", sFuncName)

        Catch ex As Exception
            Console.WriteLine("Completed with ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
        End Try

    End Sub

End Module
