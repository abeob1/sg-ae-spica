using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AE_SPICA.DAL;
using System.Drawing;
using System.Data;
using System.Configuration;

namespace AE_SPICA_V001
{
    public partial class TimeEntry : System.Web.UI.Page
    {
        #region Objects
        public string strSelect = "-- Select --";
        public clsTimeEntry oTimeEntry = new clsTimeEntry();
        public clsFileReference oFileReference = new clsFileReference();
        public string sDateFormat = ConfigurationManager.AppSettings["DateFormat"].ToString();
        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies[Constants.ScreenName].Value = "Time Entry";
                if (!IsPostBack)
                {
                    Response.Cookies[Constants.Update].Value = string.Empty;
                    ViewState["Time"] = string.Empty;
                    ViewState["BillableUnit"] = string.Empty;
                    GetDropdownValues();
                    btnSubmit.Enabled = false;
                    grvSearch.DataBind();
                    string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                    string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    DataSet ds = oTimeEntry.GetRecords(sUserCode, sCompanyCode);
                    DataSet dsBillingConfigDetails = oTimeEntry.GetBillingConfigurationDetails(sUserCode, sCompanyCode);
                    if (dsBillingConfigDetails != null && dsBillingConfigDetails.Tables.Count > 0)
                    {
                        if (dsBillingConfigDetails.Tables[0].Rows.Count > 0)
                        {
                            ViewState["Time"] = dsBillingConfigDetails.Tables[0].Rows[0]["Time"].ToString();
                            ViewState["BillableUnit"] = dsBillingConfigDetails.Tables[0].Rows[0]["BillableUnit"].ToString();
                            ViewState["HourlyRate"] = dsBillingConfigDetails.Tables[0].Rows[0]["HourlyRate"].ToString();
                            btnSeach.Enabled = true;
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Billing Configuration is not Done for this company. Kindly Configure and Retry.";
                            btnSeach.Enabled = false;
                            return;
                        }
                    }
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["Records"] = ds.Tables[0];
                        ViewState["FilteredRecords"] = ds.Tables[0];
                        grvTimeEntry.DataSource = ds.Tables[0];
                        grvTimeEntry.DataBind();
                    }
                    else
                    {
                        grvTimeEntry.DataBind();
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

                int index = grvSearch.SelectedRow.RowIndex;
                string id = grvSearch.SelectedRow.Cells[0].Text;
                //if (grvSearch.SelectedRow.Cells[3].Text != "&nbsp;")
                //{
                //    txtTimeEntryDate.Text = grvSearch.SelectedRow.Cells[3].Text;
                //}
                //else
                //{
                //    txtTimeEntryDate.Text = string.Empty;
                //}
                lblId.Text = id.ToString();
                txtTimeEntryDate.Text = DateTime.Now.Date.ToString(sDateFormat);
                txtFileReference.Text = grvSearch.SelectedRow.Cells[1].Text;

                //
                if (ViewState["Records"] != null)
                {
                    DataTable tblRecords = (DataTable)ViewState["Records"];

                    DataView dv1 = (DataView)tblRecords.DefaultView;
                    dv1.RowFilter = "FileReference = '" + grvSearch.SelectedRow.Cells[1].Text + "'";
                    DataTable dt1 = dv1.ToTable();
                    ViewState["FilteredRecords"] = dt1;
                    grvTimeEntry.DataSource = dt1;
                    grvTimeEntry.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void ddlTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTask.Text != strSelect.ToString())
                {
                    txtDescription.Text = ddlTask.SelectedItem.ToString().Trim();
                }

                if (ddlTask.Text == "Others")
                {
                    txtDescription.Text = string.Empty;
                }
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

        protected void grvTimeEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var var1 = e.NewPageIndex;
                var var2 = this.grvTimeEntry.PageIndex;
                this.grvTimeEntry.PageIndex = e.NewPageIndex;

                DataTable tblRecords = (DataTable)ViewState["FilteredRecords"];
                grvTimeEntry.DataSource = tblRecords;
                grvTimeEntry.DataBind();
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
                //ClearFields();
                Response.Cookies[Constants.IsUpdate].Expires = DateTime.Now;
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                DataSet sResult = oTimeEntry.SearchFileReference(txtVessel.Text, ddlClubSearch.SelectedValue, txtMember.Text, txtFileRefSearch.Text, txtClaimHandlerSearch.Text, txtYearSearch.Text, sUserCode, sCompanyCode);
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

                if (txtTimeEntryDate.Text.ToString() == string.Empty)
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
                if (ddlTask.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly select the Task";
                    return;
                }

                if (txtDescription.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Task Description";
                    return;
                }
                if (txtDuration.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Duration";
                    return;
                }
                if (txtBillableUnit.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Billable Unit";
                    return;
                }

                DataTable dt = new DataTable();
                dt.TableName = "tbl_TimeEntry";
                dt = CreateTableColumns(dt);
                string sResult = string.Empty;
                if (Request.Cookies[Constants.Update] == null)
                {
                    sResult = oTimeEntry.CreateTimeEntry(dt);
                }
                else
                {
                    if (Request.Cookies[Constants.Update].Value == string.Empty || Request.Cookies[Constants.Update].Value == "")
                    {
                        sResult = oTimeEntry.CreateTimeEntry(dt);
                    }
                    else
                    {
                        sResult = oTimeEntry.UpdateTimeEntry(dt);
                    }
                }
                if (sResult == Constants.Insert)
                {
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    
                    ClearFields();
                    string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                    string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    DataSet ds = oTimeEntry.GetRecords(sUserCode, sCompanyCode);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["Records"] = ds.Tables[0];
                        ViewState["FilteredRecords"] = ds.Tables[0];
                        grvTimeEntry.DataSource = ds.Tables[0];
                        grvTimeEntry.DataBind();
                    }
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Time Entry Created successfully";
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
                    DataSet ds = oTimeEntry.GetRecords(sUserCode, sCompanyCode);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["Records"] = ds.Tables[0];
                        ViewState["FilteredRecords"] = ds.Tables[0];
                        grvTimeEntry.DataSource = ds.Tables[0];
                        grvTimeEntry.DataBind();
                    }
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Time Entry updated successfully";
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
                grvSearch.DataBind();
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
                Label lblTask = (Label)grdrow.FindControl("lblTask");
                Label lblDescription = (Label)grdrow.FindControl("lblDescription");
                Label lblDuration = (Label)grdrow.FindControl("lblDuration");
                Label lblBillableUnit = (Label)grdrow.FindControl("lblBillableUnit");
                Label lblBillAmount = (Label)grdrow.FindControl("lblBillableAmount");
                Label lblPrivateRemarks = (Label)grdrow.FindControl("lblPrivateRemarks");

                DataTable dt = (DataTable)ViewState["TaskDetails"];

                lblId.Text = lblID.Text;
                txtTimeEntryDate.Text = lblDate.Text;
                txtFileReference.Text = lblFileReference.Text;
                string find = "Name = '" + lblTask.Text.Trim() + "'";
                DataRow[] foundRows = dt.Select(find);
                if (foundRows.Count() > 0)
                {
                    ddlTask.SelectedValue = foundRows[0].ItemArray[0].ToString();
                }
                txtDescription.Text = lblDescription.Text;
                txtDuration.Text = lblDuration.Text;
                txtBillableUnit.Text = lblBillableUnit.Text;
                txtBillAmount.Text = lblBillAmount.Text;
                txtPrivateRemarks.Text = lblPrivateRemarks.Text;
                Response.Cookies[Constants.Update].Value = "1";
                btnSubmit.Enabled = true;
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

                string sResult = oTimeEntry.DeleteTimeEntry(lblID.Text);

                if (sResult == Constants.Delete)
                {
                    //Response.Cookies[Constants.Update].Expires = DateTime.Now;
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    
                    ClearFields();
                    string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                    string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    DataSet ds = oTimeEntry.GetRecords(sUserCode, sCompanyCode);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["Records"] = ds.Tables[0];
                        ViewState["FilteredRecords"] = ds.Tables[0];
                        grvTimeEntry.DataSource = ds.Tables[0];
                        grvTimeEntry.DataBind();
                    }
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Time Entry Deleted successfully";
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

        protected void txtDuration_onTextChanged(object sender, EventArgs e)
        {
            try
            {
                int calcBillableUnit = 0;
                int time = Convert.ToInt32(ViewState["Time"]);
                int BillableUnit = Convert.ToInt32(ViewState["BillableUnit"]);
                decimal result = Convert.ToDecimal(txtDuration.Text) / Convert.ToDecimal(time);
                int DurationResult = (int)Math.Ceiling(result);

                if (DurationResult > BillableUnit)
                {
                    calcBillableUnit = DurationResult * BillableUnit;
                }
                else
                {
                    calcBillableUnit = BillableUnit;
                }
                txtBillableUnit.Text = calcBillableUnit.ToString();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void txtBillableUnit_onTextChanged(object sender, EventArgs e)
        {
            try
            {
                int calcDuration = 0;
                int time = Convert.ToInt32(ViewState["Time"]);
                int BillableUnit = Convert.ToInt32(ViewState["BillableUnit"]);
                int resultForOneUnit =  time/ BillableUnit;
                calcDuration = resultForOneUnit * Convert.ToInt32(txtBillableUnit.Text);

                txtDuration.Text = calcDuration.ToString();
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

        public void GetDropdownValues()
        {
            string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
            ddlClubSearch.DataSource = oTimeEntry.GetClubDetails(sCompanyCode);
            ddlClubSearch.DataTextField = "Name";
            ddlClubSearch.DataValueField = "Code";
            ddlClubSearch.DataBind();

            DataSet ds = oTimeEntry.GetTaskDetails();
            ddlTask.DataSource = ds;
            ViewState["TaskDetails"] = ds.Tables[0];
            ddlTask.DataTextField = "Name";
            ddlTask.DataValueField = "Code";
            ddlTask.DataBind();
        }

        private DataTable CreateTableColumns(DataTable dt)
        {
            try
            {
                dt.Columns.Add("Id");
                dt.Columns.Add("Date");
                dt.Columns.Add("FileReference");
                dt.Columns.Add("Task");
                dt.Columns.Add("Description");
                dt.Columns.Add("Duration");
                dt.Columns.Add("BillableUnit");
                dt.Columns.Add("BillAmount");
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("UserCode");
                dt.Columns.Add("PrivateRemarks");
                dt.Columns.Add("ApprovalStatus");

                dt.Rows.Add();
                dt.Rows[0]["Id"] = lblId.Text;
                dt.Rows[0]["Date"] = txtTimeEntryDate.Text;
                dt.Rows[0]["FileReference"] = txtFileReference.Text;
                dt.Rows[0]["Task"] = ddlTask.SelectedItem.ToString();
                dt.Rows[0]["Description"] = txtDescription.Text;
                dt.Rows[0]["Duration"] = txtDuration.Text;
                dt.Rows[0]["BillableUnit"] = txtBillableUnit.Text;
                //To calculate the Bill Amount , the Formula is 
                // BillingRateConfiguration table (hourly rate/60) * (time entry table duration/duration in configuration table)

                double result = Convert.ToDouble(txtDuration.Text) / Convert.ToDouble(ViewState["Time"]);
                int DurationResult = (int)Math.Ceiling(result);
                double dResult = (Convert.ToDouble(ViewState["HourlyRate"]) / 10) * Convert.ToDouble(DurationResult);
                dt.Rows[0]["BillAmount"] = dResult.ToString();
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                dt.Rows[0]["CompanyCode"] = sCompanyCode;
                dt.Rows[0]["UserCode"] = sUserCode;
                dt.Rows[0]["PrivateRemarks"] = txtPrivateRemarks.Text;
                dt.Rows[0]["ApprovalStatus"] = "NOT YET BILLED";

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
                ddlTask.SelectedIndex = 0;

                txtTimeEntryDate.Text = string.Empty;
                txtFileReference.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtDuration.Text = string.Empty;
                txtBillableUnit.Text = string.Empty;
                txtBillAmount.Text = string.Empty;

                txtVessel.Text = string.Empty;
                txtMember.Text = string.Empty;
                txtFileRefSearch.Text = string.Empty;
                txtClaimHandlerSearch.Text = string.Empty;
                txtYearSearch.Text = string.Empty;
                txtPrivateRemarks.Text = string.Empty;

                //grvSearch.DataBind();
                grvTimeEntry.DataBind();
                btnSubmit.Enabled = false;
                if (Response.Cookies[Constants.Update] != null)
                {
                    Response.Cookies[Constants.Update].Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }

        }

        #endregion

    }
}