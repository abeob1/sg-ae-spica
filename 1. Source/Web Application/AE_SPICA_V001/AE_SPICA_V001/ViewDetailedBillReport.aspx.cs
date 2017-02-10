using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace AE_SPICA_V001
{
    public partial class ViewDetailedBillReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string sFileRef = Convert.ToString(Request.Cookies[Constants.FileRefNo].Value);
            string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            string[] dbInfo = connection.Split(';');
            string strServer = dbInfo[0].Split('=')[1];//ConfigurationSettings.AppSettings["SQLserver"].ToString();
            string strDBName = dbInfo[1].Split('=')[1];//ConfigurationSettings.AppSettings["SQLdatabaseName"].ToString();
            string strUID = dbInfo[2].Split('=')[1];//ConfigurationSettings.AppSettings["SQLUserName"].ToString();
            string strPassword = dbInfo[3].Split('=')[1];//ConfigurationSettings.AppSettings["SQLPassword"].ToString();

            ReportDocument myReportDocument;
            myReportDocument = new ReportDocument();
            // myReportDocument.Load(Server.MapPath("~/Reports/") + "AB_Billing_Report_Approved_Bill.rpt");
            myReportDocument.Load(Server.MapPath("~/Reports/") + "AB_Billing_Report_Details.rpt");
            myReportDocument.SetDatabaseLogon(strUID, strPassword, strServer, strDBName);

            //string sFromDate = Convert.ToString(Request.Cookies[Constants.ABRFromDate].Value);
            //string sToDate = Convert.ToString(Request.Cookies[Constants.ABRToDate].Value);

            //CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
            //CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
            //ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
            //pdisval1.Value = sFromDate;
            //pval1.Add(pdisval1);
            //ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
            //pdisval2.Value = sToDate;
            //pval2.Add(pdisval2);

            //myReportDocument.DataDefinition.ParameterFields["@fromdate"].ApplyCurrentValues(pval1);
            //myReportDocument.DataDefinition.ParameterFields["@todate"].ApplyCurrentValues(pval2);

            CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval3 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval4 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval5 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval6 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval7 = new ParameterValues();

            ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
            pdisval1.Value = Convert.ToString(Request.Cookies[Constants.DBRFromDate].Value);
            pval1.Add(pdisval1);

            ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
            pdisval2.Value = Convert.ToString(Request.Cookies[Constants.DBRToDate].Value);
            pval2.Add(pdisval2);

            ParameterDiscreteValue pdisval3 = new ParameterDiscreteValue();
            pdisval3.Value = Convert.ToString(Request.Cookies[Constants.DBRFromClaim].Value);
            pval3.Add(pdisval3);

            ParameterDiscreteValue pdisval4 = new ParameterDiscreteValue();
            pdisval4.Value = Convert.ToString(Request.Cookies[Constants.DBRToClaim].Value);
            pval4.Add(pdisval4);

            ParameterDiscreteValue pdisval5 = new ParameterDiscreteValue();
            pdisval5.Value = Convert.ToString(Request.Cookies[Constants.DBRFromComp].Value);
            pval5.Add(pdisval5);

            ParameterDiscreteValue pdisval6 = new ParameterDiscreteValue();
            pdisval6.Value = Convert.ToString(Request.Cookies[Constants.DBRToComp].Value);
            pval6.Add(pdisval6);

            ParameterDiscreteValue pdisval7 = new ParameterDiscreteValue();
            pdisval7.Value = Convert.ToString(Request.Cookies[Constants.DBRStatus].Value);
            pval7.Add(pdisval7);

            myReportDocument.DataDefinition.ParameterFields["@fromdate"].ApplyCurrentValues(pval1);
            myReportDocument.DataDefinition.ParameterFields["@todate"].ApplyCurrentValues(pval2);
            myReportDocument.DataDefinition.ParameterFields["fromCH@select ClaimHandler from tbl_FileReference"].ApplyCurrentValues(pval3);
            myReportDocument.DataDefinition.ParameterFields["ToCH@select ClaimHandler from tbl_FileReference"].ApplyCurrentValues(pval4);
            myReportDocument.DataDefinition.ParameterFields["FromCompany@select Name from tbl_CompanyData"].ApplyCurrentValues(pval5);
            myReportDocument.DataDefinition.ParameterFields["ToCompany@select Name from tbl_CompanyData"].ApplyCurrentValues(pval6);
            myReportDocument.DataDefinition.ParameterFields["Status@select ApprovalStatus from tbl_FileReference"].ApplyCurrentValues(pval7);

            CrystalReportViewer1.ReportSource = myReportDocument;
        }
    }
}