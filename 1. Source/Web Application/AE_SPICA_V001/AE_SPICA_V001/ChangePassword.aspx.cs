using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AE_SPICA.DAL;

namespace AE_SPICA_V001
{
    public partial class ChangePassword : System.Web.UI.Page
    {

        #region Objects
        public string strSelect = "-- Select --";
        public clsLogin oLogin = new clsLogin();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies[Constants.ScreenName].Value = "Change Password";
                if (!IsPostBack)
                {

                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Visible = false;
                lblerror.Text = string.Empty;
                lblSuccess.Visible = true;
                lblSuccess.Text = string.Empty;

                string oldPassword = txtOldPassword.Text;
                string newPass = txtNewPassword.Text;
                string ConfirmPass = txtConfirmPassword.Text;

                txtOldPassword.Attributes.Add("value", oldPassword);
                txtNewPassword.Attributes.Add("value", newPass);
                txtConfirmPassword.Attributes.Add("value", ConfirmPass);

                string sPassword = Convert.ToString(Request.Cookies[Constants.Password].Value);
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);

                string soldPassword = oLogin.GetPassword(sUserCode, sCompanyCode);

                if (txtOldPassword.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the old password";
                    return;
                }
                if (txtNewPassword.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the new password";
                    return;
                }
                if (txtConfirmPassword.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the confirm password";
                    return;
                }
                if (soldPassword.ToString() != txtOldPassword.Text)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the correct Old password";
                    return;
                }
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "New Password and confirm password must be same";
                    return;
                }

                string sResult = oLogin.ChangePassword(txtConfirmPassword.Text, sUserCode, sCompanyCode);
                if (sResult == "SUCCESS")
                {
                    //string message = "Password Changed Successfully and You will now be redirected to Login Page.";
                    //string url = "Login.aspx";
                    //string script = "window.onload = function(){ alert('";
                    //script += message;
                    //script += "');";
                    //script += "window.location = '";
                    //script += url;
                    //script += "'; }";
                    //ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                    //string message = "You will now be redirected to ASPSnippets Home Page.";
                    //string url = "http://www.aspsnippets.com/";
                    //string script = "window.onload = function(){ alert('";
                    //script += message;
                    //script += "');";
                    //script += "window.location = '";
                    //script += url;
                    //script += "'; }";
                    //ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);

                    //Response.Write(@"<script language='javascript'>alert('Password Changed Successfully. \n Kinldy Login Again');</script>");
                    //Response.Redirect("Login.aspx", false);
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Password Changed Successfully.";
                    clearFields();
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = sResult;
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblSuccess.Text = string.Empty;
                lblerror.Text = string.Empty;
                clearFields();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message;
            }
        }

        public void clearFields()
        {
            try
            {
                //lblerror.Visible = false;
                //lblerror.Text = string.Empty;
                //lblSuccess.Visible = true;
                //lblSuccess.Text = string.Empty;
                txtOldPassword.Attributes.Add("value", string.Empty);
                txtNewPassword.Attributes.Add("value", string.Empty);
                txtConfirmPassword.Attributes.Add("value", string.Empty);
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        #endregion Events
    }
}