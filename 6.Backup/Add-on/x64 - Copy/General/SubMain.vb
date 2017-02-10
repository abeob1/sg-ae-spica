Option Explicit On
Option Strict Off
Imports System.Windows.Forms
'Imports System.ServiceProcess

Module SubMain


    Public p_iDebugMode As Int16
    Public p_iErrDispMethod As Int16
    Public p_iDeleteDebugLog As Int16
    Public Const RTN_SUCCESS As Int16 = 1
    Public Const RTN_ERROR As Int16 = 0

    Public Const DEBUG_ON As Int16 = 1
    Public Const DEBUG_OFF As Int16 = 0



    Public Sub Main()

        Dim Args As String() = Environment.GetCommandLineArgs()
        p_iDebugMode = DEBUG_ON
        Dim obj As New Add_on
        Application.Run()


    End Sub

    Public Function GetExchangeRate(holdingCompany As SAPbobsCOM.Company, Currency As String, CurDate As Date) As Double
        Dim vObj As SAPbobsCOM.SBObob = Nothing
        Dim rs As SAPbobsCOM.Recordset = Nothing
        Try
            Dim Result As Double = 1
            vObj = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            rs = holdingCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            rs = vObj.GetCurrencyRate(Currency, CurDate)
            Result = rs.Fields.Item(0).Value
            Return Result
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            vObj = Nothing
            rs = Nothing
        End Try
    End Function
End Module
