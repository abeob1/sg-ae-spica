Imports System.Xml

Public Class HoldingCreateBP
    Dim WithEvents SBO_Application As SAPbouiCOM.Application
    Dim oCompany As SAPbobsCOM.Company
    'Dim oCompany_Branch As SAPbobsCOM.Company = New SAPbobsCOM.Company
    'Dim CardCode As String

    Sub New(ByVal ocompany1 As SAPbobsCOM.Company, ByVal sbo_application1 As SAPbouiCOM.Application)
        SBO_Application = sbo_application1
        oCompany = ocompany1
    End Sub
    Private Sub SBO_Application_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.FormDataEvent
        Dim oForm As SAPbouiCOM.Form = Nothing
        If BusinessObjectInfo.BeforeAction = False Then
            Select Case BusinessObjectInfo.FormTypeEx
                Case "134" 'BUSINESS PARTNER
                    Select Case BusinessObjectInfo.EventType
                        Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD
                            If Functions.CheckHO Then
                                If BusinessObjectInfo.ActionSuccess Then
                                    oForm = SBO_Application.Forms.Item(BusinessObjectInfo.FormUID)
                                    If oForm.DataSources.DBDataSources.Item(0).GetValue("U_AE_Sync", 0).Trim = "Y" Then
                                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function CreateBPtoAllBranches()", "BP Add Event")
                                        CreateBPtoAllBranches(oForm.DataSources.DBDataSources.Item(0).GetValue("CardCode", 0))
                                    End If
                                End If
                            End If
                        Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
                            If Functions.CheckHO Then
                                If BusinessObjectInfo.ActionSuccess Then
                                    oForm = SBO_Application.Forms.Item(BusinessObjectInfo.FormUID)
                                    If oForm.DataSources.DBDataSources.Item(0).GetValue("U_AE_Sync", 0).Trim = "Y" Then
                                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function UpdateBPtoAllBranches()", "BP Update Event")
                                        UpdateBPtoAllBranches(oForm.DataSources.DBDataSources.Item(0).GetValue("CardCode", 0))
                                    End If
                                End If
                            End If
                    End Select
            End Select
        End If
    End Sub
    Private Sub SBO_Application__AppEvent(ByVal EventType As SAPbouiCOM.BoAppEventTypes) Handles SBO_Application.AppEvent
        Dim sMessage As String = String.Empty
        Select Case EventType
            Case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged

                sMessage = String.Format("Please wait for a while to disconnect the AddOn {0} ....", System.Windows.Forms.Application.ProductName)
                SBO_Application.SetStatusBarMessage(sMessage, SAPbouiCOM.BoMessageTime.bmt_Medium, False)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(sMessage, "App Event")
                System.Windows.Forms.Application.Exit()
            Case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged
            Case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition

                sMessage = String.Format("Please wait for a while to disconnect the AddOn {0} ....", System.Windows.Forms.Application.ProductName)
                SBO_Application.SetStatusBarMessage(sMessage, SAPbouiCOM.BoMessageTime.bmt_Medium, False)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(sMessage, "App Event")
                System.Windows.Forms.Application.Exit()
            Case SAPbouiCOM.BoAppEventTypes.aet_ShutDown

                sMessage = String.Format("Please wait for a while to disconnect the AddOn {0} ....", System.Windows.Forms.Application.ProductName)
                SBO_Application.SetStatusBarMessage(sMessage, SAPbouiCOM.BoMessageTime.bmt_Medium, False)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(sMessage, "App Event")
                System.Windows.Forms.Application.Exit()
        End Select
    End Sub

    Private Sub UpdateBPtoAllBranches(CardCode As String)
        Dim oBP As SAPbobsCOM.BusinessPartners = Nothing
        'Dim oContact As SAPbobsCOM.ContactEmployees
        'Dim brachContact As SAPbobsCOM.Contacts
        Dim ors As SAPbobsCOM.Recordset = Nothing
        Dim orsB As SAPbobsCOM.Recordset = Nothing
        Dim orsGroup As SAPbobsCOM.Recordset = Nothing
        Dim branchBP As SAPbobsCOM.BusinessPartners = Nothing
        Dim strConnection As Array
        Dim GroupName As String = ""
        Dim sFuncName As String = String.Empty
        sFuncName = "UpdateBPtoAllBranches()"
        Dim sSQL As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Dim oDVContact As DataView = Nothing

        oBP = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

        If oBP.GetByKey(CardCode) Then
            Try
                Dim sErrMsg As String = ""
                Dim sErrCode As Integer = 0
                'If oBP.UserFields.Fields.Item("U_AE_JPI").Value = "N" Then
                '    Return
                'End If
                'oCompany.StartTransaction()
                Dim brachCompany = New SAPbobsCOM.Company

                ors = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                ors.DoQuery("Select * from [@BRANCH_MASTER]")
                ors.MoveFirst()

                While Not ors.EoF
                    strConnection = ors.Fields.Item("U_AE_Connection").Value.ToString.Split(";")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function ConnectToCompany()", sFuncName)
                    If (Functions.ConnectToCompany(strConnection, brachCompany) <> 0) Then
                        Throw New Exception(Functions.ErrorMessage)
                    End If

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

                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Update BP " & CardCode, sFuncName)
                        If branchBP.GetByKey(CardCode) Then
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
                            CreateBPtoAllBranches(CardCode, branchBP, oBP, brachCompany)
                        End If
                    End If
                    ors.MoveNext()
                End While
                SBO_Application.MessageBox("Business Partner is updated for all branches")
            Catch ex As Exception
                'If oCompany.InTransaction Then
                '    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                'End If
                SBO_Application.MessageBox("Error: " + ex.Message)
            Finally
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ors)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(orsGroup)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(branchBP)

                ors = Nothing
                oBP = Nothing
                orsGroup = Nothing
                branchBP = Nothing

            End Try
        Else
            SBO_Application.MessageBox("Error: CardCode not found!!!!")
        End If
    End Sub

    Private Sub UpdateBPtoAllBranches_OLD(CardCode As String)
        Dim oBP As SAPbobsCOM.BusinessPartners = Nothing
        'Dim oContact As SAPbobsCOM.ContactEmployees
        'Dim brachContact As SAPbobsCOM.Contacts
        Dim ors As SAPbobsCOM.Recordset = Nothing
        Dim orsGroup As SAPbobsCOM.Recordset = Nothing
        Dim branchBP As SAPbobsCOM.BusinessPartners = Nothing
        Dim strConnection As Array
        Dim GroupName As String = ""
        Dim sFuncName As String = String.Empty
        sFuncName = "UpdateBPtoAllBranches()"
        oBP = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

        If oBP.GetByKey(CardCode) Then
            Try
                Dim sErrMsg As String = ""
                Dim sErrCode As Integer = 0
                'If oBP.UserFields.Fields.Item("U_AE_JPI").Value = "N" Then
                '    Return
                'End If
                'oCompany.StartTransaction()
                Dim brachCompany = New SAPbobsCOM.Company

                ors = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                ors.DoQuery("Select * from [@BRANCH_MASTER]")
                ors.MoveFirst()

                While Not ors.EoF
                    strConnection = ors.Fields.Item("U_AE_Connection").Value.ToString.Split(";")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function ConnectToCompany()", sFuncName)
                    If (Functions.ConnectToCompany(strConnection, brachCompany) <> 0) Then
                        Throw New Exception(Functions.ErrorMessage)
                    End If
                    If brachCompany.Connected Then
                        branchBP = brachCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Update BP " & CardCode, sFuncName)
                        If branchBP.GetByKey(CardCode) Then
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
                            If branchBP.ContactEmployees.Count > 0 Then
                                Dim delete As Boolean = False

                                For x As Integer = 0 To oBP.ContactEmployees.Count - 1
                                    oBP.ContactEmployees.SetCurrentLine(x)
                                    Dim foundContact As Boolean = False
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("foundContact = True", sFuncName)
                                    For i As Integer = 0 To branchBP.ContactEmployees.Count - 1
                                        branchBP.ContactEmployees.SetCurrentLine(i)
                                        Dim branchInternalCode As Integer = 0
                                        Integer.TryParse(branchBP.ContactEmployees.Remarks2, branchInternalCode)
                                        If oBP.ContactEmployees.InternalCode = branchInternalCode Then
                                            foundContact = True

                                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Contact Employee " & oBP.ContactEmployees.Name & " - " & oBP.ContactEmployees.FirstName, sFuncName)
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
                                        End If
                                    Next
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("foundContact = True OUT", sFuncName)
                                    If foundContact = False Then
                                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("foundContact = False", sFuncName)
                                        Dim addNew As Boolean = True
                                        For i As Integer = 0 To branchBP.ContactEmployees.Count - 1
                                            branchBP.ContactEmployees.SetCurrentLine(i)
                                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(oBP.ContactEmployees.Name & " - " & oBP.ContactEmployees.FirstName, sFuncName)
                                            If branchBP.ContactEmployees.Name = oBP.ContactEmployees.Name Then
                                                addNew = False
                                            End If
                                        Next
                                        If addNew Then
                                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("addNew", sFuncName)
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
                                        End If
                                    End If
                                Next
                            End If
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("foundContact = False OUT", sFuncName)
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
                            CreateBPtoAllBranches(CardCode, branchBP, oBP, brachCompany)
                        End If
                    End If
                    ors.MoveNext()
                End While
                SBO_Application.MessageBox("Business Partner is updated for all branches")
            Catch ex As Exception
                'If oCompany.InTransaction Then
                '    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                'End If
                SBO_Application.MessageBox("Error: " + ex.Message)
            Finally
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ors)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(orsGroup)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(branchBP)

                ors = Nothing
                oBP = Nothing
                orsGroup = Nothing
                branchBP = Nothing

            End Try
        Else
            SBO_Application.MessageBox("Error: CardCode not found!!!!")
        End If
    End Sub
    Public Sub CreateBPtoAllBranches(CardCode As String, branchBP As SAPbobsCOM.BusinessPartners, oBP As SAPbobsCOM.BusinessPartners, branchCompany As SAPbobsCOM.Company)
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
            Catch ex As Exception
                Throw ex
                'SBO_Application.MessageBox("Error: " + ex.Message)
            Finally
                ''System.Runtime.InteropServices.Marshal.ReleaseComObject(ors)
                ''System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP)
                ''System.Runtime.InteropServices.Marshal.ReleaseComObject(branchBP)
                ors = Nothing
                oBP = Nothing
                branchBP = Nothing

            End Try
        Else
            SBO_Application.MessageBox("Error: CardCode not found!!!!")
        End If
    End Sub
    Public Sub CreateBPtoAllBranches(CardCode As String)
        Dim oBP As SAPbobsCOM.BusinessPartners = Nothing
        Dim ors As SAPbobsCOM.Recordset = Nothing
        Dim orsGroup As SAPbobsCOM.Recordset = Nothing
        Dim branchBP As SAPbobsCOM.BusinessPartners = Nothing
        '' Dim branchCurrency As SAPbobsCOM.Currencies = Nothing
        Dim strConnection As Array
        Dim GroupName As String = ""
        Dim sFuncName As String = String.Empty

        oBP = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
        If oBP.GetByKey(CardCode) Then
            Try
                sFuncName = "CreateBPtoAllBranches()"
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
                Dim brachCompany = New SAPbobsCOM.Company
                ors = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Fetching Company Information " & "Select * from [@BRANCH_MASTER]", sFuncName)
                ors.DoQuery("Select * from [@BRANCH_MASTER]")
                ors.MoveFirst()
                While Not ors.EoF
                    strConnection = ors.Fields.Item("U_AE_Connection").Value.ToString.Split(";")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function ConnectToCompany() ", sFuncName)
                    If (Functions.ConnectToCompany(strConnection, brachCompany) <> 0) Then
                        Throw New Exception(Functions.ErrorMessage)
                    End If
                    ''  SBO_Application.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
                    If brachCompany.Connected Then
                        branchBP = brachCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                        ''  branchCurrency = brachCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCurrencyCodes)
                        If branchBP.GetByKey(oBP.CardCode) Then
                            Throw New Exception(String.Format("BP : {0} aldready existed in Branch : {1}", oBP.CardCode, brachCompany.CompanyName))
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

                        orsGroup = brachCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
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
                            branchBP.Addresses.FederalTaxID = oBP.Addresses.FederalTaxID
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
                            brachCompany.GetLastError(errCode, errMess)
                            Throw New Exception("Could not create BP to Branch Company" + " - " + errMess)
                        End If
                    End If
                    ors.MoveNext()
                End While
                SBO_Application.MessageBox("Business Partner is created for all branches")
            Catch ex As Exception
                SBO_Application.MessageBox("Error: " + ex.Message)
            Finally
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ors)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(orsGroup)

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(branchBP)
                oBP = Nothing
                branchBP = Nothing
                ors = Nothing
                orsGroup = Nothing

                '' GC.Collect()
            End Try
        Else
            SBO_Application.MessageBox("Error: CardCode not found!!!!")
        End If
    End Sub

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
End Class
