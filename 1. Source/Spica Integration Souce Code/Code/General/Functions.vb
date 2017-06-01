Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.ServiceProcess
Imports Microsoft.Reporting.WinForms
Imports System.Net.Mail
Imports System.IO.Packaging
Imports System
Imports System.Text
Imports System.Web.Services.Protocols
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Net

Public Class Functions
    
    
    Public Shared Sub WriteLog(ByVal Str As String)
        Dim oWrite As IO.StreamWriter
        Dim FilePath As String
        FilePath = Application.StartupPath + "\logfile.txt"

        If IO.File.Exists(FilePath) Then
            oWrite = IO.File.AppendText(FilePath)
        Else
            oWrite = IO.File.CreateText(FilePath)
        End If
        oWrite.Write(Now.ToString() + ":" + Str + vbCrLf)
        oWrite.Close()
    End Sub
    
    Public Sub AutoRun()
        Try

            SystemInitial()
            '---------NightAuditHandler----------
            Dim NAH As New NightAuditHandler
            NAH.CreateDocumentForAudit()

        Catch ex As Exception
            WriteLog(ex.ToString)
        End Try
    End Sub
    Public Shared Function SystemInitial() As String
        Try
            PublicVariable.AutoRetry = CBool(System.Configuration.ConfigurationSettings.AppSettings.Get("AutoRetry"))
            PublicVariable.IntegrationConnectionString = System.Configuration.ConfigurationSettings.AppSettings.Get("IntegrationConnectionString")
            PublicVariable.SAPConnectionString = System.Configuration.ConfigurationSettings.AppSettings.Get("SAPConnectionString")
            PublicVariable.Timer = System.Configuration.ConfigurationSettings.AppSettings.Get("Timer")
            PublicVariable.ToErrorEmail = System.Configuration.ConfigurationSettings.AppSettings.Get("ErrorReceiver")

            Dim smtp As Array = System.Configuration.ConfigurationSettings.AppSettings.Get("SMTPServer").Split(";")
            PublicVariable.smtpServer = smtp(0)
            PublicVariable.smtpSenderEmail = smtp(1)
            PublicVariable.smtpPwd = smtp(2)
            PublicVariable.smtpPort = smtp(3)

            Return ""
        Catch ex As Exception
            WriteLog("SystemInitial: " + ex.ToString)
            Return ex.ToString
        End Try

    End Function

    Public Shared Function SAPConnection(ByVal CompCode As String) As String
        Try

            Dim cn As New Connection
            Dim CompanyDB As String = ""
            Dim UserName As String = ""
            Dim Password As String = ""
            Dim SrName As String = ""
            Dim dt As DataTable = New DataTable
            dt = cn.SAP_RunQuery("SELECT Top 1 T0.[U_AB_DB], T0.[U_AB_UserID], T0.[U_AB_Password],T0.[U_AB_Series] FROM [dbo].[@COMPANYSETUP]  T0 WHERE T0.[U_AB_CompanyCode] ='" & CompCode & "'")
            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    CompanyDB = row("U_AB_DB")
                    UserName = row("U_AB_UserID")
                    Password = row("U_AB_Password")
                    SrName = row("U_AB_Series")
                Next
            End If
            PublicVariable.Series = cn.SAP_RunQuery_SingleValue("select Top 1 Series from NNM1 where ObjectCode='17' and SeriesName='" & SrName & "'")

            If Not PublicVariable.oCompanyInfo.Connected Then
                Dim MyArr As Array
                MyArr = PublicVariable.SAPConnectionString.Split(";")

                PublicVariable.oCompanyInfo.CompanyDB = CompanyDB 'MyArr(0).ToString()
                PublicVariable.oCompanyInfo.UserName = UserName 'MyArr(1).ToString()
                PublicVariable.oCompanyInfo.Password = Password 'MyArr(2).ToString()
                PublicVariable.oCompanyInfo.Server = MyArr(3).ToString()
                PublicVariable.oCompanyInfo.DbUserName = MyArr(4).ToString()
                PublicVariable.oCompanyInfo.DbPassword = MyArr(5).ToString()
                PublicVariable.oCompanyInfo.LicenseServer = MyArr(6)
                If MyArr(7).ToString = "2008" Then
                    PublicVariable.oCompanyInfo.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
                ElseIf MyArr(7).ToString = "2012" Then
                    PublicVariable.oCompanyInfo.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
                Else
                    PublicVariable.oCompanyInfo.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005
                End If

                Dim lRetCode As Integer
                Dim lErrCode As Integer
                Dim sErrMsg As String = ""
                lRetCode = PublicVariable.oCompanyInfo.Connect
                If lRetCode <> 0 Then
                    PublicVariable.oCompanyInfo.GetLastError(lErrCode, sErrMsg)
                    Functions.WriteLog("SystemInitial:" + sErrMsg)
                    Return sErrMsg
                Else
                    Return ""
                End If
            Else
                Return "Company Not Connected!"
            End If


        Catch ex As Exception
            WriteLog("SystemInitial: " + ex.ToString)
            Return ex.ToString
        End Try

    End Function
    Public Shared Function GetPaymentType(PaymentMethodCode As String) As String
        Dim strQuery As String = ""
        Dim cn As New Connection
        strQuery = "select U_PaymentType from [@PAYMENTMETHOD] where Code='" + PaymentMethodCode + "'"
        Dim dt As DataTable = cn.SAP_RunQuery(strQuery)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("U_PaymentType")
        Else
            Return ""
        End If
    End Function
    Public Shared Function CheckBPCode(CompanyCode As String) As String

        If CompanyCode = "" Then
            Return "Company Code is missing"
        End If


        Dim strQuery As String = ""
        Dim cn As New Connection
        strQuery = "select * from OCRD where CardCode='" + CompanyCode + "'"
        Dim dt As DataTable = cn.SAP_RunQuery(strQuery)
        If dt.Rows.Count > 0 Then
            Return ""
        Else
            Return "Company Code is missing"
        End If

    End Function
    Public Shared Function GetCostCenterByItem(ItemCode As String) As String
        Dim strQuery As String = ""
        Dim cn As New Connection
        strQuery = "select isnull(U_CostCenter,'') U_CostCenter from OITM where ItemCode='" + ItemCode + "'"
        Dim dt As DataTable = cn.SAP_RunQuery(strQuery)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("U_CostCenter").ToString
        Else
            Return ""
        End If
    End Function
    Public Shared Function GetOneTimeCustomerCode(CardCode As String) As String
        If CardCode <> "" Then
            Return CardCode
        End If
        Dim cn As New Connection
        Dim strQuery As String = ""
        strQuery = "select top(1) dfltcard CardCode from oacp"
        Dim dt As DataTable = cn.SAP_RunQuery(strQuery)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("CardCode").ToString
        Else
            Return ""
        End If
    End Function
#Region "Build Table Structure"
    ''' <param name="tableName">table name , example : T </param>
    ''' <param name="fieldName">list of fields, example: A;B;C;D.</param>
    Public Shared Function BuildTable(tableName As String, fieldName As String) As DataTable
        Dim dt As New DataTable(tableName)
        If fieldName <> "" Then
            Dim arrFieldName As Array = fieldName.Split(";")
            For i As Integer = 0 To arrFieldName.Length - 1
                dt.Columns.Add(arrFieldName(i).ToString)
            Next
        End If
        Return dt
    End Function
#End Region
End Class
