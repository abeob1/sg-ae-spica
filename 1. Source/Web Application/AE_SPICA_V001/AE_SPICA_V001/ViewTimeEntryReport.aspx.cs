using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;

namespace AE_SPICA_V001
{
    public partial class ViewTimeEntryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            string[] dbInfo = connection.Split(';');
            string strServer = dbInfo[0].Split('=')[1];//ConfigurationSettings.AppSettings["SQLserver"].ToString();
            string strDBName = dbInfo[1].Split('=')[1];//ConfigurationSettings.AppSettings["SQLdatabaseName"].ToString();
            string strUID = dbInfo[2].Split('=')[1];//ConfigurationSettings.AppSettings["SQLUserName"].ToString();
            string strPassword = dbInfo[3].Split('=')[1];//ConfigurationSettings.AppSettings["SQLPassword"].ToString();

            ReportDocument myReportDocument;
            myReportDocument = new ReportDocument();
            myReportDocument.Load(Server.MapPath("~/Reports/") + "AB_BIlling_Report_TimeEntry.rpt");
            myReportDocument.SetDatabaseLogon(strUID, strPassword, strServer, strDBName);

            string sFromDate = Convert.ToString(Request.Cookies[Constants.TERFromDate].Value);
            string sToDate = Convert.ToString(Request.Cookies[Constants.TERToDate].Value);

            CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
            ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
            pdisval1.Value = sFromDate;
            pval1.Add(pdisval1);
            ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
            pdisval2.Value = sToDate;
            pval2.Add(pdisval2);

            myReportDocument.DataDefinition.ParameterFields["@fromdate"].ApplyCurrentValues(pval1);
            myReportDocument.DataDefinition.ParameterFields["@todate"].ApplyCurrentValues(pval2);


            CrystalReportViewer1.ReportSource = myReportDocument;
        }
    }
}