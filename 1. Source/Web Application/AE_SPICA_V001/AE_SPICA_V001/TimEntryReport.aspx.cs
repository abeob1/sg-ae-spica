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
    public partial class TimEntryReport : System.Web.UI.Page
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
                Response.Cookies[Constants.ScreenName].Value = "Time Entry Report";
                if (!IsPostBack)
                {
                    ClearFields();
                    DataSet ds = new DataSet();
                    string sRoleName = Request.Cookies[Constants.UserRoleName].Value.ToString();
                    string sCompanyCode = Request.Cookies[Constants.CompanyCode].Value.ToString();
                    ds = oLogin.GetCompanyBasedOnUserRole(sRoleName, sCompanyCode);
                    ddlFromCompany.DataSource = ds;
                    ddlFromCompany.DataTextField = "Name";
                    ddlFromCompany.DataValueField = "Code";
                    ddlFromCompany.DataBind();
                    ddlToCompany.DataSource = ds;
                    ddlToCompany.DataTextField = "Name";
                    ddlToCompany.DataValueField = "Code";
                    ddlToCompany.DataBind();
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
                if (ddlFromCompany.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select From Company";
                    return;
                }
                if (ddlToCompany.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select To Company";
                    return;
                }

                Response.Cookies[Constants.TERFromDate].Value = txtFromDate.Text;
                Response.Cookies[Constants.TERToDate].Value = txtToDate.Text;
                Response.Cookies[Constants.TERFromCompany].Value = ddlFromCompany.SelectedItem.ToString();
                Response.Cookies[Constants.TERToCompany].Value = ddlToCompany.SelectedItem.ToString();
                Response.Redirect("ViewTimeEntryReport.aspx", false);

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
            ddlFromCompany.SelectedIndex = 0;
            ddlToCompany.SelectedIndex = 0;
        }

        #endregion
    }
}