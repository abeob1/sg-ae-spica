Option Explicit On
Imports System.Xml
Imports System.IO
Imports System.Windows.Forms
Imports System.Globalization
Imports System.Net.Mail
Imports System.Configuration
Imports System.Data.Sql
Imports System.Data.SqlClient



Module modCommon

    Public DateConversion As New System.Globalization.CultureInfo("fr-FR", True)

    ''Public Sub UpdateXML(ByVal oDICompany As SAPbobsCOM.Company, ByVal oDITargetComp As SAPbobsCOM.Company, _
    ''                         ByVal sNode As String, ByVal sTblName As String, ByVal sField1 As String, ByVal sField2 As String, _
    ''                         ByVal bIsNumeric As Boolean, ByRef oXMLDoc As XmlDocument, ByRef sXMLFile As String)

    ''    Dim oNode As XmlNode
    ''    Dim sFuncName As String = String.Empty
    ''    Dim sSQL As String = String.Empty
    ''    Dim oRs As SAPbobsCOM.Recordset
    ''    Dim iCode As Integer
    ''    Dim sCode As String = String.Empty

    ''    Try
    ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Updating " & sField1 & " in XML file..", sFuncName)
    ''        oRs = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

    ''        oNode = oXMLDoc.SelectSingleNode(sNode)

    ''        If Not IsNothing(oNode) Then
    ''            If Not oNode.InnerText = String.Empty Then
    ''                If bIsNumeric Then
    ''                    iCode = CInt(oNode.InnerText)

    ''                    If sTblName = "OLGT" Then
    ''                        If CInt(oNode.InnerText) = 0 Then iCode = 1
    ''                    End If


    ''                    sSQL = " SELECT " & sField1 & " from  [" & oDITargetComp.CompanyDB.ToString & "].[dbo]." & sTblName & _
    ''                           " WHERE " & sField2 & " in (select " & sField2 & " from " & sTblName & " WHERE " & sField1 & "=" & iCode & ")"
    ''                Else
    ''                    sCode = oNode.InnerText
    ''                    sSQL = " SELECT " & sField1 & " from  [" & oDITargetComp.CompanyDB.ToString & "].[dbo]." & sTblName & _
    ''                           " WHERE " & sField2 & " in (select " & sField2 & " from " & sTblName & " WHERE " & sField1 & "='" & sCode & "')"
    ''                End If

    ''                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing SQL Query" & sSQL, sFuncName)
    ''                oRs.DoQuery(sSQL)
    ''                If Not oRs.EoF Then
    ''                    oNode.InnerText = oRs.Fields.Item(0).Value
    ''                Else
    ''                    oNode.ParentNode.RemoveChild(oNode)
    ''                    oXMLDoc.Save(sXMLFile)
    ''                End If
    ''                oXMLDoc.Save(sXMLFile)
    ''            Else
    ''                oNode.ParentNode.RemoveChild(oNode)
    ''                oXMLDoc.Save(sXMLFile)
    ''            End If
    ''        End If

    ''    Catch ex As Exception

    ''    End Try

    ''End Sub

    ''Public Function GetDate(ByVal sDate As String, ByRef oCompany As SAPbobsCOM.Company) As String

    ''    Dim dateValue As DateTime
    ''    Dim DateString As String = String.Empty
    ''    Dim sSQL As String = String.Empty
    ''    Dim oRs As SAPbobsCOM.Recordset
    ''    Dim sDatesep As String

    ''    oRs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

    ''    sSQL = "SELECT DateFormat,DateSep FROM OADM"

    ''    oRs.DoQuery(sSQL)

    ''    If Not oRs.EoF Then
    ''        sDatesep = oRs.Fields.Item("DateSep").Value

    ''        Select Case oRs.Fields.Item("DateFormat").Value
    ''            Case 0
    ''                If Date.TryParseExact(sDate, "dd" & sDatesep & "MM" & sDatesep & "yy", _
    ''                   New CultureInfo("en-US"), _
    ''                   DateTimeStyles.None, _
    ''                   dateValue) Then
    ''                    DateString = dateValue.ToString("yyyyMMdd")
    ''                End If
    ''            Case 1
    ''                If Date.TryParseExact(sDate, "dd" & sDatesep & "MM" & sDatesep & "yyyy", _
    ''                   New CultureInfo("en-US"), _
    ''                   DateTimeStyles.None, _
    ''                   dateValue) Then
    ''                    DateString = dateValue.ToString("yyyyMMdd")
    ''                End If
    ''            Case 2
    ''                If Date.TryParseExact(sDate, "MM" & sDatesep & "dd" & sDatesep & "yy", _
    ''                    New CultureInfo("en-US"), _
    ''                    DateTimeStyles.None, _
    ''                    dateValue) Then
    ''                    DateString = dateValue.ToString("yyyyMMdd")
    ''                End If
    ''            Case 3
    ''                If Date.TryParseExact(sDate, "MM" & sDatesep & "dd" & sDatesep & "yyyy", _
    ''                    New CultureInfo("en-US"), _
    ''                    DateTimeStyles.None, _
    ''                    dateValue) Then
    ''                    DateString = dateValue.ToString("yyyyMMdd")
    ''                End If
    ''            Case 4
    ''                If Date.TryParseExact(sDate, "yyyy" & sDatesep & "MM" & sDatesep & "dd", _
    ''                    New CultureInfo("en-US"), _
    ''                    DateTimeStyles.None, _
    ''                    dateValue) Then
    ''                    DateString = dateValue.ToString("yyyyMMdd")
    ''                End If
    ''            Case 5
    ''                If Date.TryParseExact(sDate, "dd" & sDatesep & "MMMM" & sDatesep & "yyyy", _
    ''                    New CultureInfo("en-US"), _
    ''                    DateTimeStyles.None, _
    ''                    dateValue) Then
    ''                    DateString = dateValue.ToString("yyyyMMdd")
    ''                End If
    ''            Case 6
    ''                If Date.TryParseExact(sDate, "yy" & sDatesep & "MM" & sDatesep & "dd", _
    ''                    New CultureInfo("en-US"), _
    ''                    DateTimeStyles.None, _
    ''                    dateValue) Then
    ''                    DateString = dateValue.ToString("yyyyMMdd")
    ''                End If
    ''            Case Else
    ''                DateString = dateValue.ToString("yyyyMMdd")
    ''        End Select

    ''    End If

    ''    Return DateString

    ''End Function

    Public Sub Write_TextFile_Account(ByVal sAccount() As String)
        Try
            Dim irow As Integer
            Dim sPath As String = System.Windows.Forms.Application.StartupPath & "\"
            Dim sFileName As String = "AccountCode_NotMap.txt"
            Dim sbuffer As String = String.Empty

            If File.Exists(sPath & sFileName) Then
                Try
                    File.Delete(sPath & sFileName)
                Catch ex As Exception
                End Try
            End If

            Dim sw As StreamWriter = New StreamWriter(sPath & sFileName)
            ' Add some text to the file.
            sw.WriteLine("")
            sw.WriteLine("Error!  The following AccNumbers do not have a corresponding SAP G/L Account in the mapping table! ")
            sw.WriteLine("")
            sw.WriteLine("Account Code                       ")
            sw.WriteLine("=============================================================")
            sw.WriteLine(" ")

            For irow = 0 To sAccount.Length
                If Not String.IsNullOrEmpty(sAccount(irow)) Then
                    sw.WriteLine(sAccount(irow).ToString.PadRight(40, " "c))
                Else
                    Exit For
                End If
            Next irow

            sw.WriteLine(" ")
            sw.WriteLine("===============================================================")
            sw.WriteLine("Please create an entry for each of these invalid AccNumbers.")
            sw.Close()
            Process.Start(sPath & sFileName)


        Catch ex As Exception

        End Try

    End Sub

    Public Sub Write_TextFile_ActiveAccount(ByVal sAccount() As String)
        Try
            Dim irow As Integer
            Dim sPath As String = System.Windows.Forms.Application.StartupPath & "\"
            Dim sFileName As String = "AccountCode_ExistorInactive.txt"
            Dim sbuffer As String = String.Empty

            If File.Exists(sPath & sFileName) Then
                Try
                    File.Delete(sPath & sFileName)
                Catch ex As Exception
                End Try
            End If

            Dim sw As StreamWriter = New StreamWriter(sPath & sFileName)
            ' Add some text to the file.
            sw.WriteLine("")
            sw.WriteLine("Error!The following SAP G/L Accounts are not found in the Chart of Accounts or the Account is not an Active ! ")
            sw.WriteLine("")
            sw.WriteLine("Account Code                       ")
            sw.WriteLine("=============================================================")
            sw.WriteLine(" ")

            For irow = 0 To sAccount.Length
                If Not String.IsNullOrEmpty(sAccount(irow)) Then
                    sw.WriteLine(sAccount(irow).ToString.PadRight(40, " "c))
                Else
                    Exit For
                End If
            Next irow

            sw.WriteLine(" ")
            sw.WriteLine("===============================================================")
            sw.WriteLine("Please create an entry for each of these invalid Account Numbers in Chart of Accounts or make sure these accounts are Active.")
            sw.Close()
            Process.Start(sPath & sFileName)


        Catch ex As Exception

        End Try

    End Sub

    Public Sub Write_TextFile_Amount(ByVal sAmount(,) As String)
        Try
            Dim irow As Integer
            Dim sPath As String = System.Windows.Forms.Application.StartupPath & "\"
            Dim sFileName As String = "AccountCode_NotMap.txt"
            Dim sbuffer As String = String.Empty

            If File.Exists(sPath & sFileName) Then
                Try
                    File.Delete(sPath & sFileName)
                Catch ex As Exception
                End Try
            End If

            Dim sw As StreamWriter = New StreamWriter(sPath & sFileName)
            ' Add some text to the file.
            sw.WriteLine("")
            sw.WriteLine("Error!  The Total Debit is not equal to the Total Credit for the following group(RefNo)")
            sw.WriteLine("")
            sw.WriteLine("Debit Amount                 Credit Amount                Difference                       RefNo")
            sw.WriteLine("================================================================================================")
            sw.WriteLine(" ")

            For irow = 0 To UBound(sAmount, 1) - 1
                If Not String.IsNullOrEmpty(sAmount(irow, 0)) Then
                    sw.WriteLine(sAmount(irow, 0).ToString.PadRight(30, " "c) & sAmount(irow, 1).ToString.PadRight(30, " "c) & " " & sAmount(irow, 2).ToString.PadRight(30, " "c) & " " & sAmount(irow, 3))
                Else
                    Exit For
                End If
            Next irow

            sw.WriteLine(" ")
            sw.WriteLine("================================================================================================")
            sw.WriteLine("Please check the grouping of entries in the CSV file")
            sw.Close()
            Process.Start(sPath & sFileName)

        Catch ex As Exception

        End Try

    End Sub

    Public Function ExecuteSQLQuery_DT(ByVal sQuery As String, ByRef sErrDesc As String) As DataTable

        '**************************************************************
        ' Function      : ExecuteQuery
        ' Purpose       : Execute SQL
        ' Parameters    : ByVal sSQL - string command Text
        ' Author        : JOHN
        ' Date          : MAY 2014 20
        ' Change        :
        '**************************************************************
        Dim oDs As New DataSet
        If sQuery <> "" Then
            Dim sConstr As String = "Data Source=" & p_oCompDef.sServer & ";Initial Catalog=" & p_oCompDef.sSAPDBName & ";User ID=" & p_oCompDef.sDBUser & "; Password=" & p_oCompDef.sDBPwd

            Dim oCon As New SqlConnection(sConstr)
            Dim oCmd As New SqlCommand

            Dim sFuncName As String = String.Empty

            'Dim sConstr As String = "DRIVER={HDBODBC32};SERVERNODE={" & p_oCompDef.sServer & "};DSN=" & p_oCompDef.sDSN & ";UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & ";"

            Try
                sFuncName = "ExecExecuteSQLQuery_DT()"
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Connection Details " & sConstr, sFuncName)
                'oCon.ConnectionString = "DRIVER={HDBODBC};UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & " ;SERVERNODE=" & p_oCompDef.sServer & ";CS=" & p_oCompDef.sSAPDBName & ""
                ' oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & ";SERVERNODE=" & p_oCompDef.sServer & ";CS=" & p_oCompDef.sSAPDBName

                oCon.Open()
                oCmd.CommandType = CommandType.Text
                oCmd.CommandText = sQuery
                oCmd.Connection = oCon
                oCmd.CommandTimeout = 0
                Dim da As New SqlDataAdapter(oCmd)
                da.Fill(oDs)
                'Console.WriteLine("Completed Successfully. ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed Successfully.", sFuncName)
                sErrDesc = String.Empty
            Catch ex As Exception
                sErrDesc = ex.Message
                WriteToLogFile(ex.Message, sFuncName)
                'Console.WriteLine("Completed with ERROR ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                Throw New Exception(ex.Message)
            Finally
                oCon.Dispose()
            End Try
            Return oDs.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Public Function ExecuteSQLInsertQuery(ByVal sQuery As String)

        '**************************************************************
        ' Function      : ExecuteSQLInsertQuery
        ' Purpose       : Execute SQL
        ' Parameters    : ByVal sSQL - string command Text
        ' Author        : JOHN
        ' Date          : APRIL 2015 20
        ' Change        :
        '**************************************************************
        Dim oDs As New DataSet
        If sQuery <> "" Then
            Dim sConstr As String = "Data Source=" & p_oCompDef.sServer & ";Initial Catalog=" & p_oCompDef.sSAPDBName & ";User ID=" & p_oCompDef.sDBUser & "; Password=" & p_oCompDef.sDBPwd

            Dim oCon As New SqlConnection(sConstr)
            Dim oCmd As New SqlCommand

            Dim sFuncName As String = String.Empty

            'Dim sConstr As String = "DRIVER={HDBODBC32};SERVERNODE={" & p_oCompDef.sServer & "};DSN=" & p_oCompDef.sDSN & ";UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & ";"

            Try
                sFuncName = "ExecuteSQLQuery()"
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Connection Details " & sConstr, sFuncName)
                'oCon.ConnectionString = "DRIVER={HDBODBC};UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & " ;SERVERNODE=" & p_oCompDef.sServer & ";CS=" & p_oCompDef.sSAPDBName & ""
                ' oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & ";SERVERNODE=" & p_oCompDef.sServer & ";CS=" & p_oCompDef.sSAPDBName

                oCon.Open()
                oCmd.CommandType = CommandType.Text
                oCmd.CommandText = sQuery
                oCmd.Connection = oCon
                oCmd.CommandTimeout = 0
                oCmd.ExecuteNonQuery()
                ''Dim da As New SqlDataAdapter(oCmd)
                ''Try
                ''    da.Fill(oDs)
                ''Catch ex As Exception
                ''End Try

                'Console.WriteLine("Completed Successfully. ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed Successfully.", sFuncName)

            Catch ex As Exception
                WriteToLogFile(ex.Message, sFuncName)
                'Console.WriteLine("Completed with ERROR ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                Throw New Exception(ex.Message)
            Finally
                oCon.Dispose()
            End Try
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Public Function GetSystemIntializeInfo(ByRef oCompDef As CompanyDefault, ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   GetSystemIntializeInfo()
        '   Purpose     :   This function will be providing information about the initialing variables
        '               
        '   Parameters  :   ByRef oCompDef As CompanyDefault
        '                       oCompDef =  set the Company Default structure
        '                   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   MAY 2014
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sConnection As String = String.Empty
        Dim sSqlstr As String = String.Empty
        Try

            sFuncName = "GetSystemIntializeInfo()"
            ' Console.WriteLine("Starting Function", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)


            oCompDef.sServer = String.Empty
            oCompDef.sLicenseServer = String.Empty
            '' oCompDef.iServerLanguage = 3
            'oCompDef.iServerType = 7
            oCompDef.sSAPUser = String.Empty
            oCompDef.sSAPPwd = String.Empty
            oCompDef.sSAPDBName = String.Empty

            oCompDef.sDebug = String.Empty



            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Server")) Then
                oCompDef.sServer = ConfigurationManager.AppSettings("Server")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("ServerType")) Then
                oCompDef.sServerType = ConfigurationManager.AppSettings("ServerType")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LicenseServer")) Then
                oCompDef.sLicenseServer = ConfigurationManager.AppSettings("LicenseServer")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPDBName")) Then
                oCompDef.sSAPDBName = ConfigurationManager.AppSettings("SAPDBName")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPUserName")) Then
                oCompDef.sSAPUser = ConfigurationManager.AppSettings("SAPUserName")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPPass")) Then
                oCompDef.sSAPPwd = ConfigurationManager.AppSettings("SAPPassword")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("DBUser")) Then
                oCompDef.sDBUser = ConfigurationManager.AppSettings("DBUser")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("DBPwd")) Then
                oCompDef.sDBPwd = ConfigurationManager.AppSettings("DBPwd")
            End If

            ' folder

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Debug")) Then
                oCompDef.sDebug = ConfigurationManager.AppSettings("Debug")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LogPath")) Then
                oCompDef.sFilepath = ConfigurationManager.AppSettings("LogPath")
            End If



            'Console.WriteLine("Completed with SUCCESS ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            GetSystemIntializeInfo = RTN_SUCCESS

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            'Console.WriteLine("Completed with ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GetSystemIntializeInfo = RTN_ERROR
        End Try
    End Function

    Public Function ConnectToCompany(ByVal Connection As Array, ByVal company As SAPbobsCOM.Company, ByRef sErrDesc As String) As Long
        Dim sErrMsg As String = ""
        Dim sErrCode As Integer
        If company.Connected Then
            company.Disconnect()
        End If
        company.CompanyDB = Connection(0).ToString()
        company.UserName = Connection(1).ToString()
        company.Password = Connection(2).ToString()
        company.Server = Connection(3).ToString()
        company.DbUserName = Connection(4).ToString()
        company.DbPassword = Connection(5).ToString()
        company.LicenseServer = Connection(6)
        If Connection(7) = "2008" Then
            company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
        ElseIf Connection(7) = "2005" Then
            company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005
        ElseIf Connection(7) = "2012" Then
            company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
        End If

        sErrDesc = String.Empty
        'company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
        If company.Connect <> 0 Then
            company.GetLastError(sErrCode, sErrMsg)
            sErrDesc = sErrMsg
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Company not connected " & Connection(0).ToString() & " - " & sErrMsg, "ConnectToCompany()")
        Else
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Company connected  successfully " & Connection(0).ToString(), "ConnectToCompany()")
        End If
        Return sErrCode
    End Function

    Public Function UpdateBPtoAllBranches(CardCode As String, ByRef oCompany As SAPbobsCOM.Company, ByRef brachCompany As SAPbobsCOM.Company, ByRef sErrDesc As String) As Long
        Dim oBP As SAPbobsCOM.BusinessPartners = Nothing
        'Dim oContact As SAPbobsCOM.ContactEmployees
        'Dim brachContact As SAPbobsCOM.Contacts
        Dim orsB As SAPbobsCOM.Recordset = Nothing
        Dim orsGroup As SAPbobsCOM.Recordset = Nothing
        Dim branchBP As SAPbobsCOM.BusinessPartners = Nothing
        Dim strConnection As Array
        Dim GroupName As String = ""
        Dim sFuncName As String = String.Empty
        sFuncName = "UpdateBPtoAllBranches()"
        Dim sSQL As String = String.Empty
        Dim oDVContact As DataView = Nothing

        oBP = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

        If oBP.GetByKey(CardCode) Then
            Try
                Dim sErrMsg As String = ""
                Dim sErrCode As Integer = 0

                ''   SBO_Application.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
                If brachCompany.Connected Then
                    Dim bfound As Boolean = False
                    orsB = brachCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    branchBP = brachCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                    sSQL = "select  ROW_NUMBER() OVER(ORDER BY T1.CntctCode ) -1 [No], T1.CntctCode , T1.Name , T1.Position  from " & _
                          "ocpr t1  where T1.cardcode = '" & CardCode & "' order by t1.CntctCode "
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Getting Contact Info " & sSQL, sFuncName)
                    orsB.DoQuery(sSQL)
                    oDVContact = New DataView(ConvertRecordset(orsB, sErrDesc))
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Conected with Company " & brachCompany.CompanyDB, sFuncName)
                    If branchBP.GetByKey(CardCode) Then
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Update BP " & CardCode, sFuncName)
                        branchBP.CardName = oBP.CardName
                        branchBP.CardType = oBP.CardType
                        branchBP.CompanyPrivate = oBP.CompanyPrivate
                        'branchBP.ContactPerson = oBP.ContactPerson
                        '' branchBP.Currency = oBP.Currency
                        branchBP.DiscountPercent = oBP.DiscountPercent
                        branchBP.Address = oBP.Address
                        branchBP.EmailAddress = oBP.EmailAddress
                        branchBP.Phone1 = oBP.Phone1
                        branchBP.Phone2 = oBP.Phone2
                        branchBP.Cellular = oBP.Cellular
                        branchBP.Fax = oBP.Fax
                        branchBP.Password = oBP.Password
                        branchBP.BusinessType = oBP.BusinessType
                        branchBP.AdditionalID = oBP.AdditionalID
                        branchBP.VatIDNum = oBP.VatIDNum
                        branchBP.FederalTaxID = oBP.FederalTaxID
                        branchBP.Notes = oBP.Notes
                        branchBP.FreeText = oBP.FreeText
                        branchBP.AliasName = oBP.AliasName
                        branchBP.GlobalLocationNumber = oBP.GlobalLocationNumber
                        branchBP.Valid = oBP.Valid
                        branchBP.Frozen = oBP.Frozen
                        orsGroup = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        orsGroup.DoQuery(String.Format("Select GroupName from OCRG where GroupCode = {0}", oBP.GroupCode))
                        GroupName = orsGroup.Fields.Item(0).Value

                        orsGroup = brachCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        orsGroup.DoQuery(String.Format("Select GroupCode from OCRG where GroupName = '{0}'", GroupName))
                        If orsGroup.RecordCount = 1 Then
                            branchBP.GroupCode = orsGroup.Fields.Item(0).Value
                        End If
                        branchBP.DebitorAccount = oBP.DebitorAccount
                        If branchBP.Valid = SAPbobsCOM.BoYesNoEnum.tYES Then
                            branchBP.ValidFrom = oBP.ValidFrom
                            branchBP.ValidTo = oBP.ValidTo
                            branchBP.ValidRemarks = oBP.ValidRemarks
                        End If
                        If branchBP.Frozen = SAPbobsCOM.BoYesNoEnum.tYES Then
                            branchBP.FrozenFrom = oBP.FrozenFrom
                            branchBP.FrozenTo = oBP.FrozenTo
                            branchBP.FrozenRemarks = oBP.FrozenRemarks
                        End If
                        If branchBP.Addresses.Count > 0 Then

                            Dim delete As Boolean = False
                            For i As Integer = 0 To branchBP.Addresses.Count - 1
                                branchBP.Addresses.SetCurrentLine(branchBP.Addresses.Count - 1)
                                branchBP.Addresses.Delete()
                                If branchBP.Addresses.Count = 0 Then
                                    Exit For
                                End If
                            Next
                        End If
                        'Handle add update/add new Address
                        For i As Integer = 0 To oBP.Addresses.Count - 1
                            oBP.Addresses.SetCurrentLine(i)
                            branchBP.Addresses.AddressName = oBP.Addresses.AddressName
                            branchBP.Addresses.AddressName2 = oBP.Addresses.AddressName2
                            branchBP.Addresses.AddressName3 = oBP.Addresses.AddressName3
                            branchBP.Addresses.AddressType = oBP.Addresses.AddressType
                            branchBP.Addresses.Block = oBP.Addresses.Block
                            branchBP.Addresses.City = oBP.Addresses.City
                            branchBP.Addresses.County = oBP.Addresses.County
                            branchBP.Addresses.Country = oBP.Addresses.Country
                            branchBP.Addresses.StreetNo = oBP.Addresses.StreetNo
                            branchBP.Addresses.TypeOfAddress = oBP.Addresses.TypeOfAddress
                            branchBP.Addresses.State = oBP.Addresses.State
                            branchBP.Addresses.ZipCode = oBP.Addresses.ZipCode
                            branchBP.Addresses.Street = oBP.Addresses.Street
                            branchBP.Addresses.Add()
                        Next
                        branchBP.BilltoDefault = oBP.BilltoDefault
                        branchBP.ShipToDefault = oBP.ShipToDefault
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting Contact Employees", sFuncName)

                        If branchBP.ContactEmployees.Count = 0 Then
                            For imjs As Integer = 0 To oBP.ContactEmployees.Count - 1
                                oBP.ContactEmployees.SetCurrentLine(imjs)
                                ''branchBP.ContactEmployees.Add()
                                branchBP.ContactEmployees.Name = oBP.ContactEmployees.Name
                                branchBP.ContactEmployees.Position = oBP.ContactEmployees.Position
                                branchBP.ContactEmployees.E_Mail = oBP.ContactEmployees.E_Mail
                                branchBP.ContactEmployees.Phone1 = oBP.ContactEmployees.Phone1
                                branchBP.ContactEmployees.Phone2 = oBP.ContactEmployees.Phone2

                                '*****************ADDED ON 07/09/2015 STARTS**************
                                branchBP.ContactEmployees.FirstName = oBP.ContactEmployees.FirstName
                                branchBP.ContactEmployees.MiddleName = oBP.ContactEmployees.MiddleName
                                branchBP.ContactEmployees.LastName = oBP.ContactEmployees.LastName
                                branchBP.ContactEmployees.Title = oBP.ContactEmployees.Title
                                branchBP.ContactEmployees.Address = oBP.ContactEmployees.Address
                                branchBP.ContactEmployees.MobilePhone = oBP.ContactEmployees.MobilePhone
                                branchBP.ContactEmployees.Fax = oBP.ContactEmployees.Fax
                                branchBP.ContactEmployees.Pager = oBP.ContactEmployees.Pager
                                branchBP.ContactEmployees.Remarks1 = oBP.ContactEmployees.Remarks1
                                branchBP.ContactEmployees.Remarks2 = oBP.ContactEmployees.Remarks2
                                branchBP.ContactEmployees.Password = oBP.ContactEmployees.Password
                                branchBP.ContactEmployees.PlaceOfBirth = oBP.ContactEmployees.PlaceOfBirth
                                branchBP.ContactEmployees.DateOfBirth = oBP.ContactEmployees.DateOfBirth
                                branchBP.ContactEmployees.Gender = oBP.ContactEmployees.Gender
                                branchBP.ContactEmployees.Profession = oBP.ContactEmployees.Profession
                                branchBP.ContactEmployees.CityOfBirth = oBP.ContactEmployees.CityOfBirth
                                '*****************ADDED ON 07/09/2015 ENDS**************
                                branchBP.ContactEmployees.Add()
                            Next
                        ElseIf branchBP.ContactEmployees.Count > 0 Then
                            For imjs As Integer = 0 To oBP.ContactEmployees.Count - 1
                                oBP.ContactEmployees.SetCurrentLine(imjs)
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("oBP.ContactEmployees.Name " & oBP.ContactEmployees.Name, sFuncName)
                                If oBP.ContactEmployees.Name = "" Then Continue For
                                oDVContact.RowFilter = "Name='" & oBP.ContactEmployees.Name & "'"
                                If oDVContact.Count > 0 Then
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Index " & oDVContact(0)("No").ToString(), sFuncName)
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Name " & oDVContact(0)("Name").ToString(), sFuncName)
                                    branchBP.ContactEmployees.SetCurrentLine(Convert.ToInt32(oDVContact(0)("No").ToString()))
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Assigned", sFuncName)
                                    branchBP.ContactEmployees.Name = oBP.ContactEmployees.Name
                                    branchBP.ContactEmployees.Position = oBP.ContactEmployees.Position
                                    branchBP.ContactEmployees.E_Mail = oBP.ContactEmployees.E_Mail
                                    branchBP.ContactEmployees.Phone1 = oBP.ContactEmployees.Phone1
                                    branchBP.ContactEmployees.Phone2 = oBP.ContactEmployees.Phone2

                                    '*****************ADDED ON 07/09/2015 STARTS**************
                                    branchBP.ContactEmployees.FirstName = oBP.ContactEmployees.FirstName
                                    branchBP.ContactEmployees.MiddleName = oBP.ContactEmployees.MiddleName
                                    branchBP.ContactEmployees.LastName = oBP.ContactEmployees.LastName
                                    branchBP.ContactEmployees.Title = oBP.ContactEmployees.Title
                                    branchBP.ContactEmployees.Address = oBP.ContactEmployees.Address
                                    branchBP.ContactEmployees.MobilePhone = oBP.ContactEmployees.MobilePhone
                                    branchBP.ContactEmployees.Fax = oBP.ContactEmployees.Fax
                                    branchBP.ContactEmployees.Pager = oBP.ContactEmployees.Pager
                                    branchBP.ContactEmployees.Remarks1 = oBP.ContactEmployees.Remarks1
                                    branchBP.ContactEmployees.Remarks2 = oBP.ContactEmployees.Remarks2
                                    branchBP.ContactEmployees.Password = oBP.ContactEmployees.Password
                                    branchBP.ContactEmployees.PlaceOfBirth = oBP.ContactEmployees.PlaceOfBirth
                                    branchBP.ContactEmployees.DateOfBirth = oBP.ContactEmployees.DateOfBirth
                                    branchBP.ContactEmployees.Gender = oBP.ContactEmployees.Gender
                                    branchBP.ContactEmployees.Profession = oBP.ContactEmployees.Profession
                                    branchBP.ContactEmployees.CityOfBirth = oBP.ContactEmployees.CityOfBirth
                                Else

                                    branchBP.ContactEmployees.Add()
                                    branchBP.ContactEmployees.Name = oBP.ContactEmployees.Name
                                    branchBP.ContactEmployees.Position = oBP.ContactEmployees.Position
                                    branchBP.ContactEmployees.E_Mail = oBP.ContactEmployees.E_Mail
                                    branchBP.ContactEmployees.Phone1 = oBP.ContactEmployees.Phone1
                                    branchBP.ContactEmployees.Phone2 = oBP.ContactEmployees.Phone2

                                    '*****************ADDED ON 07/09/2015 STARTS**************
                                    branchBP.ContactEmployees.FirstName = oBP.ContactEmployees.FirstName
                                    branchBP.ContactEmployees.MiddleName = oBP.ContactEmployees.MiddleName
                                    branchBP.ContactEmployees.LastName = oBP.ContactEmployees.LastName
                                    branchBP.ContactEmployees.Title = oBP.ContactEmployees.Title
                                    branchBP.ContactEmployees.Address = oBP.ContactEmployees.Address
                                    branchBP.ContactEmployees.MobilePhone = oBP.ContactEmployees.MobilePhone
                                    branchBP.ContactEmployees.Fax = oBP.ContactEmployees.Fax
                                    branchBP.ContactEmployees.Pager = oBP.ContactEmployees.Pager
                                    branchBP.ContactEmployees.Remarks1 = oBP.ContactEmployees.Remarks1
                                    branchBP.ContactEmployees.Remarks2 = oBP.ContactEmployees.Remarks2
                                    branchBP.ContactEmployees.Password = oBP.ContactEmployees.Password
                                    branchBP.ContactEmployees.PlaceOfBirth = oBP.ContactEmployees.PlaceOfBirth
                                    branchBP.ContactEmployees.DateOfBirth = oBP.ContactEmployees.DateOfBirth
                                    branchBP.ContactEmployees.Gender = oBP.ContactEmployees.Gender
                                    branchBP.ContactEmployees.Profession = oBP.ContactEmployees.Profession
                                    branchBP.ContactEmployees.CityOfBirth = oBP.ContactEmployees.CityOfBirth
                                End If
                                ''branchBP.ContactEmployees.Add()
                            Next
                        End If
                        branchBP.ContactPerson = oBP.ContactPerson
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Updating BP " & CardCode, sFuncName)
                        If branchBP.Update() <> 0 Then
                            brachCompany.GetLastError(sErrCode, sErrMsg)
                            Throw New Exception("Could not update BP to Branch Company" + " - " + sErrMsg)
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR " & "Could not update BP to Branch Company" + " - " + sErrMsg, sFuncName)
                        Else
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
                        End If
                    Else
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function CreateBPtoAllBranches()", sFuncName)
                        If CreateBPtoAllBranches(CardCode, branchBP, oBP, brachCompany, oCompany, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                    End If
                End If

                UpdateBPtoAllBranches = RTN_SUCCESS
                sErrDesc = String.Empty

            Catch ex As Exception
                'If oCompany.InTransaction Then
                '    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                'End If
                sErrDesc = ex.Message
                '' oMail.Dispose()
                WriteToLogFile(ex.Message, sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                'Console.WriteLine("Completed with Error " & sFuncName)
                UpdateBPtoAllBranches = RTN_ERROR
            Finally
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP)
                ''System.Runtime.InteropServices.Marshal.ReleaseComObject(orsGroup)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(branchBP)
                oBP = Nothing
                orsGroup = Nothing
                branchBP = Nothing

            End Try
        Else
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error: CardCode not found!!!!", sFuncName)
        End If
    End Function

    Public Function CreateBPtoAllBranches(CardCode As String, branchBP As SAPbobsCOM.BusinessPartners, oBP As SAPbobsCOM.BusinessPartners, branchCompany As SAPbobsCOM.Company, oCompany As SAPbobsCOM.Company, ByRef sErrDesc As String) As Long
        Dim ors As SAPbobsCOM.Recordset = Nothing
        Dim GroupName As String = ""
        Dim sFuncName = "CreateBPtoAllBranches()"
        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
        If oBP.GetByKey(CardCode) Then
            Try
                ors = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                If branchBP.GetByKey(oBP.CardCode) Then
                    Throw New Exception(String.Format("BP : {0} aldready existed in Branch : {1}", oBP.CardCode, branchCompany.CompanyName))
                End If
                branchBP.CardCode = oBP.CardCode
                branchBP.CardName = oBP.CardName
                branchBP.CardType = oBP.CardType

                branchBP.CompanyPrivate = oBP.CompanyPrivate
                'branchBP.ContactPerson = oBP.ContactPerson
                'branchBP.Currency = oBP.Currency
                If oBP.CardType = SAPbobsCOM.BoCardTypes.cCustomer Then
                    branchBP.Currency = "##"
                End If
                branchBP.DiscountPercent = oBP.DiscountPercent
                branchBP.Address = oBP.Address
                branchBP.EmailAddress = oBP.EmailAddress
                branchBP.Phone1 = oBP.Phone1
                branchBP.Phone2 = oBP.Phone2
                branchBP.Cellular = oBP.Cellular
                branchBP.Fax = oBP.Fax
                branchBP.Password = oBP.Password
                branchBP.BusinessType = oBP.BusinessType
                branchBP.AdditionalID = oBP.AdditionalID
                branchBP.VatIDNum = oBP.VatIDNum
                branchBP.FederalTaxID = oBP.FederalTaxID
                branchBP.Notes = oBP.Notes
                branchBP.FreeText = oBP.FreeText
                branchBP.AliasName = oBP.AliasName
                branchBP.GlobalLocationNumber = oBP.GlobalLocationNumber
                branchBP.Valid = oBP.Valid
                branchBP.Frozen = oBP.Frozen
                ors.DoQuery(String.Format("Select GroupName from OCRG where GroupCode = {0}", oBP.GroupCode))
                GroupName = ors.Fields.Item(0).Value

                ors = branchCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                ors.DoQuery(String.Format("Select GroupCode from OCRG where GroupName = '{0}'", GroupName))
                If ors.RecordCount = 1 Then
                    branchBP.GroupCode = ors.Fields.Item(0).Value
                End If

                branchBP.DebitorAccount = oBP.DebitorAccount
                If branchBP.Valid = SAPbobsCOM.BoYesNoEnum.tYES Then
                    branchBP.ValidFrom = oBP.ValidFrom
                    branchBP.ValidTo = oBP.ValidTo
                    branchBP.ValidRemarks = oBP.ValidRemarks
                End If
                If branchBP.Frozen = SAPbobsCOM.BoYesNoEnum.tYES Then
                    branchBP.FrozenFrom = oBP.FrozenFrom
                    branchBP.FrozenTo = oBP.FrozenTo
                    branchBP.FrozenRemarks = oBP.FrozenRemarks
                End If

                For i As Integer = 0 To oBP.Addresses.Count - 1
                    oBP.Addresses.SetCurrentLine(i)
                    branchBP.Addresses.AddressName = oBP.Addresses.AddressName
                    branchBP.Addresses.AddressName2 = oBP.Addresses.AddressName2
                    branchBP.Addresses.AddressName3 = oBP.Addresses.AddressName3
                    branchBP.Addresses.AddressType = oBP.Addresses.AddressType
                    branchBP.Addresses.Block = oBP.Addresses.Block
                    branchBP.Addresses.City = oBP.Addresses.City
                    branchBP.Addresses.County = oBP.Addresses.County
                    branchBP.Addresses.Country = oBP.Addresses.Country
                    branchBP.Addresses.StreetNo = oBP.Addresses.StreetNo
                    branchBP.Addresses.TypeOfAddress = oBP.Addresses.TypeOfAddress
                    branchBP.Addresses.State = oBP.Addresses.State
                    branchBP.Addresses.ZipCode = oBP.Addresses.ZipCode
                    branchBP.Addresses.Street = oBP.Addresses.Street

                    branchBP.Addresses.Add()
                Next

                branchBP.BilltoDefault = oBP.BilltoDefault
                branchBP.ShipToDefault = oBP.ShipToDefault

                For i As Integer = 0 To oBP.ContactEmployees.Count - 1
                    oBP.ContactEmployees.SetCurrentLine(i)
                    branchBP.ContactEmployees.Name = oBP.ContactEmployees.Name
                    branchBP.ContactEmployees.FirstName = oBP.ContactEmployees.FirstName
                    branchBP.ContactEmployees.MiddleName = oBP.ContactEmployees.MiddleName
                    branchBP.ContactEmployees.LastName = oBP.ContactEmployees.LastName
                    branchBP.ContactEmployees.Title = oBP.ContactEmployees.Title
                    branchBP.ContactEmployees.Position = oBP.ContactEmployees.Position
                    branchBP.ContactEmployees.Address = oBP.ContactEmployees.Address
                    branchBP.ContactEmployees.Phone1 = oBP.ContactEmployees.Phone1
                    branchBP.ContactEmployees.Phone2 = oBP.ContactEmployees.Phone2
                    branchBP.ContactEmployees.MobilePhone = oBP.ContactEmployees.MobilePhone
                    branchBP.ContactEmployees.Fax = oBP.ContactEmployees.Fax
                    branchBP.ContactEmployees.E_Mail = oBP.ContactEmployees.E_Mail
                    branchBP.ContactEmployees.Pager = oBP.ContactEmployees.Pager
                    branchBP.ContactEmployees.Remarks1 = oBP.ContactEmployees.Remarks1
                    branchBP.ContactEmployees.Remarks2 = oBP.ContactEmployees.InternalCode
                    branchBP.ContactEmployees.CityOfBirth = oBP.ContactEmployees.CityOfBirth
                    branchBP.ContactEmployees.DateOfBirth = oBP.ContactEmployees.DateOfBirth
                    branchBP.ContactEmployees.Gender = oBP.ContactEmployees.Gender
                    branchBP.ContactEmployees.Profession = oBP.ContactEmployees.Profession
                    branchBP.ContactEmployees.Active = oBP.ContactEmployees.Active
                    branchBP.ContactEmployees.Add()
                Next

                branchBP.ContactPerson = oBP.ContactPerson

                If branchBP.Add() <> 0 Then
                    Dim errCode As Integer
                    Dim errMess As String = ""
                    branchCompany.GetLastError(errCode, errMess)
                    Throw New Exception("Could not create BP to Branch Company" + " - " + errMess)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR " & "Could not create BP to Branch Company" + " - " + errMess, sFuncName)
                Else
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
                End If

                CreateBPtoAllBranches = RTN_SUCCESS
                sErrDesc = String.Empty

            Catch ex As Exception
                sErrDesc = ex.Message
                '' oMail.Dispose()
                WriteToLogFile(ex.Message, sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                CreateBPtoAllBranches = RTN_ERROR
            Finally
                ''System.Runtime.InteropServices.Marshal.ReleaseComObject(ors)
                ''System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP)
                ''System.Runtime.InteropServices.Marshal.ReleaseComObject(branchBP)
                ''ors = Nothing
                ''oBP = Nothing
                ''branchBP = Nothing

            End Try
        Else
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error: CardCode not found!!!!", sFuncName)
        End If
    End Function

    Public Function ConvertRecordset(ByVal SAPRecordset As SAPbobsCOM.Recordset, ByRef sErrDesc As String) As DataTable

        Dim NewCol As DataColumn
        Dim NewRow As DataRow
        Dim ColCount As Integer
        Dim dtTable As DataTable = Nothing


        Try
            dtTable = New DataTable

            For ColCount = 0 To SAPRecordset.Fields.Count - 1
                NewCol = New DataColumn(SAPRecordset.Fields.Item(ColCount).Name)
                dtTable.Columns.Add(NewCol)
            Next

            Do Until SAPRecordset.EoF
                NewRow = dtTable.NewRow
                'populate each column in the row we're creating
                For ColCount = 0 To SAPRecordset.Fields.Count - 1
                    NewRow.Item(SAPRecordset.Fields.Item(ColCount).Name) = SAPRecordset.Fields.Item(ColCount).Value
                Next

                'Add the row to the datatable
                dtTable.Rows.Add(NewRow)
                SAPRecordset.MoveNext()
            Loop
            Return dtTable
        Catch ex As Exception
            sErrDesc = ex.Message
            Exit Function
        End Try


    End Function

End Module
