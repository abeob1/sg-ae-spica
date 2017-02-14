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
    public partial class NoActiveFilesReport : System.Web.UI.Page
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
                Response.Cookies[Constants.ScreenName].Value = "No Active Files Report";
                if (!IsPostBack)
                {
                    ddlFilterBy.Items.Insert(0, new ListItem("-- Select --", "-- Select --"));
                    ddlFilterBy.Items.Insert(1, new ListItem("Branch", "Branch"));
                    ddlFilterBy.Items.Insert(2, new ListItem("ClaimHandler", "ClaimHandler"));
                    ddlFilterBy.SelectedIndex = 0;

                    string sRoleName = Request.Cookies[Constants.UserRoleName].Value.ToString();
                    string sCompanyCode = Request.Cookies[Constants.CompanyCode].Value.ToString();
                    DataSet ds = oLogin.GetCompanyBasedOnUserRole(sRoleName, sCompanyCode);
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "Name";
                    ddlBranch.DataValueField = "Code";
                    ddlBranch.DataBind();

                    DataSet dsClaimHandler = oReports.GetClaimHandler();
                    ddlClaimHandler.DataSource = dsClaimHandler;
                    ddlClaimHandler.DataTextField = "Name";
                    ddlClaimHandler.DataValueField = "Name";
                    ddlClaimHandler.DataBind();

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
                string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                string[] dbInfo = connection.Split(';');
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

                if (ddlFilterBy.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select Filter By";
                    return;
                }

                if (ddlFilterBy.Text == "Branch")
                {
                    if (ddlBranch.Text == strSelect.ToString())
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Kindly Enter the Branch";
                        return;
                    }
                }
                else if (ddlFilterBy.Text == "ClaimHandler")
                {
                    if (ddlClaimHandler.Text == strSelect.ToString())
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Kindly Enter the Claim Handler";
                        return;
                    }
                }

                Response.Cookies[Constants.NAFRFromDate].Value = txtFromDate.Text;
                Response.Cookies[Constants.NAFRToDate].Value = txtToDate.Text;
                Response.Cookies[Constants.NAFRFilterBy].Value = ddlFilterBy.SelectedValue;
                Response.Cookies[Constants.NAFRBranch].Value = ddlBranch.SelectedItem.ToString();
                Response.Cookies[Constants.NAFRClaimHandler].Value = ddlClaimHandler.SelectedValue;


                Response.Redirect("ViewNoActiveFilesReport.aspx", false);

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
            ddlFilterBy.SelectedIndex = 0;
            ddlClaimHandler.SelectedIndex = 0;
            ddlBranch.SelectedIndex = 0;
            lblerror.Text = string.Empty;
            lblSuccess.Text = string.Empty;
        }

        #endregion
    }
}