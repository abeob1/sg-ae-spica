using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AE_SPICA.DAL;

namespace AE_SPICA_V001
{
    public partial class Login : System.Web.UI.Page
    {
        #region Objects
        public static string strSelect = "-- Select --";
        public clsLogin oLogin = new clsLogin();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Response.Cookies[Constants.UserName].Expires = DateTime.Now;
                    if (Convert.ToString(Response.Cookies[Constants.SessionExpired].Value) == "SessionExpired")
                    {
                        // Display the alert in login page, if the session has expired
                        Response.Write(@"<script language='javascript'>alert('The current session of 20 mins has been Expired. \n Kindly Login Again');</script>");
                        Response.Cookies[Constants.SessionExpired].Expires = DateTime.Now;
                    }

                    DataSet ds = oLogin.GetCompanyDetails();
                    ddlCompany.DataSource = ds;
                    ddlCompany.DataTextField = "Name";
                    ddlCompany.DataValueField = "Code";
                    ddlCompany.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Message.ToString();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string Password = txtPassword.Text;
                txtPassword.Attributes.Add("value", Password);
                if (txtUserCode.Text == string.Empty)
                { lblError.Text = "Kindly enter username"; }
                else if (txtPassword.Text == string.Empty)
                { lblError.Text = "Kindly enter password"; }
                else if (ddlCompany.Text == strSelect || ddlCompany.Text == string.Empty)
                { lblError.Text = "Kindly select the Company"; }
                else
                {
                    var ds = oLogin.LoginValidation(txtUserCode.Text, txtPassword.Text, ddlCompany.SelectedValue);
                    //DataSet ds = new DataSet();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string sValue = ds.Tables[0].Rows[0]["Value"].ToString();
                        if (sValue == "0")
                        {
                            lblError.Text = "User is InActive.";
                        }
                        else if (sValue == "1")
                        {
                            lblError.Text = "User is not in this company";
                        }
                        else if (sValue == "2")
                        {
                            if (Response.Cookies[Constants.IsUpdate] != null)
                            {
                                Response.Cookies[Constants.IsUpdate].Value = string.Empty;
                            }
                            if (Response.Cookies[Constants.ScreenName] != null)
                            {
                                Response.Cookies[Constants.ScreenName].Value = string.Empty;
                            }
                            string sUserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                            string sUserRoleName = ds.Tables[0].Rows[0]["UserRoleName"].ToString();
                            Response.Cookies[Constants.UserCode].Value = txtUserCode.Text.Trim().ToUpper().ToString();
                            Response.Cookies[Constants.UserName].Value = sUserName;
                            Response.Cookies[Constants.Password].Value = txtPassword.Text.Trim().ToString(); ;
                            Response.Cookies[Constants.UserRoleName].Value = sUserRoleName;
                            Response.Cookies[Constants.SAPDBName].Value = ds.Tables[0].Rows[0]["SAPDBName"].ToString(); ;
                            Response.Cookies[Constants.CompanyCode].Value = ddlCompany.SelectedValue.ToString(); // Company Id
                            Response.Cookies[Constants.CompanyName].Value = ddlCompany.SelectedItem.ToString(); // Company Name

                            Response.Cookies[Constants.UserName].Expires = DateTime.Now.AddDays(1);
                            Response.Cookies[Constants.CompanyCode].Expires = DateTime.Now.AddDays(1);
                            Response.Cookies[Constants.CompanyName].Expires = DateTime.Now.AddDays(1);
                            Response.Cookies[Constants.SAPDBName].Expires = DateTime.Now.AddDays(1);

                            Response.Redirect("Dashboard.aspx", false);
                        }
                        else if (sValue == "3")
                        {
                            lblError.Text = "Invalid UserName / Password";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Message.ToString();
            }
        }
        #endregion

        #region Methods
        public void LoadCompanyDropdown()
        {
            try
            {
                oLogin.GetCompanyDetails();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Message.ToString();
            }
        }
        #endregion
    }
}