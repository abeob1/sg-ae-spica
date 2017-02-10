Public Class Service1

    Public oEmailTrigger As New System.Timers.Timer
    Public strConnection As Array
    Public p_HoCompany As SAPbobsCOM.Company
    Public sApp As String = String.Empty



    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
       
        Try
            p_iDebugMode = DEBUG_ON

            sFuncName = "Onstart()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSystemIntializeInfo() ", sFuncName)
            If GetSystemIntializeInfo(p_oCompDef, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            sApp = p_oCompDef.sSAPDBName & ";" & p_oCompDef.sSAPUser & ";" & p_oCompDef.sSAPPwd & ";" & p_oCompDef.sServer & ";" & p_oCompDef.sDBUser & ";" & p_oCompDef.sDBPwd & ";" & p_oCompDef.sLicenseServer & ";" & p_oCompDef.sServerType
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Connection " & sApp, sFuncName)
            p_HoCompany = New SAPbobsCOM.Company
            strConnection = sApp.Split(";")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function ConnectToCompany()", sFuncName)
            If (ConnectToCompany(strConnection, p_HoCompany, sErrDesc) <> 0) Then
                Throw New Exception(sErrDesc)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("BP SYNC Service Starts  " & Format(Now.Date, "dd-MMM-yyyy"), sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("----------------------------------------------------------------------", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Timer Activated  ", sFuncName)
            oEmailTrigger.Interval = 300000
            oEmailTrigger.Start()
            AddHandler oEmailTrigger.Elapsed, AddressOf BPSync

        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With ERROR (OnStart)  " & ex.Message, sFuncName)
            Call WriteToLogFile("(OnStart) " & ex.Message, sFuncName)
        End Try

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.

        Try
            p_iDebugMode = DEBUG_ON

            sFuncName = "Onstop()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("BP SYNC Service Stops  " & Format(Now.Date, "dd-MMM-yyyy"), sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("----------------------------------------------------------------------", sFuncName)
            oEmailTrigger.Stop()

        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With ERROR (OnStop)  " & ex.Message, sFuncName)
            Call WriteToLogFile("(OnStop) " & ex.Message, sFuncName)
        End Try


    End Sub


    Private Sub BPSync()

        ' **********************************************************************************
        '   Function   :    EmailNotifiation()
        '   Purpose    :    This function Trigger Emails if any PO /PR send for an approval
        '
        '   Author     :    JOHN
        '   Date       :    18 Aug 2016
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim oDT_BranchEntities As DataTable = Nothing
        Dim oDT_BPCode As DataTable = Nothing
        Dim oRset_HL As SAPbobsCOM.Recordset = p_HoCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Dim sCode As String = String.Empty
        Dim oBranchCompany() As SAPbobsCOM.Company = Nothing
        Dim imjs As Integer = 0
        Dim sBPCode As String = String.Empty
        Dim oDT_Log As DataTable = Nothing
        p_iDebugMode = DEBUG_ON
        Try
            sFuncName = "BPSYNC()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function " & Now.ToLongDateString, sFuncName)
            oDT_Log = New DataTable()
            oDT_Log.Columns.Add("Code", GetType(String))
            oDT_Log.Columns.Add("Status", GetType(String))
            oDT_Log.Columns.Add("Msg", GetType(String))
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
                        If (ConnectToCompany(strConnection, oBranchCompany(imjs), sErrDesc) <> 0) Then
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(sErrDesc, sFuncName)
                            Call WriteToLogFile(sErrDesc, sFuncName)
                        End If
                        ''''-------------  BP Code
                        For Each oBPCode As DataRow In oDT_BPCode.Rows
                            sBPCode = oBPCode("U_BPCode").ToString.Trim()
                            If UpdateBPtoAllBranches(sBPCode, p_HoCompany, oBranchCompany(imjs), sErrDesc) <> RTN_SUCCESS Then
                                WriteToLogFile_Sync(sBPCode.PadRight(20, " "c) & "  " & Now.ToLongDateString.PadRight(30, " "c) & "FAIL   " & "  " & sErrDesc, "")
                                oDT_Log.Rows.Add(oBPCode("Code").ToString.Trim(), "FAIL", sErrDesc)
                            Else
                                WriteToLogFile_Sync(sBPCode.PadRight(20, " "c) & "  " & Now.ToLongDateString.PadRight(30, " "c) & "SUCCESS" & "  " & sErrDesc, "")
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
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("No Business Partner for SYNC ", sFuncName)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS ", sFuncName)
        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(sErrDesc, sFuncName)
            Call WriteToLogFile(sErrDesc, sFuncName)
        End Try
    End Sub

  
End Class
