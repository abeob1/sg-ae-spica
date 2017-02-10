using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AE_SPICA.DAL;
using System.Drawing;

namespace AE_SPICA_V001
{
    public partial class ApprovalSetup : System.Web.UI.Page
    {
        #region Objects
        public clsApproverSetup oApprover = new clsApproverSetup();
        public string strSelect = "-- Select --";

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                Response.Cookies[Constants.ScreenName].Value = "Approval Setup";
                if (!IsPostBack)
                {
                    if (Response.Cookies[Constants.Update] != null)
                    {
                        Response.Cookies[Constants.Update].Value = string.Empty;
                    }
                    ddlCompany.DataSource = oApprover.GetCompanyDetails();
                    ddlCompany.DataTextField = "Name";
                    ddlCompany.DataValueField = "Code";
                    ddlCompany.DataBind();
                    grvSearch.DataBind();
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
                if (txtCompanySearch.Text == string.Empty || txtCompanySearch.Text == "")
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly key in the data to search";
                    grvSearch.DataBind();
                }
                else
                {
                    Response.Cookies[Constants.IsUpdate].Expires = DateTime.Now;
                    lblerror.Text = string.Empty;
                    lblSuccess.Text = string.Empty;
                    DataSet sResult = oApprover.SearchApprover(txtCompanySearch.Text);
                    if (sResult != null && sResult.Tables[0].Rows.Count > 0)
                    {
                        ViewState["SearchResult"] = sResult.Tables[0];
                        ViewState["sortOrder"] = "";
                        BindGridView("", "");

                        //AssignValues(sResult.Tables[0]);
                        //Response.Cookies[Constants.Update].Value = "1";
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Search result doesnot exist";
                        grvSearch.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                if (ddlCompany.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly select the Company";
                    return;
                }
                if (ddlApprover.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the Approver";
                    return;
                }

                DataTable dt = new DataTable();
                dt.TableName = "tbl_ApprovalSetup";
                dt = CreateTableColumns(dt);
                string sResult = string.Empty;
                if (Request.Cookies[Constants.Update].Value == string.Empty || Request.Cookies[Constants.Update].Value == "")
                {
                    sResult = oApprover.CreateApprover(dt);
                }
                else
                {
                    sResult = oApprover.UpdateApprover(dt);
                }
                if (sResult == Constants.Insert)
                {
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    ClearFields();
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Approval setup created successfully";
                    lblSuccess.Focus();
                }
                else if (sResult == Constants.Update)
                {
                    //Response.Cookies[Constants.Update].Expires = DateTime.Now;
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    ClearFields();
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Approval setup updated successfully";
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
                grvSearch.DataBind();
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void grvSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var var1 = e.NewPageIndex;
                var var2 = this.grvSearch.PageIndex;
                this.grvSearch.PageIndex = e.NewPageIndex;
                DataTable tblSTCS = (DataTable)ViewState["SearchResult"];
                grvSearch.DataSource = tblSTCS;
                grvSearch.DataBind();

            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void grvSearch_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                BindGridView(e.SortExpression, sortOrder);
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }

        }

        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvSearch, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grvSearch.Rows)
            {
                if (row.RowIndex == grvSearch.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#3FBBEE");
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                }
            }
            int index = grvSearch.SelectedRow.RowIndex;
            string id = grvSearch.SelectedRow.Cells[0].Text;
            string companycode = grvSearch.SelectedRow.Cells[1].Text;
            string approvercode = grvSearch.SelectedRow.Cells[3].Text;
            string date = grvSearch.SelectedRow.Cells[5].Text;

            ddlApprover.DataSource = oApprover.GetUsers(companycode);
            ddlApprover.DataTextField = "Name";
            ddlApprover.DataValueField = "Code";
            ddlApprover.DataBind();

            lblId.Text = id;
            ddlCompany.SelectedValue = companycode;
            ddlApprover.SelectedValue = approvercode;
            if (date != "&nbsp;")
            {
                txtApprovedMonth.Text = date;
            }
            else
            {
                txtApprovedMonth.Text = string.Empty;
            }

            Response.Cookies[Constants.Update].Value = "1";

            //string message = "Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
        }

        protected void ddlCompany_SelectedIndexChanging(object sender, EventArgs e)
        {
            try
            {
                ddlApprover.DataSource = oApprover.GetUsers(ddlCompany.SelectedValue);
                ddlApprover.DataTextField = "Name";
                ddlApprover.DataValueField = "Code";
                ddlApprover.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        public string sortOrder
        {
            get
            {
                if (ViewState["sortOrder"].ToString() == "desc")
                {
                    ViewState["sortOrder"] = "asc";
                }
                else
                {
                    ViewState["sortOrder"] = "desc";
                }

                return ViewState["sortOrder"].ToString();
            }
            set
            {
                ViewState["sortOrder"] = value;
            }
        }

        private void BindGridView(string sortExp, string sortDir)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["SearchResult"];

                if (dt.Rows.Count > 0)
                {
                    DataView dv = new DataView();
                    dv = dt.DefaultView;

                    if (sortExp != string.Empty)
                    {
                        dv.Sort = string.Format("{0} {1}", sortExp, sortDir);
                    }

                    grvSearch.DataSource = dv;
                    grvSearch.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataTable CreateTableColumns(DataTable dt)
        {
            try
            {
                dt.Columns.Add("Id");
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("CompanyName");
                dt.Columns.Add("UserCode");
                dt.Columns.Add("UserName");
                dt.Columns.Add("ApprovedMonth");


                dt.Rows.Add();
                dt.Rows[0]["Id"] = lblId.Text;
                dt.Rows[0]["CompanyCode"] = ddlCompany.SelectedValue;
                dt.Rows[0]["CompanyName"] = ddlCompany.SelectedItem;
                dt.Rows[0]["UserCode"] = ddlApprover.SelectedValue;
                dt.Rows[0]["UserName"] = ddlApprover.SelectedItem;
                dt.Rows[0]["ApprovedMonth"] = txtApprovedMonth.Text;

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ClearFields()
        {
            try
            {
                ddlApprover.SelectedIndex = 0;
                ddlCompany.SelectedIndex = 0;
                txtApprovedMonth.Text = string.Empty;
                txtCompanySearch.Text = string.Empty;
                grvSearch.DataBind();
                if (Response.Cookies[Constants.Update] != null)
                {
                    Response.Cookies[Constants.Update].Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

    }
}