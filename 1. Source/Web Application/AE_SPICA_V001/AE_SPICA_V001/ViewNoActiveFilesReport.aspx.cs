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
    public partial class ViewNoActiveFilesReport : System.Web.UI.Page
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
            myReportDocument.Load(Server.MapPath("~/Reports/") + "AB_BIlling_Report_NoActiveFiles.rpt");
            myReportDocument.SetDatabaseLogon(strUID, strPassword, strServer, strDBName);

            string sFromDate = Convert.ToString(Request.Cookies[Constants.NAFRFromDate].Value);
            string sToDate = Convert.ToString(Request.Cookies[Constants.NAFRToDate].Value);
            string sFilterBy = Convert.ToString(Request.Cookies[Constants.NAFRFilterBy].Value);
            string sBranch = Convert.ToString(Request.Cookies[Constants.NAFRBranch].Value);
            string sClaimHandler = Convert.ToString(Request.Cookies[Constants.NAFRClaimHandler].Value);

            CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval3 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval4 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval5 = new ParameterValues();

            ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
            ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
            ParameterDiscreteValue pdisval3 = new ParameterDiscreteValue();
            ParameterDiscreteValue pdisval4 = new ParameterDiscreteValue();
            ParameterDiscreteValue pdisval5 = new ParameterDiscreteValue();

            pdisval1.Value = sFromDate;
            pval1.Add(pdisval1);

            pdisval2.Value = sToDate;
            pval2.Add(pdisval2);

            pdisval3.Value = sFilterBy;
            pval3.Add(pdisval3);

            pdisval4.Value = sBranch;
            pval4.Add(pdisval4);

            pdisval5.Value = sClaimHandler;
            pval5.Add(pdisval5);

            myReportDocument.DataDefinition.ParameterFields["@fromdate"].ApplyCurrentValues(pval1);
            myReportDocument.DataDefinition.ParameterFields["@todate"].ApplyCurrentValues(pval2);
            myReportDocument.DataDefinition.ParameterFields["@filterby"].ApplyCurrentValues(pval3);
            myReportDocument.DataDefinition.ParameterFields["@branch select Name from tbl_CompanyData order by Name"].ApplyCurrentValues(pval4);
            myReportDocument.DataDefinition.ParameterFields["@claimhandler select distinct ClaimHandler from tbl_FileReference order by ClaimHandler"].ApplyCurrentValues(pval5);

            CrystalReportViewer1.ReportSource = myReportDocument;
        }

    }
}