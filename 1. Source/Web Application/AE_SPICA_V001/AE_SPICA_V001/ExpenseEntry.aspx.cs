using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AE_SPICA.DAL;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.IO;

namespace AE_SPICA_V001
{
    public partial class ExpenseEntry : System.Web.UI.Page
    {
        #region Objects
        public string strSelect = "-- Select --";
        public clsExpenseEntry oExpenseEntry = new clsExpenseEntry();
        public clsFileReference oFileReference = new clsFileReference();
        public clsBillingRate oBillingRate = new clsBillingRate();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies[Constants.ScreenName].Value = "Expense Entry";
                if (!IsPostBack)
                {
                    ClearFields();
                    GetDropdownValues();
                    btnSubmit.Enabled = false;
                    btnSubmit.Style["background-image"] = "url('Images/bgButton_disable.png')";
                    btnSubmit.Attributes.Add("class", "ScreenICEButton disabled");
                    grvSearch.DataBind();

                    if (Request.Cookies[Constants.UserCode] != null)
                    {
                        string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                        string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                        txtCurrency.Text = oBillingRate.GetCurrency(sCompanyCode);
                        DataSet ds = oExpenseEntry.GetRecords(sUserCode, sCompanyCode);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            ViewState["Records"] = ds.Tables[0];
                            grvExpenseEntry.DataSource = ds.Tables[0];
                            grvExpenseEntry.DataBind();
                        }
                        else
                        {
                            grvExpenseEntry.DataBind();
                        }
                    }
                    else
                    {
                        Response.Redirect("Login.aspx", false);
                    }
                }
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
            try
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
                btnSubmit.Enabled = true;
                btnSubmit.Style["background-image"] = "url('Images/bgButton.png')";
                btnSubmit.Attributes.Add("class", "ScreenICEButton");

                int index = grvSearch.SelectedRow.RowIndex;
                string id = grvSearch.SelectedRow.Cells[0].Text;
                if (grvSearch.SelectedRow.Cells[3].Text != "&nbsp;")
                {
                    txtExpenseEntryDate.Text = grvSearch.SelectedRow.Cells[3].Text;
                }
                else
                {
                    txtExpenseEntryDate.Text = string.Empty;
                }
                txtFileReference.Text = grvSearch.SelectedRow.Cells[1].Text;

                //
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void grvSearch_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindGridView(e.SortExpression, sortOrder);
        }

        protected void grvExpenseEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var var1 = e.NewPageIndex;
                var var2 = this.grvExpenseEntry.PageIndex;
                this.grvExpenseEntry.PageIndex = e.NewPageIndex;

                DataTable tblRecords = (DataTable)ViewState["Records"];
                grvExpenseEntry.DataSource = tblRecords;
                grvExpenseEntry.DataBind();
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
                Response.Cookies[Constants.IsUpdate].Expires = DateTime.Now;
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                DataSet sResult = oExpenseEntry.SearchFileReference(txtVessel.Text, ddlClubSearch.SelectedValue, txtMember.Text, txtFileRefSearch.Text, txtClaimHandlerSearch.Text, txtYearSearch.Text, sUserCode, sCompanyCode);
                if (sResult != null && sResult.Tables[0].Rows.Count > 0)
                {
                    ViewState["SearchResult"] = sResult.Tables[0];
                    ViewState["sortOrder"] = "";
                    BindGridView("", "");
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Search result doesnot exist";
                    grvSearch.DataBind();
                }
                //}
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;

                if (txtExpenseEntryDate.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Date";
                    return;
                }

                //if (txtFileReference.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the File Reference";
                //    return;
                //}
                if (ddlExpense.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly select the Expenses";
                    return;
                }

                if (txtChargeableAmount.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Chargeable Amount";
                    return;
                }
                if (txtCurrency.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Currency";
                    return;
                }
                //if (txtRemarks.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the Remarks";
                //    return;
                //}

                DataTable dt = new DataTable();
                dt.TableName = "tbl_ExpenseEntry";
                dt = CreateTableColumns(dt);
                string sResult = string.Empty;
                if (Request.Cookies[Constants.Update] == null)
                {
                    sResult = oExpenseEntry.CreateExpenseEntry(dt);
                }
                else
                {
                    if (Request.Cookies[Constants.Update].Value == string.Empty || Request.Cookies[Constants.Update].Value == "")
                    {
                        sResult = oExpenseEntry.CreateExpenseEntry(dt);
                    }
                    else
                    {
                        sResult = oExpenseEntry.UpdateExpenseEntry(dt);
                    }
                }
                if (sResult == Constants.Insert)
                {
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;

                    ClearFields();
                    string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                    string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    DataSet ds = oExpenseEntry.GetRecords(sUserCode, sCompanyCode);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["Records"] = ds.Tables[0];
                        grvExpenseEntry.DataSource = ds.Tables[0];
                        grvExpenseEntry.DataBind();
                    }
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Expense Entry Created successfully";
                    lblSuccess.Focus();
                }
                else if (sResult == Constants.Update)
                {
                    //Response.Cookies[Constants.Update].Expires = DateTime.Now;
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;

                    ClearFields();
                    string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                    string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    DataSet ds = oExpenseEntry.GetRecords(sUserCode, sCompanyCode);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["Records"] = ds.Tables[0];
                        grvExpenseEntry.DataSource = ds.Tables[0];
                        grvExpenseEntry.DataBind();
                    }
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Expense Entry updated successfully";
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
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                ClearFields();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow grdrow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Label lblID = (Label)grdrow.FindControl("lblId");
                Label lblDate = (Label)grdrow.FindControl("lblDate");
                Label lblFileReference = (Label)grdrow.FindControl("lblFileReference");
                Label lblExpense = (Label)grdrow.FindControl("lblExpense");
                Label lblChargeableAmount = (Label)grdrow.FindControl("lblChargeableAmount");
                Label lblCurrency = (Label)grdrow.FindControl("lblCurrency");
                Label lblRemarks = (Label)grdrow.FindControl("lblRemarks");
                Label lblAttachment = (Label)grdrow.FindControl("lblAttachment");
                if (lblAttachment.Text != string.Empty)
                {
                    uploadedFile.HRef = ResolveUrl("~/Uploads/") + lblAttachment.Text;
                    uploadedFile.InnerText = lblAttachment.Text;
                }

                DataTable dt = (DataTable)ViewState["ExpenseDetails"];

                lblId.Text = lblID.Text;
                txtExpenseEntryDate.Text = lblDate.Text;
                txtFileReference.Text = lblFileReference.Text;

                string find = "Name = '" + lblExpense.Text.Trim() + "'";
                DataRow[] foundRows = dt.Select(find);
                if (foundRows.Count() > 0)
                {
                    ddlExpense.SelectedValue = foundRows[0].ItemArray[0].ToString();
                }

                txtChargeableAmount.Text = lblChargeableAmount.Text;
                txtCurrency.Text = lblCurrency.Text;
                txtRemarks.Text = lblRemarks.Text;
                Response.Cookies[Constants.Update].Value = "1";
                btnSubmit.Enabled = true;
                btnSubmit.Style["background-image"] = "url('Images/bgButton.png')";
                btnSubmit.Attributes.Add("class", "ScreenICEButton");
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }

        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow grdrow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Label lblID = (Label)grdrow.FindControl("lblId");

                string sResult = oExpenseEntry.DeleteExpenseEntry(lblID.Text);

                if (sResult == Constants.Delete)
                {
                    //Response.Cookies[Constants.Update].Expires = DateTime.Now;
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;

                    ClearFields();
                    string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                    string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    DataSet ds = oExpenseEntry.GetRecords(sUserCode, sCompanyCode);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["Records"] = ds.Tables[0];
                        grvExpenseEntry.DataSource = ds.Tables[0];
                        grvExpenseEntry.DataBind();
                    }
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Expense Entry Deleted successfully";
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                ViewState["IsClicked"] = "0";
                // Read the file and convert it to Byte Array
                string filePath = fileUpload.PostedFile.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                string contenttype = String.Empty;

                //Set the contenttype based on File Extension
                switch (ext)
                {
                    case ".jpg":
                        contenttype = "image/jpg";
                        break;
                    case ".png":
                        contenttype = "image/png";
                        break;
                    case ".gif":
                        contenttype = "image/gif";
                        break;
                    case ".pdf":
                        contenttype = "application/pdf";
                        break;
                }
                if (contenttype != String.Empty)
                {
                    string sTimeStampValue = string.Empty;
                    sTimeStampValue = MyExtensions.AppendTimeStamp(filename);
                    string sPathToSave = Server.MapPath("~/Uploads/") + sTimeStampValue;
                    fileUpload.PostedFile.SaveAs(sPathToSave);
                    ViewState["PathToSave"] = sPathToSave;
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "File Uploaded successfully.Now click SUBMIT button.";
                    ViewState["IsClicked"] = "1";
                    lblSuccess.Focus();
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "File format not recognised." +
                      " Upload Image/PDF formats";
                    ViewState["IsClicked"] = "0";
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        private void BindGridView(string sortExp, string sortDir)
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

        public void GetDropdownValues()
        {
            ddlClubSearch.DataSource = oExpenseEntry.GetClubDetails();
            ddlClubSearch.DataTextField = "Name";
            ddlClubSearch.DataValueField = "Code";
            ddlClubSearch.DataBind();

            DataSet ds = oExpenseEntry.GetExpenseDetails();
            ddlExpense.DataSource = ds;
            ViewState["ExpenseDetails"] = ds.Tables[0];
            ddlExpense.DataTextField = "Name";
            ddlExpense.DataValueField = "Code";
            ddlExpense.DataBind();
        }

        private DataTable CreateTableColumns(DataTable dt)
        {
            try
            {
                dt.Columns.Add("Id");
                dt.Columns.Add("Date");
                dt.Columns.Add("FileReference");
                dt.Columns.Add("Expense");
                dt.Columns.Add("ChargableAmt");
                dt.Columns.Add("Currency");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("Attachment");
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("UserCode");

                dt.Rows.Add();
                dt.Rows[0]["Id"] = lblId.Text;
                dt.Rows[0]["Date"] = txtExpenseEntryDate.Text;
                dt.Rows[0]["FileReference"] = txtFileReference.Text;
                dt.Rows[0]["Expense"] = ddlExpense.SelectedItem.ToString();
                dt.Rows[0]["ChargableAmt"] = txtChargeableAmount.Text;
                dt.Rows[0]["Currency"] = txtCurrency.Text;
                dt.Rows[0]["Remarks"] = txtRemarks.Text;

                if (ViewState["PathToSave"] != null)
                {
                    dt.Rows[0]["Attachment"] = ViewState["PathToSave"].ToString();
                }
                else
                {
                    dt.Rows[0]["Attachment"] = string.Empty;
                }

                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                dt.Rows[0]["CompanyCode"] = sCompanyCode;
                dt.Rows[0]["UserCode"] = sUserCode;

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
                ddlClubSearch.SelectedIndex = 0;
                ddlExpense.SelectedIndex = 0;

                txtExpenseEntryDate.Text = string.Empty;
                txtFileReference.Text = string.Empty;
                txtChargeableAmount.Text = string.Empty;
                //txtCurrency.Text = string.Empty;
                txtRemarks.Text = string.Empty;

                txtVessel.Text = string.Empty;
                txtMember.Text = string.Empty;
                txtFileRefSearch.Text = string.Empty;
                txtClaimHandlerSearch.Text = string.Empty;
                txtYearSearch.Text = string.Empty;

                grvSearch.DataBind();
                grvExpenseEntry.DataBind();
                btnSubmit.Enabled = false;
                btnSubmit.Style["background-image"] = "url('Images/bgButton_disable.png')";
                btnSubmit.Attributes.Add("class", "ScreenICEButton disabled");
                if (Response.Cookies[Constants.Update] != null)
                {
                    Response.Cookies[Constants.Update].Value = string.Empty;
                }
                uploadedFile.HRef = string.Empty;
                uploadedFile.InnerText = string.Empty;
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }

        }
    }
}