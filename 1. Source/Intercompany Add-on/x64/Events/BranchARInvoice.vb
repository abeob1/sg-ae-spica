﻿'Imports System.Xml

Public Class BranchARInvoice
    Dim WithEvents SBO_Application As SAPbouiCOM.Application
    Dim oCompany As SAPbobsCOM.Company
    Dim oForm As SAPbouiCOM.Form = Nothing
    Dim itemJPI As String = ""
    Dim accJPI As String = ""
    'Dim Docnum As String = ""
    'Dim oCompany_Holding = New SAPbobsCOM.Company
    Dim ContactCode As String = ""
    Sub New(ByVal ocompany1 As SAPbobsCOM.Company, ByVal sbo_application1 As SAPbouiCOM.Application)
        SBO_Application = sbo_application1
        oCompany = ocompany1
    End Sub
    Private Sub SBO_Application_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.FormDataEvent
        'Dim oForm As SAPbouiCOM.Form = Nothing
        If BusinessObjectInfo.BeforeAction = False Then
            Select Case BusinessObjectInfo.FormTypeEx
                Case "133" 'AR Invoice
                    Select Case BusinessObjectInfo.EventType
                        Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD To SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
                            If Not Functions.CheckHO Then
                                If BusinessObjectInfo.ActionSuccess And CreateDocumentForHoldings(oForm.DataSources.DBDataSources.Item(0).GetValue("CardCode", 0)) Then
                                    oForm = SBO_Application.Forms.Item(BusinessObjectInfo.FormUID)

                                    If BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Then
                                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function Add_Update_Documents()", "Branch ARInvoice Add Event")
                                        Add_Update_Documents(oForm.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0), False)
                                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ", "Branch ARInvoice Add Event")
                                    ElseIf BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE Then
                                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function Add_Update_Documents()", "Branch ARInvoice Update Event")
                                        Add_Update_Documents(oForm.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0), True)
                                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ", "Branch ARInvoice Update Event")
                                    End If
                                End If
                            End If
                    End Select
            End Select
        End If
    End Sub
    Private Sub SBO_Application_ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) Handles SBO_Application.ItemEvent
        Try
            If pVal.EventType <> SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD And pVal.EventType <> SAPbouiCOM.BoEventTypes.et_FORM_DEACTIVATE Then
                oForm = SBO_Application.Forms.Item(FormUID)
            End If
            If pVal.BeforeAction = False Then
                Select Case pVal.FormType
                    Case "133" 'SALES ORDER
                        If Not Functions.CheckHO Then
                            Select Case pVal.EventType
                                Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD
                                    Functions.DrawTabSO(oForm, SBO_Application, "OINV")
                                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                                    If pVal.ItemUID = "ClubFld" Then
                                        oForm.PaneLevel = 99
                                    End If
                                Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                                    Dim chooseFromListEvent As SAPbouiCOM.IChooseFromListEvent
                                    chooseFromListEvent = pVal
                                    Dim strChooseFromListId As String = chooseFromListEvent.ChooseFromListUID
                                    Dim chooseFromList As SAPbouiCOM.ChooseFromList = oForm.ChooseFromLists.Item(strChooseFromListId)
                                    Dim editText As SAPbouiCOM.EditText

                                    If (chooseFromListEvent.Before_Action = False) Then
                                        Dim dataTable As SAPbouiCOM.DataTable = Nothing
                                        dataTable = chooseFromListEvent.SelectedObjects

                                        If Not dataTable Is Nothing Then
                                            Dim strValue As String

                                            If pVal.ItemUID = "ClubCode" Then
                                                strValue = System.Convert.ToString(dataTable.GetValue(0, 0))
                                                editText = oForm.Items.Item(pVal.ItemUID).Specific
                                                Try
                                                    editText.Value = strValue
                                                Catch
                                                End Try

                                                strValue = System.Convert.ToString(dataTable.GetValue(1, 0))
                                                editText = oForm.Items.Item("ClubName").Specific
                                                Try
                                                    editText.Value = strValue
                                                Catch
                                                End Try
                                                editText = oForm.Items.Item("ClubAddr").Specific
                                                Try
                                                    editText.Value = Functions.GetAddress(System.Convert.ToString(dataTable.GetValue(0, 0)), oCompany)
                                                Catch
                                                End Try
                                                editText = oForm.Items.Item("ClubCntct").Specific
                                                Try
                                                    editText.Value = ""
                                                Catch
                                                End Try
                                                'ElseIf pVal.ItemUID = "ClubName" Then
                                                '    strValue = System.Convert.ToString(dataTable.GetValue(1, 0))
                                                '    editText = oForm.Items.Item(pVal.ItemUID).Specific
                                                '    Try
                                                '        editText.Value = strValue
                                                '    Catch
                                                '    End Try
                                                '    strValue = System.Convert.ToString(dataTable.GetValue(0, 0))
                                                '    editText = oForm.Items.Item("ClubCode").Specific
                                                '    Try
                                                '        editText.Value = strValue
                                                '    Catch
                                                '    End Try
                                            ElseIf pVal.ItemUID = "ClubCntct" Then
                                                strValue = System.Convert.ToString(dataTable.GetValue(2, 0))
                                                editText = oForm.Items.Item(pVal.ItemUID).Specific
                                                Try
                                                    editText.Value = strValue
                                                Catch
                                                End Try
                                            End If

                                        End If
                                    End If
                                Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                                    'If pVal.ItemUID = "ClubCode" Then
                                    '    Dim editText As SAPbouiCOM.EditText = oForm.Items.Item("ClubCode").Specific
                                    '    If editText.Value Is Nothing Or editText.Value = "" Then
                                    '        oForm.Items.Item("ClubCntct").Enabled = False
                                    '    Else
                                    '        oForm.Items.Item("ClubCntct").Enabled = True
                                    '    End If
                                    '    oForm.Items.Item("ClubCntct").Refresh()
                                    'End If
                            End Select
                        End If
                End Select
            Else
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        If Not Functions.CheckHO Then
                            If pVal.ItemUID = "ClubCntct" Then
                                Dim editText As SAPbouiCOM.EditText = oForm.Items.Item("ClubCode").Specific

                                Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
                                oCFLs = oForm.ChooseFromLists

                                Dim Conditions As SAPbouiCOM.Conditions = oCFLs.Item("CFL3").GetConditions()
                                If Conditions.Count = 1 Then
                                    Conditions.Item(0).CondVal = editText.Value
                                    oCFLs.Item("CFL3").SetConditions(Conditions)
                                Else
                                    Dim Condition As SAPbouiCOM.Condition = Conditions.Add()
                                    Condition.Alias = "CardCode"
                                    Condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                                    Condition.CondVal = editText.Value
                                    oCFLs.Item("CFL3").SetConditions(Conditions)
                                End If

                            End If
                        End If
                End Select
            End If
        Catch ex As Exception
            SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, True)
        End Try
    End Sub
    Private Sub Add_Update_Documents(DocEntry As String, isUpdate As Boolean)

        Dim oInvoice As SAPbobsCOM.Documents = Nothing
        Dim ors As SAPbobsCOM.Recordset = Nothing
        Dim record As SAPbobsCOM.Recordset = Nothing
        Dim vObj As SAPbobsCOM.SBObob = Nothing
        Dim errCode As Integer
        Dim errMess As String = ""
        Dim strConnection As Array
        Dim sErrMsg As String = ""
        Dim sErrCode As Integer = 0
        Dim invoiceCurrency As String
        Dim invoiceCurSource As String
        Try
            oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function " & DocEntry, "Add_Update_Documents()")
            If oInvoice.GetByKey(DocEntry) Then

                Dim isJPI As Boolean = oInvoice.UserFields.Fields.Item("U_AE_JPI").Value = "Y"

                oCompany.StartTransaction()
                ors = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                ors.DoQuery(String.Format("Select CurSource from OINV where DocEntry = {0}", DocEntry))
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(String.Format("Select CurSource from OINV where DocEntry = {0}", DocEntry), "Add_Update_Documents()")
                invoiceCurSource = ors.Fields.Item(0).Value
                vObj = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)

                If invoiceCurSource = "S" Then
                    ors = vObj.GetSystemCurrency()
                    invoiceCurrency = ors.Fields.Item(0).Value
                ElseIf invoiceCurSource = "L" Then
                    ors = vObj.GetLocalCurrency()
                    invoiceCurrency = ors.Fields.Item(0).Value
                Else
                    invoiceCurrency = oInvoice.DocCurrency
                End If

                Dim holdingCompany = New SAPbobsCOM.Company
                ors.DoQuery("Select * from [@HOLDINGS]")
                ors.MoveFirst()
                While Not ors.EoF
                    strConnection = ors.Fields.Item("U_AE_Connection").Value.ToString.Split(";")
                    If (Functions.ConnectToCompany(strConnection, holdingCompany) <> 0) Then
                        Throw New Exception(Functions.ErrorMessage)
                    End If
                    If holdingCompany.Connected Then
                        Try
                            record = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            Dim query As String = String.Format("Select CntctCode from OCPR Where Name = '{0}' and CardCode = '{1}'", oInvoice.UserFields.Fields.Item("U_AE_ClubContactPer").Value, oInvoice.UserFields.Fields.Item("U_AE_ClubCode").Value)
                            record.DoQuery(query)

                            If record.RecordCount > 0 Then
                                record.MoveFirst()
                                ContactCode = record.Fields.Item(0).Value
                            End If

                            holdingCompany.StartTransaction()
                            If isUpdate Then
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function UpdateHoldingSO()", "Add_Update_Documents()")
                                oInvoice.UserFields.Fields.Item("U_AE_HDSONo").Value = UpdateHoldingSO(holdingCompany, oInvoice, isJPI, invoiceCurrency)
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function UpdateHoldingAP()", "Add_Update_Documents()")
                                oInvoice.UserFields.Fields.Item("U_AE_HDAPInvNo").Value = UpdateHoldingAP(holdingCompany, oInvoice, isJPI, invoiceCurrency)
                            Else
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function CreateHoldingSO()", "Add_Update_Documents()")
                                oInvoice.UserFields.Fields.Item("U_AE_HDSONo").Value = CreateHoldingSO(holdingCompany, oInvoice, isJPI, invoiceCurrency)
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function CreateHoldingAP()", "Add_Update_Documents()")
                                oInvoice.UserFields.Fields.Item("U_AE_HDAPInvNo").Value = CreateHoldingAP(holdingCompany, oInvoice, isJPI, invoiceCurrency)
                            End If
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Update Invoice", "Add_Update_Documents()")
                            If oInvoice.Update() <> 0 Then
                                oCompany.GetLastError(errCode, errMess)
                                Throw New Exception(String.Format("Erro : {0} - {1}", errCode, errMess))
                            End If
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Updating the Invoice Successfully", "Add_Update_Documents()")
                            If holdingCompany.InTransaction Then
                                holdingCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            End If
                        Catch ex As Exception
                            If holdingCompany.InTransaction Then
                                holdingCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            End If
                            Throw New Exception(ex.Message)
                        End Try
                    End If
                    ors.MoveNext()
                End While
                Dim message As String
                If isUpdate Then
                    message = "Sale Order and A/P Invoice updated successfully in Spica Holdings"
                Else
                    message = "Sale Order and A/P Invoice created successfully in Spica Holdings"
                End If
                SBO_Application.MessageBox(message)
                If oCompany.InTransaction Then
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                End If
            Else
                SBO_Application.MessageBox("Error: Document not found!!!!")
            End If
        Catch ex As Exception
            If oCompany.InTransaction Then
                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            SBO_Application.MessageBox(String.Format("Error : {0}", ex.Message))
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(vObj)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oInvoice)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ors)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(record)

            vObj = Nothing
            oInvoice = Nothing
            ors = Nothing
            record = Nothing
            ''  GC.Collect()
        End Try
    End Sub
    Private Function CreateHoldingSO(holdingCompany As SAPbobsCOM.Company, branchARInvoice As SAPbobsCOM.Documents, isJPI As Boolean, brachInvCur As String) As String
        Dim holdingSO As SAPbobsCOM.Documents = Nothing
        Dim isBillUSD As Boolean
        Dim errCode As Integer
        Dim errMess As String = ""
        Dim sDocNum As String = String.Empty
        Try

            holdingSO = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            isBillUSD = branchARInvoice.UserFields.Fields.Item("U_AE_BillUSD").Value = "Y"

            CopyDocument(holdingCompany, branchARInvoice, holdingSO, False, branchARInvoice.UserFields.Fields.Item("U_AE_ClubCode").Value, isJPI, isBillUSD, brachInvCur)

            If holdingSO.Add = 0 Then
                Dim docEntry As String = ""
                Dim docTotal As Double = 0.0
                holdingCompany.GetNewObjectCode(docEntry)
                holdingSO.GetByKey(docEntry)
                If holdingSO.DocCurrency = "USD" Then
                    If holdingSO.DocTotalFc > 0 Then
                        docTotal = holdingSO.DocTotalFc
                    Else
                        docTotal = holdingSO.DocTotalSys
                    End If
                    docTotal = holdingSO.DocTotalSys
                Else
                    docTotal = holdingSO.DocTotal
                End If

                If isJPI And itemJPI <> "" And (docTotal Mod 100) > 0 Then
                    holdingSO.Lines.Add()
                    holdingSO.Lines.ItemCode = itemJPI
                    holdingSO.Lines.Quantity = 1
                    holdingSO.Lines.PriceAfterVAT = (100 - (docTotal Mod 100)) '* exchangeRate
                    holdingSO.Lines.AccountCode = accJPI
                    holdingSO.Update()
                End If
                sDocNum = holdingSO.DocNum
                Return sDocNum
            Else
                holdingCompany.GetLastError(errCode, errMess)
                Throw New Exception(String.Format("Error : {0} - {1}", errCode, "Failed to create Sale Order"))
            End If
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(holdingSO)
            holdingSO = Nothing
        End Try
    End Function
    Private Function UpdateHoldingSO(holdingCompany As SAPbobsCOM.Company, branchARInvoice As SAPbobsCOM.Documents, isJPI As Boolean, brachInvCur As String) As String
        Dim holdingSO As SAPbobsCOM.Documents = Nothing
        Dim ors As SAPbobsCOM.Recordset = Nothing
        Dim isBill As Boolean
        Dim errCode As Integer
        Dim errMess As String = ""
        Dim DocNum As String = ""

        Try
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", "UpdateHoldingSO()")

            ors = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim str As String = String.Format("Select DocEntry from ORDR where U_AE_BRARInvNo = '{0}' and U_AE_BRCode = '{1}'", branchARInvoice.DocNum, Functions.BranchCode)
            ors.DoQuery(String.Format("Select DocEntry from ORDR where U_AE_BRARInvNo = '{0}' and U_AE_BRCode = '{1}'", branchARInvoice.DocNum, Functions.BranchCode))
            holdingSO = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            isBill = branchARInvoice.UserFields.Fields.Item("U_AE_BillUSD").Value = "Y"
            If ors.RecordCount = 1 Then
                If holdingSO.GetByKey(ors.Fields.Item("DocEntry").Value) Then
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function CopyDocument()", "UpdateHoldingSO()")
                    CopyDocument(holdingCompany, branchARInvoice, holdingSO, True, branchARInvoice.UserFields.Fields.Item("U_AE_ClubCode").Value, isJPI, isBill, brachInvCur)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Update Holding SO ", "UpdateHoldingSO()")
                    If holdingSO.Update <> 0 Then
                        holdingCompany.GetLastError(errCode, errMess)
                        Throw New Exception(String.Format("Error : {0} - {1}", errCode, "Failed to update Sale Order"))
                    End If
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Updated Successfully ", "UpdateHoldingSO()")
                End If

                DocNum = holdingSO.DocNum
                Return DocNum
            ElseIf ors.RecordCount = 0 Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function CopyDocument()", "UpdateHoldingSO()")
                CopyDocument(holdingCompany, branchARInvoice, holdingSO, False, branchARInvoice.UserFields.Fields.Item("U_AE_ClubCode").Value, isJPI, isBill, brachInvCur)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Add Holding SO ", "UpdateHoldingSO()")
                If holdingSO.Add = 0 Then
                    Dim docEntry As String = ""
                    Dim docTotal As Double = 0.0
                    holdingCompany.GetNewObjectCode(docEntry)
                    holdingSO.GetByKey(docEntry)
                    If holdingSO.DocCurrency = "USD" Then
                        If holdingSO.DocTotalFc > 0 Then
                            docTotal = holdingSO.DocTotalFc
                        Else
                            docTotal = holdingSO.DocTotalSys
                        End If
                        docTotal = holdingSO.DocTotalSys
                    Else
                        docTotal = holdingSO.DocTotal
                    End If
                    If isJPI And itemJPI <> "" And (holdingSO.DocTotal Mod 100) > 0 Then
                        holdingSO.Lines.Add()
                        holdingSO.Lines.ItemCode = itemJPI
                        holdingSO.Lines.Quantity = 1
                        holdingSO.Lines.PriceAfterVAT = (100 - (docTotal Mod 100)) '* exchangeRate
                        holdingSO.Lines.AccountCode = accJPI
                        holdingSO.Update()
                    End If
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Added Holding SO Successsfully ", "UpdateHoldingSO()")
                    DocNum = holdingSO.DocNum
                    Return DocNum
                Else
                    holdingCompany.GetLastError(errCode, errMess)
                    Throw New Exception(String.Format("Error : {0} - {1}", errMess, "Fail to Create Sale Order"))
                End If
            End If
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(holdingSO)
            holdingSO = Nothing
        End Try
    End Function
    Private Function CreateHoldingAP(holdingCompany As SAPbobsCOM.Company, branchARInvoice As SAPbobsCOM.Documents, isJPI As Boolean, brachInvCur As String) As String
        Dim holdingAP As SAPbobsCOM.Documents = Nothing
        Dim errCode As Integer
        Dim errMess As String = ""
        Dim sDocNum As String = String.Empty
        Try

            holdingAP = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices)

            CopyDocument(holdingCompany, branchARInvoice, holdingAP, False, Functions.BranchCode, isJPI, False, brachInvCur)
            holdingAP.NumAtCard = branchARInvoice.DocNum.ToString()
            If holdingAP.Add = 0 Then
                Dim docEntry As String = ""
                holdingCompany.GetNewObjectCode(docEntry)
                holdingAP.GetByKey(docEntry)
                sDocNum = holdingAP.DocNum
                Return sDocNum
            Else
                holdingCompany.GetLastError(errCode, errMess)
                Throw New Exception(String.Format("Error : {0} - {1}", errCode, "Failed to create AP Invoice"))
            End If
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(holdingAP)
            holdingAP = Nothing
        End Try
    End Function
    Private Function UpdateHoldingAP(holdingCompany As SAPbobsCOM.Company, branchARInvoice As SAPbobsCOM.Documents, isJPI As Boolean, brachInvCur As String) As String
        Dim holdingAP As SAPbobsCOM.Documents = Nothing
        Dim ors As SAPbobsCOM.Recordset = Nothing
        Dim errCode As Integer
        Dim errMess As String = ""
        Dim DocNum As String = ""

        Try
            ors = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            ors.DoQuery(String.Format("Select DocEntry from OPCH where U_AE_BRARInvNo = '{0}' and U_AE_BRCode = '{1}'", branchARInvoice.DocNum, Functions.BranchCode))
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", "UpdateHoldingAP()")
            holdingAP = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices)
            If ors.RecordCount = 1 Then
                If holdingAP.GetByKey(ors.Fields.Item("DocEntry").Value) Then
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function CopyDocument()", "UpdateHoldingAP()")
                    CopyDocument(holdingCompany, branchARInvoice, holdingAP, True, Functions.BranchCode, isJPI, False, brachInvCur)
                    holdingAP.NumAtCard = branchARInvoice.DocNum.ToString()
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Update Holding AP", "UpdateHoldingAP()")
                    If holdingAP.Update <> 0 Then
                        holdingCompany.GetLastError(errCode, errMess)
                        Throw New Exception(String.Format("Error : {0} - {1}", errCode, "Failed to update AP Invoice"))
                    End If
                End If
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Updated Holding AP Successfully", "UpdateHoldingAP()")
                DocNum = holdingAP.DocNum
                Return DocNum
            ElseIf ors.RecordCount = 0 Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function CopyDocument()", "UpdateHoldingAP()")
                CopyDocument(holdingCompany, branchARInvoice, holdingAP, False, Functions.BranchCode, isJPI, False, brachInvCur)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Add Holding AP", "UpdateHoldingAP()")
                If holdingAP.Add = 0 Then
                    Dim docEntry As String = ""
                    holdingCompany.GetNewObjectCode(docEntry)
                    holdingAP.GetByKey(docEntry)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Added Holding AP Successfully", "UpdateHoldingAP()")
                    DocNum = holdingAP.DocNum
                    Return DocNum
                Else
                    holdingCompany.GetLastError(errCode, errMess)
                    Throw New Exception(String.Format("Error : {0} - {1}", errMess, "Fail to Create AP Invoice"))
                End If
            End If
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(holdingAP)
            holdingAP = Nothing
        End Try
    End Function
    Private Function GetDocumentLine(DocEntry As Integer, TableName As String) As SAPbobsCOM.Recordset
        Dim ors As SAPbobsCOM.Recordset = Nothing
        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", "GetDocumentLine()")
        Try
            ors = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(String.Format("select ItemCode, Dscription, Text, Quantity, PriceBefDi, DiscPrcnt, U_AE_SDateFr, U_AE_SDateTo, U_AE_Indv, U_AE_Hours from {0} where DocEntry = {1}", TableName, DocEntry), "GetDocumentLine()")
            ors.DoQuery(String.Format("select ItemCode, Dscription, Text, Quantity, PriceBefDi, DiscPrcnt, U_AE_SDateFr, U_AE_SDateTo, U_AE_Indv, U_AE_Hours from {0} where DocEntry = {1}", TableName, DocEntry))
            ors.MoveFirst()
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Ors Count  " & ors.RecordCount, "GetDocumentLine()")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed Successfully  ", "GetDocumentLine()")
            Return ors

        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error : " & ex.Message, "GetDocumentLine()")

        Finally
            'If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Finally", "GetDocumentLine()")
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(ors)
            'ors = Nothing
        End Try
    End Function
    Private Sub CopyDocument(ByVal holdingCompany As SAPbobsCOM.Company, ByVal FromDoc As SAPbobsCOM.Documents,
                             ByVal ToDoc As SAPbobsCOM.Documents, ByVal isUpdate As Boolean,
                             ByVal CardCode As String, ByVal isJPI As Boolean, ByVal isBillUSD As Boolean, ByVal fromDocCur As String)
        Dim oRc As SAPbobsCOM.Recordset = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Dim myitemJPI As String() = Nothing
        Dim exchangeRate As Double = 1
        Dim USDRate As Double = 1
        itemJPI = ""

        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", "CopyDocument()")
        Try
            If Not isUpdate Then 'Do not copy CardCode , Document Lines, Contact Persion in Update Mode

                ToDoc.CardCode = CardCode
                If ContactCode <> "" And ToDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders Then
                    ToDoc.ContactPersonCode = ContactCode
                End If
                If ToDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices Then
                    ToDoc.DocCurrency = fromDocCur
                ElseIf ToDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders Then
                    If isBillUSD Then
                        ToDoc.DocCurrency = "USD"
                    End If
                End If

                If fromDocCur <> "SGD" And fromDocCur <> "USD" Then
                    exchangeRate = GetExchangeRate(holdingCompany, fromDocCur, FromDoc.DocDate)
                End If
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function GetExchangeRate()", "CopyDocument()")
                USDRate = GetExchangeRate(holdingCompany, "USD", FromDoc.DocDate)

                ToDoc.Rounding = FromDoc.Rounding

                If FromDoc.RoundingDiffAmountFC <> 0 Then
                    ToDoc.RoundingDiffAmount = FromDoc.RoundingDiffAmountFC
                Else
                    ToDoc.RoundingDiffAmount = FromDoc.RoundingDiffAmountSC
                End If


                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function GetDocumentLine()", "CopyDocument()")
                Dim docLines As SAPbobsCOM.Recordset = GetDocumentLine(FromDoc.DocEntry, "INV1")

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("docLines Record count " & docLines.RecordCount, "CopyDocument()")
                While Not docLines.EoF

                    If Not docLines.BoF Then

                        ToDoc.Lines.Add()
                    End If

                    ToDoc.Lines.ItemCode = docLines.Fields.Item("ItemCode").Value
                    ToDoc.Lines.ItemDescription = docLines.Fields.Item("Dscription").Value
                    ToDoc.Lines.ItemDetails = docLines.Fields.Item("Text").Value
                    ToDoc.Lines.Quantity = docLines.Fields.Item("Quantity").Value
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("5", "CopyDocument()")
                    ToDoc.Lines.UserFields.Fields.Item("U_AE_SDateFr").Value = docLines.Fields.Item("U_AE_SDateFr").Value
                    ToDoc.Lines.UserFields.Fields.Item("U_AE_SDateTo").Value = docLines.Fields.Item("U_AE_SDateTo").Value
                    ToDoc.Lines.UserFields.Fields.Item("U_AE_Indv").Value = docLines.Fields.Item("U_AE_Indv").Value
                    ToDoc.Lines.UserFields.Fields.Item("U_AE_Hours").Value = docLines.Fields.Item("U_AE_Hours").Value

                    If ToDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices Then
                        ToDoc.Lines.UnitPrice = docLines.Fields.Item("PriceBefDi").Value
                    ElseIf ToDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders Then
                        If isBillUSD Then
                            Select Case fromDocCur
                                Case "USD"
                                    ToDoc.Lines.UnitPrice = docLines.Fields.Item("PriceBefDi").Value
                                Case "SGD"
                                    ToDoc.Lines.UnitPrice = docLines.Fields.Item("PriceBefDi").Value / USDRate
                                Case Else
                                    ToDoc.Lines.UnitPrice = docLines.Fields.Item("PriceBefDi").Value * exchangeRate / USDRate
                            End Select
                        Else
                            If fromDocCur <> "USD" Then
                                If fromDocCur = "SGD" Then
                                    ToDoc.Lines.UnitPrice = docLines.Fields.Item("PriceBefDi").Value * exchangeRate
                                Else
                                    ToDoc.Lines.UnitPrice = docLines.Fields.Item("PriceBefDi").Value * (exchangeRate + (exchangeRate * 5 / 100))
                                End If

                            Else
                                ToDoc.Lines.UnitPrice = docLines.Fields.Item("PriceBefDi").Value * (USDRate + (USDRate * 5 / 100))
                            End If
                        End If
                    End If

                    If ToDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders Then
                        ToDoc.Lines.DiscountPercent = If(FromDoc.UserFields.Fields.Item("U_AE_BillType").Value = "MARKUP",
                                                        GetMarkupPercent(ToDoc.Lines.ItemCode, holdingCompany),
                                                        docLines.Fields.Item("DiscPrcnt").Value)

                        If isJPI And itemJPI = "" Then
                            myitemJPI = GetJPIItemCode(ToDoc.Lines.ItemCode, holdingCompany)
                            itemJPI = myitemJPI.GetValue(0).ToString()
                            accJPI = myitemJPI.GetValue(1).ToString()
                        End If
                    Else
                        ToDoc.Lines.DiscountPercent = docLines.Fields.Item("DiscPrcnt").Value
                    End If
                    docLines.MoveNext()
                End While

                Dim VATSum As Double = 0

                If fromDocCur = "USD" Then
                    If (FromDoc.VatSumFc) > 0 Then
                        VATSum = FromDoc.VatSumFc
                    Else
                        VATSum = FromDoc.VatSumSys
                    End If
                Else
                    VATSum = FromDoc.VatSum
                End If

                If VATSum > 0 Then
                    ToDoc.Lines.Add()
                    ToDoc.Lines.ItemCode = "ITM199"
                    ToDoc.Lines.Quantity = 1
                    If ToDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices Then
                        ToDoc.Lines.UnitPrice = VATSum
                    ElseIf ToDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders Then
                        If isBillUSD Then
                            Select Case fromDocCur
                                Case "USD"
                                    ToDoc.Lines.UnitPrice = VATSum
                                Case "SGD"
                                    ToDoc.Lines.UnitPrice = VATSum / USDRate
                                Case Else
                                    ToDoc.Lines.UnitPrice = VATSum * exchangeRate / USDRate
                            End Select
                        Else
                            If fromDocCur <> "USD" Then
                                If fromDocCur = "SGD" Then
                                    ToDoc.Lines.UnitPrice = VATSum * exchangeRate
                                Else
                                    ToDoc.Lines.UnitPrice = VATSum * (exchangeRate + (exchangeRate * 5 / 100))
                                End If
                            Else
                                ToDoc.Lines.UnitPrice = VATSum * (USDRate + (USDRate * 5 / 100))
                            End If
                        End If
                    End If
                End If

                ToDoc.DocDate = FromDoc.DocDate
                ToDoc.DocDueDate = FromDoc.DocDueDate
                ToDoc.TaxDate = FromDoc.TaxDate
                ToDoc.DocType = FromDoc.DocType
                ToDoc.DiscountPercent = FromDoc.DiscountPercent


            End If


            ToDoc.NumAtCard = FromDoc.NumAtCard
            ToDoc.Comments = FromDoc.Comments

            ToDoc.UserFields.Fields.Item("U_AE_BRARInvNo").Value = FromDoc.DocNum.ToString()


            oRc.DoQuery(String.Format("SELECT COLUMN_NAME " +
                                        "FROM [{0}].INFORMATION_SCHEMA.COLUMNS " +
                                        "WHERE " +
                                        "   TABLE_NAME = 'OPCH' " +
                                        "   and COLUMN_NAME like 'U_AE%' " +
                                        "   and COLUMN_NAME not in( 'U_AE_BRARInvNo','U_AE_BRARCNNo','U_AE_HDAPInvNo', " +
                                        "   'U_AE_HDSONo','U_AE_HDAPCNNo','U_AE_HDARCNNo','U_AE_BRCode')", holdingCompany.CompanyDB))
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("10", "CopyDocument()")
            oRc.MoveFirst()
            If oRc.RecordCount Then
                While Not oRc.EoF
                    ToDoc.UserFields.Fields.Item(oRc.Fields.Item(0).Value).Value = FromDoc.UserFields.Fields.Item(oRc.Fields.Item(0).Value).Value
                    oRc.MoveNext()
                End While
            End If




            ToDoc.UserFields.Fields.Item("U_AE_BRCode").Value = Functions.BranchCode
        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error  " & ex.Message, "GetDocumentLine()")
        End Try
       
    End Sub
 
    Private Function GetMarkupPercent(ItemCode As String, holdingCompany As SAPbobsCOM.Company) As Double
        Dim markupPercent As Double = 0
        Dim oItem As SAPbobsCOM.Items = Nothing
        Dim oRS As SAPbobsCOM.Recordset = Nothing
        Try
            oRS = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oItem = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            If oItem.GetByKey(ItemCode) Then
                If oItem.UserFields.Fields.Item("U_AE_ItemMarkUp").Value = "Y" Then
                    oRS.DoQuery(String.Format("select U_AE_BrMarkUp from [@MARKUP_PERCENT] where U_AE_BrCode = '{0}'", Functions.BranchCode))
                    If oRS.RecordCount > 0 Then
                        oRS.MoveFirst()
                        Double.TryParse(oRS.Fields.Item("U_AE_BrMarkUp").Value, markupPercent)
                    End If
                End If
            End If
            GetMarkupPercent = markupPercent * (-1)
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oItem)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS)
            oItem = Nothing
            oRS = Nothing
        End Try
    End Function
    Private Function GetJPIItemCode(ItemCode As String, holdingCompany As SAPbobsCOM.Company) As String()
        Dim jpiItem(0 To 1) As String
        Dim code As String = ""
        Dim glAccount As String = ""
        Dim oItem As SAPbobsCOM.Items = Nothing

        Try
            oItem = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            If oItem.GetByKey(ItemCode) Then
                If oItem.UserFields.Fields.Item("U_AE_JPIItem").Value = "Y" Then
                    code = oItem.ItemCode
                    glAccount = oItem.WhsInfo.WipAccount
                End If
            End If
            jpiItem.SetValue(code, 0)
            jpiItem.SetValue(glAccount, 1)
            Return jpiItem
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oItem)
            oItem = Nothing
        End Try
    End Function
    Private Function CreateDocumentForHoldings(BPCode As String) As Boolean
        Dim isCreate As Boolean = False
        Dim oBP As SAPbobsCOM.BusinessPartners = Nothing
        Dim oGroupBP As SAPbobsCOM.BusinessPartnerGroups = Nothing
        Try
            oBP = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
            oGroupBP = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartnerGroups)
            If oBP.GetByKey(BPCode) Then
                If oGroupBP.GetByKey(oBP.GroupCode) Then
                    If oGroupBP.Name = "Holdings" Then
                        isCreate = True
                    End If
                End If
            End If
            Return isCreate
        Catch ex As Exception
            SBO_Application.SetStatusBarMessage(ex.Message)
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oGroupBP)
            oBP = Nothing
            oGroupBP = Nothing
            ''GC.Collect()
        End Try
    End Function
End Class
