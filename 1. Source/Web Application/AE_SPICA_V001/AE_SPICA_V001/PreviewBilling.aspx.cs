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
    public partial class PreviewBilling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sFileRef = Convert.ToString(Request.Cookies[Constants.FileRefNo].Value);
            string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            string[] dbInfo = connection.Split(';');
            string strServer = dbInfo[0].Split('=')[1];//ConfigurationSettings.AppSettings["SQLserver"].ToString();
            string strDBName = dbInfo[1].Split('=')[1];//ConfigurationSettings.AppSettings["SQLdatabaseName"].ToString();
            string strUID = dbInfo[2].Split('=')[1];//ConfigurationSettings.AppSettings["SQLUserName"].ToString();
            string strPassword = dbInfo[3].Split('=')[1];//ConfigurationSettings.AppSettings["SQLPassword"].ToString();

            ReportDocument myReportDocument;
            myReportDocument = new ReportDocument();
            myReportDocument.Load(Server.MapPath("~/Reports/") + "AB_Billing_Preview.rpt");
            myReportDocument.SetDatabaseLogon(strUID, strPassword, strServer, strDBName);


            CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
            ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
            pdisval1.Value = sFileRef;
            pval1.Add(pdisval1);

            myReportDocument.DataDefinition.ParameterFields["@FileReferenceNo"].ApplyCurrentValues(pval1);
            CrystalReportViewer1.ReportSource = myReportDocument;



        }
    }
}