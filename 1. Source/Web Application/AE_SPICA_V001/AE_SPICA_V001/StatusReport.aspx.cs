using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AE_SPICA.DAL;
using System.Configuration;
using System.Data;

namespace AE_SPICA_V001
{
    public partial class StatusReport : System.Web.UI.Page
    {
        #region Objects
        public string strSelect = "-- Select --";
        public clsReports oReports = new clsReports();
        public clsLogin oLogin = new clsLogin();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies[Constants.ScreenName].Value = "Status Report";
                if (!IsPostBack)
                {
                    ClearFields();
                    
                    DataSet dsClaimHandler = oReports.GetClaimHandler();
                    ddlFromCH.DataSource = dsClaimHandler;
                    ddlFromCH.DataTextField = "Name";
                    ddlFromCH.DataValueField = "Code";
                    ddlFromCH.DataBind();

                    ddlToCH.DataSource = dsClaimHandler;
                    ddlToCH.DataTextField = "Name";
                    ddlToCH.DataValueField = "Code";
                    ddlToCH.DataBind();

                    ddlStatus.Items.Insert(0, new ListItem("-- Select --", "-- Select --"));
                    ddlStatus.Items.Insert(1, new ListItem("Approved", "Approved"));
                    ddlStatus.Items.Insert(2, new ListItem("Not Yet Billed", "Not Yet Billed"));
                    ddlStatus.Items.Insert(2, new ListItem("Pending", "Pending"));
                    ddlStatus.Items.Insert(3, new ListItem("ALL", "ALL"));
                    ddlStatus.SelectedIndex = 0;
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
                string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                string[] dbInfo = connection.Split(';');
                lblSuccess.Text = string.Empty;
                lblerror.Text = string.Empty;
                if (txtIncidentFromDate.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the Incident From Date";
                    return;
                }
                if (txtIncidentToDate.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the Incident To Date";
                    return;
                }
                if (txtIncidentFromDate.Text.ToString() != string.Empty && txtIncidentToDate.Text.ToString() != string.Empty)
                {
                    DateTime dIncidentFromdate = Convert.ToDateTime(txtIncidentFromDate.Text);
                    DateTime dIncidentTodate = Convert.ToDateTime(txtIncidentToDate.Text);
                    if (dIncidentTodate < dIncidentFromdate)
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Incident To Date should not be less than Incident From Date";
                        return;
                    }
                }

                //if (txtPeriodFromDate.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Select the Period From Date";
                //    return;
                //}
                if (txtPeriodToDate.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the Time entry To Date";
                    return;
                }

                if (txtPeriodFromDate.Text.ToString() != string.Empty && txtPeriodToDate.Text.ToString() != string.Empty)
                {
                    DateTime dPeriodFromdate = Convert.ToDateTime(txtPeriodFromDate.Text);
                    DateTime dPeriodTodate = Convert.ToDateTime(txtPeriodToDate.Text);
                    if (dPeriodTodate < dPeriodFromdate)
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Time entry To Date should not be less than Time entry From Date";
                        return;
                    }
                }

                if (ddlFromCH.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select From Claim Handler";
                    return;
                }
                if (ddlToCH.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select To Claim Handler";
                    return;
                }

                if (ddlStatus.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the Status";
                    return;
                }

                Response.Cookies[Constants.SRFromIncidentDate].Value = txtIncidentFromDate.Text;
                Response.Cookies[Constants.SRToIncidentDate].Value = txtIncidentToDate.Text;
                if (txtPeriodFromDate.Text != string.Empty)
                {
                    Response.Cookies[Constants.SRFromPeriodDate].Value = txtPeriodFromDate.Text;
                }
                else
                {
                    Response.Cookies[Constants.SRFromPeriodDate].Value = "1900-01-01";
                }
                Response.Cookies[Constants.SRToPeriodDate].Value = txtPeriodToDate.Text;
                Response.Cookies[Constants.SRFromCH].Value = ddlFromCH.SelectedItem.ToString(); ;
                Response.Cookies[Constants.SRToCH].Value = ddlToCH.SelectedItem.ToString();
                Response.Cookies[Constants.SRStatus].Value = ddlStatus.SelectedItem.ToString();
                Response.Redirect("ViewStatusReport.aspx", false);

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
            txtIncidentFromDate.Text = string.Empty;
            txtIncidentToDate.Text = string.Empty;
            txtPeriodFromDate.Text = string.Empty;
            txtPeriodToDate.Text = string.Empty;
            lblerror.Text = string.Empty;
            lblSuccess.Text = string.Empty;
            ddlFromCH.SelectedIndex = 0;
            ddlToCH.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        #endregion

    }
}