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
    public partial class ViewStatusReport : System.Web.UI.Page
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
            myReportDocument.Load(Server.MapPath("~/Reports/") + "StatusReport.rpt");
            myReportDocument.SetDatabaseLogon(strUID, strPassword, strServer, strDBName);

            string sFromIncidentDate = Convert.ToString(Request.Cookies[Constants.SRFromIncidentDate].Value);
            string sToIncidentDate = Convert.ToString(Request.Cookies[Constants.SRToIncidentDate].Value);
            string sFromPeriodDate = Convert.ToString(Request.Cookies[Constants.SRFromPeriodDate].Value);
            string sToPeriodDate = Convert.ToString(Request.Cookies[Constants.SRToPeriodDate].Value);
            string sFromCH = Convert.ToString(Request.Cookies[Constants.SRFromCH].Value);
            string sToCH = Convert.ToString(Request.Cookies[Constants.SRToCH].Value);
            string sStatus = Convert.ToString(Request.Cookies[Constants.SRStatus].Value);

            CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval3 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval4 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval5 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval6 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval7 = new ParameterValues();

            ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
            pdisval1.Value = sFromIncidentDate;
            pval1.Add(pdisval1);
            ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
            pdisval2.Value = sToIncidentDate;
            pval2.Add(pdisval2);
            ParameterDiscreteValue pdisval3 = new ParameterDiscreteValue();
            pdisval3.Value = sFromPeriodDate;
            pval3.Add(pdisval3);
            ParameterDiscreteValue pdisval4 = new ParameterDiscreteValue();
            pdisval4.Value = sToPeriodDate;
            pval4.Add(pdisval4);
            ParameterDiscreteValue pdisval5 = new ParameterDiscreteValue();
            pdisval5.Value = sFromCH;
            pval5.Add(pdisval5);
            ParameterDiscreteValue pdisval6 = new ParameterDiscreteValue();
            pdisval6.Value = sToCH;
            pval6.Add(pdisval6);
            ParameterDiscreteValue pdisval7 = new ParameterDiscreteValue();
            pdisval7.Value = sStatus;
            pval7.Add(pdisval7);

            myReportDocument.DataDefinition.ParameterFields["@FromFileCreationDate"].ApplyCurrentValues(pval1);
            myReportDocument.DataDefinition.ParameterFields["@ToFileCreationDate"].ApplyCurrentValues(pval2);
            myReportDocument.DataDefinition.ParameterFields["@FromPeriodDate"].ApplyCurrentValues(pval3);
            myReportDocument.DataDefinition.ParameterFields["@ToPeriodDate"].ApplyCurrentValues(pval4);
            myReportDocument.DataDefinition.ParameterFields["@FromCH"].ApplyCurrentValues(pval5);
            myReportDocument.DataDefinition.ParameterFields["@ToCH"].ApplyCurrentValues(pval6);
            myReportDocument.DataDefinition.ParameterFields["@Status"].ApplyCurrentValues(pval7);

            CrystalReportViewer1.ReportSource = myReportDocument;
        }
    }
}