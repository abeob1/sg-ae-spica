Imports System.Globalization

Public Class SalesOrder
    Dim errMessage As String = ""

    Public Function CreateSO(ByVal SOheader As DataRow, ByRef SONo As String, ByRef ApprovedDate As String) As String
        Try
            Dim cn As New Connection
            Dim SODt As DataTable = New DataTable
            Dim errMessage As String = ""
            Dim RetCode As Integer = 0
            Dim FileRefNo As String = SOheader("FileReferenceNo")
            'Dim ApprovedDate As String = SOheader("ApprovedDate").ToString
            Dim Companycode As String = SOheader("Companycode")
            Dim oSO As SAPbobsCOM.Documents
            oSO = PublicVariable.oCompanyInfo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim SPStr As String = "exec AB_SalesOrder '" & FileRefNo & "','" & Companycode & "','" & ApprovedDate & "'"
            SODt = cn.Integration_RunQuery(SPStr)

            Dim Type As String = ""
            Dim ItemCode As String = ""
            Dim Description As String = ""
            Dim CardCode As String = ""
            Dim CardName As String = ""
            Dim Address As String = ""
            Dim SqlStr As String = ""
            If SODt Is Nothing Then
                Throw New Exception("No data found!")
            End If

            If SODt.Rows.Count > 0 Then
                CardCode = cn.SAP_RunQuery_SingleValue("SELECT Top 1 T0.[U_AB_CardCode] FROM [dbo].[@COMPANYSETUP]  T0 WHERE T0.[U_AB_CompanyCode] ='" & Companycode & "' ")
                oSO.CardCode = CardCode
                If CardCode = "" Then
                    Throw New Exception("CardCode not defined")
                End If

                CardName = cn.SAP_RunQuery_SingleValue("select CardName from OCRD where CardCode ='" & SOheader("ClubBP").ToString.Trim & "' ")
                oSO.UserFields.Fields.Item("U_AE_ClubName").Value = CardName.Trim
                If SOheader("Lumpsum") = "Y" Then

                    oSO.UserFields.Fields.Item("U_AE_BillType").Value = "LUMPSUM"
                Else

                    oSO.UserFields.Fields.Item("U_AE_BillType").Value = "MARKUP"
                End If


                Dim Country As String = ""
                SqlStr = "select top (1) CRD1.Country from OCRD left join CRD1 on OCRD.CardCode=CRD1.CardCode" & _
" left outer join OCRY on CRD1.Country=OCRY.Code" & _
" where CRD1.CardCode= '" & SOheader("ClubBP").ToString.Trim & "' and CRD1.AdresType='B'"
                Country = cn.SAP_RunQuery_SingleValue(SqlStr)

                If Country <> "SG" Then
                    SqlStr = "select top (1) isnull(CRD1.Street,'') + CHAR(13) + isnull(CRD1.StreetNo,'') + CHAR(13) + isnull(CRD1.Block,'') + CHAR(13) + " & _
" isnull(CRD1.City,'')+ ' ' + isnull(CRD1.ZipCode,'') + ' ' +isnull(OCRY.Name,'') as 'Address' " & _
"from OCRD left join CRD1 on OCRD.CardCode=CRD1.CardCode " & _
"left outer join OCRY on CRD1.Country=OCRY.Code " & _
"where CRD1.CardCode='" & SOheader("ClubBP").ToString.Trim & "' and CRD1.AdresType='B'"
                Else
                    SqlStr = "select top (1) isnull(CRD1.Street,'') + CHAR(13) + isnull(CRD1.StreetNo,'') + CHAR(13) + " & _
" isnull(OCRY.Name,'') + ' ' + isnull(CRD1.ZipCode,'') as 'Address', OCRD.CardName " & _
"from OCRD left join CRD1 on OCRD.CardCode=CRD1.CardCode " & _
"left outer join OCRY on CRD1.Country=OCRY.Code " & _
"where CRD1.CardCode='" & SOheader("ClubBP").ToString.Trim & "' and CRD1.AdresType='B'"
                End If
                '                SqlStr = "select top (1) isnull(CRD1.Street,'') + ' ' + isnull(CRD1.StreetNo,'') + ' ' + isnull(CRD1.Block,'') + ' ' + isnull(CRD1.City,'')" & _
                '"+ ' ' + isnull(CRD1.ZipCode,'') + ' ' +isnull(OCRY.Name,'') as 'Address'" & _
                '" from OCRD left join CRD1 on OCRD.CardCode=CRD1.CardCode" & _
                '" left outer join OCRY on CRD1.Country=OCRY.Code" & _
                '" where CRD1.CardCode= '" & SOheader("ClubBP").ToString.Trim & "' and CRD1.AdresType='B'"
                '+ CHAR(13) +
                Address = cn.SAP_RunQuery_SingleValue(SqlStr)


                oSO.UserFields.Fields.Item("U_AE_ClubAddress").Value = Address.ToString.Trim

                oSO.Series = PublicVariable.Series
                oSO.UserFields.Fields.Item("U_AE_YourRef").Value = SOheader("ClubReference")
                If SOheader("Status") = "FINAL" Then
                    oSO.UserFields.Fields.Item("U_AE_FinalInv").Value = "Y"
                Else
                    oSO.UserFields.Fields.Item("U_AE_FinalInv").Value = "N"
                End If
                oSO.UserFields.Fields.Item("U_AE_OutRef").Value = SOheader("FileReferenceNo")
                oSO.UserFields.Fields.Item("U_AE_PIncident").Value = SOheader("IncidentPlace")
                oSO.UserFields.Fields.Item("U_AE_VesselName").Value = SOheader("Vessel")
                oSO.UserFields.Fields.Item("U_AE_Member").Value = SOheader("Member")
                oSO.UserFields.Fields.Item("U_AE_MAdd1").Value = SOheader("MemberAdd1")
                oSO.UserFields.Fields.Item("U_AE_MAdd2").Value = SOheader("MemberAdd2")
                oSO.UserFields.Fields.Item("U_AE_MAdd3").Value = SOheader("MemberAdd3")
                oSO.UserFields.Fields.Item("U_AE_MAdd4").Value = SOheader("MemberAdd4")
                oSO.UserFields.Fields.Item("U_AE_CareOf").Value = "C/O " & SOheader("CO").ToString.Trim

                oSO.UserFields.Fields.Item("U_AE_ClubCode").Value = SOheader("ClubBP")
                oSO.UserFields.Fields.Item("U_AE_ClubContactPer").Value = SOheader("ClubBPContact")
                oSO.UserFields.Fields.Item("U_AE_Case").Value = SOheader("Description")
                oSO.UserFields.Fields.Item("U_AE_PDateFr").Value = SOheader("PeriodDateFrom")
                oSO.UserFields.Fields.Item("U_AE_PDateTo").Value = SOheader("PeriodDateTo")
                oSO.UserFields.Fields.Item("U_AE_DIncident").Value = SOheader("IncidentDate")
                oSO.UserFields.Fields.Item("U_AE_VoyageNo").Value = SOheader("VoyageNumber")
                Dim dur As Double = 0
                Dim hr As Double = 0
                For Each row As DataRow In SODt.Rows
                    Type = row("Type")
                    Description = row("Description")
                    Description = Description.Replace("'", "''")
                    If Type = "TE" Then
                        ItemCode = cn.SAP_RunQuery_SingleValue("SELECT T0.[U_AB_ItemCode] FROM [dbo].[@ITEMMAPPING]  T0 WHERE T0.[U_AB_Type] ='TE' and  T0.[U_AB_CompanyCode] ='" & Companycode & "'")
                    ElseIf Type = "EE" Then
                        ItemCode = cn.SAP_RunQuery_SingleValue("SELECT T0.[U_AB_ItemCode] FROM [dbo].[@ITEMMAPPING]  T0 WHERE T0.[U_AB_Type] ='EE' and  T0.[U_AB_CompanyCode] ='" & Companycode & "' and T0.[U_AB_ItemName]='" & Description & "'")
                    End If
                    If ItemCode = "" Then
                        Throw New Exception("ItemCode not defined")
                    End If
                    oSO.Lines.ItemCode = ItemCode
                    oSO.Lines.Quantity = row("Qty")
                    oSO.Lines.UnitPrice = row("UnitPrice")
                    If Type = "TE" Then
                        oSO.Lines.ItemDetails = SOheader("BillingDetails")
                        dur = (row("Duration"))
                        hr = row("HourlyRate")
                        oSO.Lines.UserFields.Fields.Item("U_AE_Hours").Value = dur  '(row("Duration"))
                        oSO.Lines.UserFields.Fields.Item("U_AE_HourlyRate").Value = hr ' row("HourlyRate")
                    ElseIf Type = "EE" Then
                        oSO.Lines.ItemDetails = row("Remarks")
                    End If

                    oSO.Lines.Add()
                    oSO.DocDate = row("ApprovedDate") 'Now.Date
                    oSO.DocDueDate = row("ApprovedDate") 'Now.Date'
                    oSO.TaxDate = row("ApprovedDate") 'Now.Date
                Next

                'oSO.Add()
                RetCode = oSO.Add
                If RetCode <> 0 Then
                    PublicVariable.oCompanyInfo.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompanyInfo.GetNewObjectKey()
                End If
            Else
                Throw New Exception("Line Details not found!")
            End If
            SODt.Dispose()
            SODt = New DataTable
            PublicVariable.oCompanyInfo.Disconnect()
            Return errMessage
        Catch ex As Exception
            PublicVariable.oCompanyInfo.Disconnect()
            Return ex.Message
        End Try
    End Function
End Class
