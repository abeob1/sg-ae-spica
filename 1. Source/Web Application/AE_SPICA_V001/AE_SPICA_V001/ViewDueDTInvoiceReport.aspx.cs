using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace AE_SPICA_V001
{
    public partial class ViewDueDTInvoiceReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            string[] dbInfo = connection.Split(';');
            string strServer = dbInfo[0].Split('=')[1];//ConfigurationSettings.AppSettings["SQLserver"].ToString();
            string strDBName = dbInfo[1].Split('=')[1];//ConfigurationSettings.AppSettings["SQLdatabaseName"].ToString();
            string strUID = dbInfo[2].Split('=')[1];//ConfigurationSettings.AppSettings["SQLUserName"].ToString();
            string strPassword = dbInfo[3].Split('=')[1];//ConfigurationSettings.AppSettings["SQLPassword"].ToString();

            ReportDocument myReportDocument;
            myReportDocument = new ReportDocument();
            myReportDocument.Load(Server.MapPath("~/Reports/") + "AB_Due_DT_Invoice.rpt");
            myReportDocument.SetDatabaseLogon(strUID, strPassword, strServer, strDBName);

            string sFromDate = Convert.ToString(Request.Cookies[Constants.DDIRFromDate].Value);
            string sToDate = Convert.ToString(Request.Cookies[Constants.DDIRToDate].Value);
            string sFromCompany = Convert.ToString(Request.Cookies[Constants.DDIRFromCompany].Value);
            string sToCompany = Convert.ToString(Request.Cookies[Constants.DDIRToCompany].Value);
            string sUserCode = Convert.ToString(Response.Cookies[Constants.UserCode].Value);

            CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval3 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval4 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval5 = new ParameterValues();

            ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
            pdisval1.Value = sFromDate;
            pval1.Add(pdisval1);
            ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
            pdisval2.Value = sToDate;
            pval2.Add(pdisval2);
            ParameterDiscreteValue pdisval3 = new ParameterDiscreteValue();
            pdisval3.Value = sFromCompany;
            pval3.Add(pdisval3);
            ParameterDiscreteValue pdisval4 = new ParameterDiscreteValue();
            pdisval4.Value = sToCompany;
            pval4.Add(pdisval4);
            ParameterDiscreteValue pdisval5 = new ParameterDiscreteValue();
            pdisval5.Value = sUserCode;
            pval5.Add(pdisval5);

            myReportDocument.DataDefinition.ParameterFields["@fromdate"].ApplyCurrentValues(pval1);
            myReportDocument.DataDefinition.ParameterFields["@todate"].ApplyCurrentValues(pval2);
            myReportDocument.DataDefinition.ParameterFields["@FromCompany"].ApplyCurrentValues(pval3);
            myReportDocument.DataDefinition.ParameterFields["@ToCompany"].ApplyCurrentValues(pval4);
            myReportDocument.DataDefinition.ParameterFields["@UserCode"].ApplyCurrentValues(pval5);

            CrystalReportViewer1.ReportSource = myReportDocument;
        }
    }
}