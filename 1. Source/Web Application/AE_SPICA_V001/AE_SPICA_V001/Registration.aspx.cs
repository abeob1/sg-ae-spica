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
    public partial class Registration : System.Web.UI.Page
    {
        #region Objects
        public clsUser oUser = new clsUser();
        public clsLogin oDataObject = new clsLogin();
        public string strSelect = "-- Select --";

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                //txtPassword.ReadOnly = false;
                //txtConfirmPass.ReadOnly = false;
                Response.Cookies[Constants.ScreenName].Value = "User Registration";
                Response.Cookies[Constants.Update].Value = string.Empty;
                if (!IsPostBack)
                {
                    ddlUserRole.DataSource = oUser.GetUserRole();
                    ddlUserRole.DataTextField = "Name";
                    ddlUserRole.DataValueField = "Code";
                    ddlUserRole.DataBind();

                    txtCompanyName.Text = Convert.ToString(Request.Cookies[Constants.CompanyName].Value);
                    txtCompanyCode.Text = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    chkActive.Checked = true;

                    string sUserRoleName = Convert.ToString(Request.Cookies[Constants.UserRoleName].Value);
                    string sRole1 = System.Configuration.ConfigurationManager.AppSettings["Role1"].ToString();
                    string sRole2 = System.Configuration.ConfigurationManager.AppSettings["Role2"].ToString();
                    if (sUserRoleName.ToUpper() == sRole1.ToString().ToUpper() || sUserRoleName.ToUpper() == sRole2.ToString().ToUpper())
                    {
                        txtCompanyName.Visible = false;
                        ddlCompany.Visible = true;
                        DataSet ds = oDataObject.GetCompanyDetails();
                        ddlCompany.DataSource = ds;
                        ddlCompany.DataTextField = "Name";
                        ddlCompany.DataValueField = "Code";
                        ddlCompany.DataBind();
                    }
                    else
                    {
                        txtCompanyName.Visible = true;
                        ddlCompany.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserSearch.Text == string.Empty || txtUserSearch.Text == "")
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly key in the data to search";
                }
                else
                {
                    Response.Cookies[Constants.IsUpdate].Expires = DateTime.Now;
                    lblerror.Text = string.Empty;
                    lblSuccess.Text = string.Empty;
                    DataSet sResult = oUser.SearchUser(txtUserSearch.Text, txtCompanyCode.Text);
                    if (sResult != null && sResult.Tables[0].Rows.Count > 0)
                    {
                        //txtPassword.ReadOnly = true;
                        //txtConfirmPass.ReadOnly = true;
                        AssignValues(sResult.Tables[0]);
                        Response.Cookies[Constants.Update].Value = "1";
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Search result doesnot exist";
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        private void AssignValues(DataTable dataTable)
        {
            try
            {
                txtUserCode.Text = dataTable.Rows[0]["UserCode"].ToString();
                txtUserName.Text = dataTable.Rows[0]["UserName"].ToString();
                txtPassword.Text = dataTable.Rows[0]["Password"].ToString();
                txtConfirmPass.Text = dataTable.Rows[0]["Password"].ToString();
                txtPassword.Attributes.Add("value", txtPassword.Text);
                txtConfirmPass.Attributes.Add("value", txtConfirmPass.Text);
                ddlUserRole.SelectedValue = dataTable.Rows[0]["RoleId"].ToString();
                txtEmail.Text = dataTable.Rows[0]["Email"].ToString();
                txtCompanyCode.Text = dataTable.Rows[0]["CompanyCode"].ToString();
                txtCompanyName.Text = dataTable.Rows[0]["CompanyName"].ToString();
                ddlCompany.SelectedValue = dataTable.Rows[0]["CompanyCode"].ToString();
                ViewState["OldCompanyCode"] = dataTable.Rows[0]["CompanyCode"].ToString();
                if (dataTable.Rows[0]["IsActive"].ToString() == "Y")
                {
                    chkActive.Checked = true;
                }
                else
                { chkActive.Checked = false; }
                txtUserCode.ReadOnly = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                string Password = txtPassword.Text;
                string ConfirmPass = txtConfirmPass.Text;
                txtPassword.Attributes.Add("value", Password);
                txtConfirmPass.Attributes.Add("value", ConfirmPass);
                if (txtUserCode.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the User Code";
                    return;
                }
                if (txtUserName.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the User Name";
                    return;
                }
                //if (Request.Cookies[Constants.Update].Value == string.Empty || Request.Cookies[Constants.Update].Value == "")
                //{
                if (txtPassword.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the password";
                    return;
                }
                if (txtConfirmPass.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the Confirm Password";
                    return;
                }
                if (txtPassword.Text != txtConfirmPass.Text)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Password and confirm password must be same";
                    return;
                }
                //}
                if (ddlUserRole.SelectedValue == strSelect)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly select the User Role";
                    return;
                }
                if (txtEmail.Text != string.Empty)
                {
                    var result = "@,.".Split(',');
                    foreach (var item in result)
                    {
                        if (!txtEmail.Text.Contains(item))
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Kindly enter valid email";
                            return;
                        }
                    }
                }
                if (ddlCompany.SelectedValue == strSelect)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly select the Company";
                    return;
                }
                DataTable dt = new DataTable();
                dt.TableName = "tbl_Users";
                dt = CreateTableColumns(dt);
                string sResult = string.Empty;
                if (Request.Cookies[Constants.Update].Value == string.Empty || Request.Cookies[Constants.Update].Value == "")
                {
                    sResult = oUser.CreateUser(dt);
                }
                else
                {
                    string oldCompanyCode = ViewState["OldCompanyCode"].ToString();
                    sResult = oUser.UpdateUser(dt, oldCompanyCode);
                }
                if (sResult == Constants.Insert)
                {
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    //txtPassword.ReadOnly = false;
                    //txtConfirmPass.ReadOnly = false;
                    ClearFields();
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "User registered successfully";
                    lblSuccess.Focus();
                }
                else if (sResult == Constants.Update)
                {
                    Response.Cookies[Constants.Update].Expires = DateTime.Now;
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    ClearFields();
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "User updated successfully";
                    lblSuccess.Focus();
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
                lblerror.Text = ex.Message.ToString();
            }
        }

        private DataTable CreateTableColumns(DataTable dt)
        {
            try
            {
                dt.Columns.Add("UserCode");
                dt.Columns.Add("UserName");
                dt.Columns.Add("Password");
                dt.Columns.Add("RoleId");
                dt.Columns.Add("Email");
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("IsActive");

                dt.Rows.Add();
                dt.Rows[0]["UserCode"] = txtUserCode.Text;
                dt.Rows[0]["UserName"] = txtUserName.Text;
                dt.Rows[0]["Password"] = txtPassword.Text;
                dt.Rows[0]["RoleId"] = ddlUserRole.SelectedValue;
                dt.Rows[0]["Email"] = txtEmail.Text;
                dt.Rows[0]["CompanyCode"] = ddlCompany.SelectedValue;
                dt.Rows[0]["IsActive"] = chkActive.Checked == true ? "Y" : "N";

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                //txtPassword.ReadOnly = false;
                //txtConfirmPass.ReadOnly = false;
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        public void ClearFields()
        {
            txtUserSearch.Text = string.Empty;
            txtUserCode.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPass.Text = string.Empty;
            txtPassword.Attributes.Add("value", "");
            txtConfirmPass.Attributes.Add("value", "");
            ddlUserRole.SelectedIndex = -1;
            ddlCompany.SelectedIndex = -1;
            txtEmail.Text = string.Empty;
            chkActive.Checked = false;
            Response.Cookies[Constants.Update].Expires = DateTime.Now;
            txtUserCode.ReadOnly = false;
        }

        #endregion
    }
}