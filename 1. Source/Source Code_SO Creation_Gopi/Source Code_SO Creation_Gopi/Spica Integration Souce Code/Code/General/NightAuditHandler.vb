Public Class NightAuditHandler
    Public Sub CreateDocumentForAudit()
        
        Try

            Dim oSO As SalesOrder = New SalesOrder
            Dim cn As New Connection
            Dim dt As DataTable = New DataTable
            Dim dt1 As DataTable = New DataTable
            Dim errMessage As String = ""
            Dim SONo As String = ""
            Dim CompCode As String = ""
            Dim sqlStr As String = "Select isnull(T0.Lumpsum,'') Lumpsum ,isnull(T0.Status,'') [Status],T1.[LastApprovedMonth],T2.[LastApprovedMonth] LastApprovedMonth1,T1.[ApprovedDate] ApprovedDate1,T0.Companycode,CONVERT(varchar(23)," & _
            "T1.[ApprovedDate],121) [ApprovedDate],T0.[FileReferenceNo],isnull(T0.ClubReference,'') ClubReference ," & _
            "Isnull(T0.IncidentPlace,'')IncidentPlace,Isnull(T0.Vessel,'') Vessel,Isnull(T0.Member,'') Member," & _
    "isnull(T0.MemberAdd1,'') MemberAdd1,isnull(T0.MemberAdd2,'') MemberAdd2,isnull(T0.MemberAdd3,'') MemberAdd3," & _
    "isnull(T0.MemberAdd4,'') MemberAdd4,isnull(T0.CO,'') CO,isnull(T0.ClubBP,'') ClubBP,isnull(T0.ClubBPContact,'')" & _
    "ClubBPContact,isnull(T0.[Description],'')[Description],isnull(T0.PeriodDateFrom,GETDATE()) PeriodDateFrom," & _
    "isnull(T0.PeriodDateTo,GETDATE()) PeriodDateTo,isnull(T0.IncidentDate,getdate()) IncidentDate," & _
    "isnull(T0.VoyageNumber,'') VoyageNumber,T0.BillingDetails " & _
    "FROM [dbo].[tbl_FileReference] T0 " & _
    "Left Join   [dbo].[tbl_TimeEntry] T1 on T0.[FileReferenceNo]=T1.[FileReference]" & _
    "Left Join   [dbo].[tbl_ExpenseEntry] T2 on T0.[FileReferenceNo]=T2.[FileReference]" & _
    "where (T1.ApprovalStatus='Approved' and T1.[SONumber] is null and T1.[LastApprovedMonth] is not null) or (T2.ApprovalStatus='Approved' and T2.[SONumber] is null and T2.[LastApprovedMonth] is not null)" & _
    "order by [ApprovedDate] desc"

            'dt = cn.Integration_RunQuery("Select T0.ErrorMessage,T0.[ApprovedDate] ApprovedDate1,T0.Companycode,CONVERT(varchar(23),T0.[ApprovedDate],121) [ApprovedDate],T0.[FileReferenceNo],isnull(T0.ClubReference,'') ClubReference ,Isnull(T0.IncidentPlace,'')IncidentPlace,Isnull(T0.Vessel,'') Vessel,Isnull(T0.Member,'') Member,isnull(T0.MemberAdd1,'') MemberAdd1,isnull(T0.MemberAdd2,'') MemberAdd2,isnull(T0.MemberAdd3,'') MemberAdd3,isnull(T0.MemberAdd4,'') MemberAdd4,isnull(T0.CO,'') CO,isnull(T0.ClubBP,'') ClubBP,isnull(T0.ClubBPContact,'') ClubBPContact,isnull(T0.[Description],'')[Description],isnull(T0.PeriodDateFrom,GETDATE()) PeriodDateFrom,isnull(T0.PeriodDateTo,GETDATE()) PeriodDateTo,isnull(T0.IncidentDate,getdate()) IncidentDate,isnull(T0.VoyageNumber,'') VoyageNumber FROM [dbo].[tbl_FileReference] T0 where T0.ApprovalStatus='Approved' and T0.[SONumber] is null and ApprovedDate is not null order by [ApprovedDate] desc")
            dt = cn.Integration_RunQuery(sqlStr)

            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    Dim FileRefNo As String = row("FileReferenceNo")
                    Dim ApprovedDate As String = row("LastApprovedMonth").ToString
                    If ApprovedDate <> "" Then
                        Dim Companycode As String = row("Companycode")
                        Try
                            CompCode = row("Companycode")
                            Dim sErrMsg As String
                            sErrMsg = Functions.SAPConnection(CompCode)
                            If sErrMsg <> "" Then
                                Throw New Exception(sErrMsg)
                            End If
                            errMessage = oSO.CreateSO(row, SONo, ApprovedDate)
                            If errMessage <> "" Then
                                Throw New Exception(errMessage)
                            End If
                            UpdateFileReference(FileRefNo, ApprovedDate, Companycode, SONo, "")
                        Catch ex As Exception
                            UpdateFileReference(FileRefNo, ApprovedDate, Companycode, SONo, ex.Message)
                        End Try
                    End If
                Next
            End If

            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    Dim FileRefNo As String = row("FileReferenceNo")
                    Dim ApprovedDate As String = row("LastApprovedMonth1").ToString
                    If ApprovedDate <> "" Then
                        Dim Companycode As String = row("Companycode")
                        Try
                            CompCode = row("Companycode")
                            Dim sErrMsg As String
                            sErrMsg = Functions.SAPConnection(CompCode)
                            If sErrMsg <> "" Then
                                Throw New Exception(sErrMsg)
                            End If
                            errMessage = oSO.CreateSO(row, SONo, ApprovedDate)
                            If errMessage <> "" Then
                                Throw New Exception(errMessage)
                            End If
                            UpdateFileReference(FileRefNo, ApprovedDate, Companycode, SONo, "")
                        Catch ex As Exception
                            UpdateFileReference(FileRefNo, ApprovedDate, Companycode, SONo, ex.Message)
                        End Try
                    End If
                Next
            End If

        Catch ex As Exception
            Functions.WriteLog(ex.Message.ToString)
        End Try
        PublicVariable.oCompanyInfo.Disconnect()
    End Sub
  
    Private Sub UpdateFileReference(ByVal FileReferenceNo As String, ByVal LastApprovedMonth As String, ByVal Companycode As String, ByVal SONumber As String, ByVal mess As String)
        Dim cn As New Connection
        If mess = "" Then
            cn.Integration_RunQuery("Update [tbl_TimeEntry] set SOCreatedDate = GETDATE() , ErrorMessage = '', SONumber='" & SONumber & "' where FileReference ='" & FileReferenceNo & "'  and CompanyCode='" & Companycode & "' and SONumber is null and [LastApprovedMonth]  is not null and ApprovalStatus='Approved' and LastApprovedMonth='" & LastApprovedMonth & "'")
            cn.Integration_RunQuery("Update [tbl_ExpenseEntry] set SOCreatedDate = GETDATE() , ErrorMessage = '', SONumber='" & SONumber & "' where FileReference ='" & FileReferenceNo & "'  and CompanyCode='" & Companycode & "' and SONumber is null and [LastApprovedMonth]  is not null and ApprovalStatus='Approved' and LastApprovedMonth='" & LastApprovedMonth & "'")
        Else
            cn.Integration_RunQuery("Update [tbl_TimeEntry] set ErrorMessage = '" & mess & "' where FileReference ='" & FileReferenceNo & "'  and CompanyCode='" & Companycode & "' and SONumber is null and [LastApprovedMonth]  is not null and ApprovalStatus='Approved' and LastApprovedMonth='" & LastApprovedMonth & "'")
            cn.Integration_RunQuery("Update [tbl_ExpenseEntry] set ErrorMessage = '" & mess & "'  where FileReference ='" & FileReferenceNo & "'  and CompanyCode='" & Companycode & "' and SONumber is null and [LastApprovedMonth]  is not null and ApprovalStatus='Approved' and LastApprovedMonth='" & LastApprovedMonth & "'")
        End If
    End Sub
End Class
