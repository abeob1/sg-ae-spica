using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using AE_SPICA.DAL;
using CrystalDecisions.Shared;
using System.Configuration;

namespace AE_SPICA_V001
{
    public partial class NotYetBillReport : System.Web.UI.Page
    {
        #region Objects
        public string strSelect = "-- Select --";
        public clsReports oReports = new clsReports();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies[Constants.ScreenName].Value = "NotYet Bill Report";
                if (!IsPostBack)
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {

                lblSuccess.Text = string.Empty;
                lblerror.Text = string.Empty;
                if (txtFromDate.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the From Date";
                    return;
                }
                if (txtToDate.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the To Date";
                    return;
                }
                if (txtFromDate.Text.ToString() != string.Empty && txtToDate.Text.ToString() != string.Empty)
                {
                    DateTime dFromdate = Convert.ToDateTime(txtFromDate.Text);
                    DateTime dTodate = Convert.ToDateTime(txtToDate.Text);
                    if (dTodate < dFromdate)
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "To Date should not be less than From Date";
                        return;
                    }
                }

                Response.Cookies[Constants.NYBRFromDate].Value = txtFromDate.Text;
                Response.Cookies[Constants.NYBRToDate].Value = txtToDate.Text;
                Response.Redirect("ViewNotYetBillReport.aspx", false);

                //loadReport(txtFromDate.Text, txtToDate.Text);

            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        public void ClearFields()
        {
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            lblerror.Text = string.Empty;
            lblSuccess.Text = string.Empty;
            //loadReport(string.Empty, string.Empty);
        }

        //public void loadReport(string sFromDate, string sToDate)
        //{
        //    try
        //    {
        //        string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
        //        string[] dbInfo = connection.Split(';');

        //        ReportDocument objReport = new ReportDocument();
        //        objReport.Load(Server.MapPath("~/Reports/") + "AB_Billing_Report_Not_Yet_Bill.rpt");
        //        CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
        //        CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
        //        CrystalDecisions.Shared.ParameterValues pval3 = new ParameterValues();
        //        CrystalDecisions.Shared.ParameterValues pval4 = new ParameterValues();

        //        ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
        //        pdisval1.Value = sFromDate;
        //        pval1.Add(pdisval1);

        //        ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
        //        pdisval2.Value = sToDate;
        //        pval2.Add(pdisval2);

        //        //ParameterDiscreteValue pdisval3 = new ParameterDiscreteValue();
        //        //pdisval3.Value = Convert.ToInt32(ddlItems1.SelectedValue);
        //        //pval3.Add(pdisval3);

        //        //ParameterDiscreteValue pdisval4 = new ParameterDiscreteValue();
        //        //pdisval4.Value = Convert.ToInt32(ddlDepartments.SelectedValue);
        //        //pval4.Add(pdisval4);

        //        objReport.DataDefinition.ParameterFields["@fromdate"].ApplyCurrentValues(pval1);
        //        objReport.DataDefinition.ParameterFields["@todate"].ApplyCurrentValues(pval2);
        //        //report.DataDefinition.ParameterFields["@Parm3"].ApplyCurrentValues(pval3);
        //        //report.DataDefinition.ParameterFields["@Parm4"].ApplyCurrentValues(pval4);

        //        //get connection string from web.config

        //        CrystalDecisions.CrystalReports.Engine.Table myTable;
        //        CrystalDecisions.Shared.ConnectionInfo conn = new ConnectionInfo();
        //        CrystalDecisions.Shared.TableLogOnInfo myLog;
        //        string strServer = dbInfo[0].Split('=')[1];//ConfigurationSettings.AppSettings["SQLserver"].ToString();
        //        string strDBName = dbInfo[1].Split('=')[1];//ConfigurationSettings.AppSettings["SQLdatabaseName"].ToString();
        //        string strUID = dbInfo[2].Split('=')[1];//ConfigurationSettings.AppSettings["SQLUserName"].ToString();
        //        string strPassword = dbInfo[3].Split('=')[1];//ConfigurationSettings.AppSettings["SQLPassword"].ToString();

        //        conn.ServerName = strServer;
        //        conn.DatabaseName = strDBName;
        //        conn.UserID = strUID;
        //        conn.Password = strPassword;

        //        for (int i = 0; i < objReport.Database.Tables.Count; i++)
        //        {
        //            myTable = objReport.Database.Tables[i];
        //            myLog = myTable.LogOnInfo;
        //            myLog.ConnectionInfo = conn;
        //            myTable.ApplyLogOnInfo(myLog);
        //            myTable.Location = myLog.TableName;
        //        }

        //        CrystalReportViewer1.Visible = true;

        //        CrystalReportViewer1.HasPageNavigationButtons = true;
        //        CrystalReportViewer1.HasCrystalLogo = false;
        //        CrystalReportViewer1.HasDrillUpButton = false;
        //        CrystalReportViewer1.HasSearchButton = false;
        //        CrystalReportViewer1.HasToggleGroupTreeButton = false;
        //        CrystalReportViewer1.HasZoomFactorList = false;
        //        CrystalReportViewer1.ToolbarStyle.Width = new Unit("750px");

        //        CrystalReportViewer1.ReportSource = objReport;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblerror.Visible = true;
        //        lblerror.Text = ex.Message.ToString();
        //    }
        //}

        #endregion

    }
}