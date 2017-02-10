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
    public partial class DetailedBillReport : System.Web.UI.Page
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
                Response.Cookies[Constants.ScreenName].Value = "Detailed Bill Report";
                if (!IsPostBack)
                {
                    ClearFields();
                    LoadDropDowns();
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        private void LoadDropDowns()
        {
            try
            {

                DataSet dsClaimHandler = oReports.GetClaimHandler();
                ddlFromClaimHandler.DataSource = dsClaimHandler;
                ddlFromClaimHandler.DataTextField = "Name";
                ddlFromClaimHandler.DataValueField = "Code";
                ddlFromClaimHandler.DataBind();

                ddlToClaimHandler.DataSource = dsClaimHandler;
                ddlToClaimHandler.DataTextField = "Name";
                ddlToClaimHandler.DataValueField = "Code";
                ddlToClaimHandler.DataBind();

                DataSet dsCompany = oReports.GetCompany();
                ddlFromCompany.DataSource = dsCompany;
                ddlFromCompany.DataTextField = "Name";
                ddlFromCompany.DataValueField = "Code";
                ddlFromCompany.DataBind();

                ddlToCompany.DataSource = dsCompany;
                ddlToCompany.DataTextField = "Name";
                ddlToCompany.DataValueField = "Code";
                ddlToCompany.DataBind();

                ddlStatus.DataSource = oReports.GetReportStatus();
                ddlStatus.DataTextField = "Name";
                ddlStatus.DataValueField = "Code";
                ddlStatus.DataBind();
                
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

                if (ddlFromClaimHandler.Text == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the From Claim Handler";
                    return;
                }

                if (ddlToClaimHandler.Text == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the To Claim Handler";
                    return;
                }

                if (ddlFromCompany.Text == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the From Company";
                    return;
                }

                if (ddlToCompany.Text == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the To Company";
                    return;
                }

                if (ddlStatus.Text == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the Status";
                    return;
                }

                Response.Cookies[Constants.DBRFromDate].Value = txtFromDate.Text;
                Response.Cookies[Constants.DBRToDate].Value = txtToDate.Text;
                Response.Cookies[Constants.DBRFromClaim].Value = ddlFromClaimHandler.Text;
                Response.Cookies[Constants.DBRToClaim].Value = ddlToClaimHandler.Text;
                Response.Cookies[Constants.DBRFromComp].Value = ddlFromCompany.Text;
                Response.Cookies[Constants.DBRToComp].Value = ddlToCompany.Text;
                Response.Cookies[Constants.DBRStatus].Value = ddlStatus.Text;

                Response.Redirect("ViewDetailedBillReport.aspx", false);

                //loadReport(txtFromDate.Text, txtToDate.Text, ddlFromClaimHandler.Text, ddlToClaimHandler.Text, ddlFromCompany.Text, ddlToCompany.Text, ddlStatus.Text);

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

            ddlFromClaimHandler.SelectedIndex = -1;
            ddlToClaimHandler.SelectedIndex = -1;
            ddlFromCompany.SelectedIndex = -1;
            ddlToCompany.SelectedIndex = -1;
            ddlStatus.SelectedIndex = -1;

            //loadReport(string.Empty, string.Empty);
        }

        //public void loadReport(string sFromDate, string sToDate, string sFromCH, string sToCH, string sFromComp, string sToComp, string sStatus)
        //{
        //    try
        //    {
        //        string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
        //        string[] dbInfo = connection.Split(';');

        //        ReportDocument objReport = new ReportDocument();
        //        objReport.Load(Server.MapPath("~/Reports/") + "AB_Billing_Report_Details.rpt");
        //        CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
        //        CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
        //        CrystalDecisions.Shared.ParameterValues pval3 = new ParameterValues();
        //        CrystalDecisions.Shared.ParameterValues pval4 = new ParameterValues();
        //        CrystalDecisions.Shared.ParameterValues pval5 = new ParameterValues();
        //        CrystalDecisions.Shared.ParameterValues pval6 = new ParameterValues();
        //        CrystalDecisions.Shared.ParameterValues pval7 = new ParameterValues();

        //        ParameterDiscreteValue pdisval1 = new ParameterDiscreteValue();
        //        pdisval1.Value = sFromDate;
        //        pval1.Add(pdisval1);

        //        ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
        //        pdisval2.Value = sToDate;
        //        pval2.Add(pdisval2);

        //        ParameterDiscreteValue pdisval3 = new ParameterDiscreteValue();
        //        pdisval3.Value = sFromCH;
        //        pval3.Add(pdisval3);

        //        ParameterDiscreteValue pdisval4 = new ParameterDiscreteValue();
        //        pdisval4.Value = sToCH;
        //        pval4.Add(pdisval4);

        //        ParameterDiscreteValue pdisval5 = new ParameterDiscreteValue();
        //        pdisval5.Value = sFromComp;
        //        pval5.Add(pdisval5);

        //        ParameterDiscreteValue pdisval6 = new ParameterDiscreteValue();
        //        pdisval6.Value = sToComp;
        //        pval6.Add(pdisval6);

        //        ParameterDiscreteValue pdisval7 = new ParameterDiscreteValue();
        //        pdisval7.Value = sStatus;
        //        pval7.Add(pdisval7);



        //        objReport.DataDefinition.ParameterFields["@fromdate"].ApplyCurrentValues(pval1);
        //        objReport.DataDefinition.ParameterFields["@todate"].ApplyCurrentValues(pval2);
        //        objReport.DataDefinition.ParameterFields["fromCH@select ClaimHandler from tbl_FileReference"].ApplyCurrentValues(pval3);
        //        objReport.DataDefinition.ParameterFields["ToCH@select ClaimHandler from tbl_FileReference"].ApplyCurrentValues(pval4);
        //        objReport.DataDefinition.ParameterFields["FromCompany@select Name from tbl_CompanyData"].ApplyCurrentValues(pval5);
        //        objReport.DataDefinition.ParameterFields["ToCompany@select Name from tbl_CompanyData"].ApplyCurrentValues(pval6);
        //        objReport.DataDefinition.ParameterFields["Status@select ApprovalStatus from tbl_FileReference"].ApplyCurrentValues(pval7);

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