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
    public partial class PreviewHoldingInv : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            string sDocEntry = Convert.ToString(Request.Cookies[Constants.HoldingInvDocEntry].Value);
            string sType = Convert.ToString(Request.Cookies[Constants.HoldingInvType].Value);
            string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            string[] dbInfo = connection.Split(';');
            string strServer = dbInfo[0].Split('=')[1];//ConfigurationSettings.AppSettings["SQLserver"].ToString();
            string strDBName = dbInfo[1].Split('=')[1];//ConfigurationSettings.AppSettings["SQLdatabaseName"].ToString();
            string strUID = dbInfo[2].Split('=')[1];//ConfigurationSettings.AppSettings["SQLUserName"].ToString();
            string strPassword = dbInfo[3].Split('=')[1];//ConfigurationSettings.AppSettings["SQLPassword"].ToString();

            ReportDocument myReportDocument;
            myReportDocument = new ReportDocument();
            if (sType == "AR Inv")
            {
                myReportDocument.Load(Server.MapPath("~/Normal Invoice Layout/") + "Normal Invoice_V2.rpt");
            }
            else if(sType == "AR CM")
            {
                myReportDocument.Load(Server.MapPath("~/Normal Invoice Layout/") + "Normal Invoice_Credit Memo.rpt");
            }
            myReportDocument.SetDatabaseLogon(strUID, strPassword, strServer, strDBName);


            CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
            ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
            pdisval1.Value = sDocEntry;
            pval1.Add(pdisval1);

            myReportDocument.DataDefinition.ParameterFields["DocKey@"].ApplyCurrentValues(pval1);
            CrystalReportViewer1.ReportSource = myReportDocument;
        }
    }
}