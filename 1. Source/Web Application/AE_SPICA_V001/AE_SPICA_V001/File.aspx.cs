using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AE_SPICA.DAL;
using System.Drawing;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace AE_SPICA_V001
{
    public partial class File : System.Web.UI.Page
    {
        #region Objects
        public string strSelect = "-- Select --";
        public clsFileReference oFileReference = new clsFileReference();
        public clsClub oClub = new clsClub();
        public string sDateFormat = ConfigurationManager.AppSettings["DateFormat"].ToString();
        public string sDefaultDate = ConfigurationManager.AppSettings["DefaultDate"].ToString();
        public string sSQLFormat = ConfigurationManager.AppSettings["SQLFormat"].ToString();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies[Constants.ScreenName].Value = "Create File";
                if (!IsPostBack)
                {
                    GetDropdownValues();
                    ViewState["sortOrderTime"] = "";
                    ViewState["sortOrderExpense"] = "";
                    //LoadTimeandExpenseEntryDetails();
                    grvSearch.DataBind();
                    btnApproval.Enabled = false;
                    btnApproval.Style["background-image"] = "url('Images/bgButton_disable.png')";
                    btnApproval.Attributes.Add("class", "ScreenICEButton disabled");
                    btnPreview.Enabled = false;
                    btnPreview.Style["background-image"] = "url('Images/bgButton_disable.png')";
                    btnPreview.Attributes.Add("class", "ScreenICEButton disabled");
                    btnHoldingInv.Enabled = false;
                    btnHoldingInv.Style["background-image"] = "url('Images/bgButton_disable.png')";
                    btnHoldingInv.Attributes.Add("class", "ScreenICEButton disabled");

                    btnTimeSheet.Enabled = true;
                    btnTimeSheet.Style["background-image"] = "url('Images/bgButton_disable.png')";
                    btnTimeSheet.Attributes.Add("class", "ScreenICEButton disabled");

                    if (Response.Cookies[Constants.Update] != null)
                    {
                        Response.Cookies[Constants.Update].Value = string.Empty;
                    }

                    txtClaimHandler.Text = Request.Cookies[Constants.UserCode].Value.ToString();
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

        protected void OnRowDataBoundTime(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvTimeEntry, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void OnRowDataBoundExpense(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvExpenseEntry, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void grvSearch_SelectedIndexChanged(object sender, EventArgs e)
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

            DataSet ds = oFileReference.GetRecords(id);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblId.Text = id;
                    txtIncidentDate.Text = ds.Tables[0].Rows[0]["ConvIncidentDate"].ToString();
                    txtVessel.Text = ds.Tables[0].Rows[0]["Vessel"].ToString();
                    ddlClub.Text = ds.Tables[0].Rows[0]["ClubID"].ToString();
                    if (ds.Tables[0].Rows[0]["ClubBP"].ToString() != string.Empty)
                    {
                        txtClubBP.Text = ds.Tables[0].Rows[0]["ClubBP"].ToString();
                        string sSAPDBName = Convert.ToString(Request.Cookies[Constants.SAPDBName].Value);
                        GetClubBPContact(txtClubBP.Text.ToString());
                        ddlClubBPContact.Text = ds.Tables[0].Rows[0]["ClubBPContact"].ToString();
                    }
                    else
                    {
                        txtClubBP.Text = string.Empty;
                    }

                    txtMember.Text = ds.Tables[0].Rows[0]["Member"].ToString();
                    txtAddressL1.Text = ds.Tables[0].Rows[0]["MemberAdd1"].ToString();
                    txtAddressL2.Text = ds.Tables[0].Rows[0]["MemberAdd2"].ToString();
                    txtAddressL3.Text = ds.Tables[0].Rows[0]["MemberAdd3"].ToString();
                    txtAddressL4.Text = ds.Tables[0].Rows[0]["MemberAdd4"].ToString();
                    //ddlBillTo.SelectedValue = ds.Tables[0].Rows[0]["BillTo"].ToString();
                    //txtBillingAddress.Text = ds.Tables[0].Rows[0]["BillingAddress"].ToString();
                    //txtFileReference.Text = ds.Tables[0].Rows[0]["FileReferenceNo"].ToString();
                    txtClaimHandler.Text = ds.Tables[0].Rows[0]["ClaimHandler"].ToString();

                    txtYear.Text = ds.Tables[0].Rows[0]["year"].ToString();
                    txtClubReference.Text = ds.Tables[0].Rows[0]["ClubReference"].ToString();

                    txtCO.Text = ds.Tables[0].Rows[0]["CO"].ToString();
                    //txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtContactName.Text = ds.Tables[0].Rows[0]["ContactName"].ToString();
                    txtEmailId.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                    txtVATNo.Text = ds.Tables[0].Rows[0]["VATNumber"].ToString();
                    txtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                    txtPlaceofIncident.Text = ds.Tables[0].Rows[0]["IncidentPlace"].ToString();
                    ddlFileStatus.SelectedValue = ds.Tables[0].Rows[0]["FileStatus"].ToString();
                    txtPeriodDateFrom.Text = ds.Tables[0].Rows[0]["ConvPeriodDateFrom"].ToString();
                    txtPeriodDateTo.Text = ds.Tables[0].Rows[0]["ConvPeriodDateTo"].ToString();
                    //ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                    txtVoyageNumber.Text = ds.Tables[0].Rows[0]["VoyageNumber"].ToString();
                    txtCloseOn.Text = ds.Tables[0].Rows[0]["ConvCloseOn"].ToString();
                    txtLocationStore.Text = ds.Tables[0].Rows[0]["LocationStore"].ToString();
                    if (ds.Tables[0].Rows[0]["Status"].ToString() != null && ds.Tables[0].Rows[0]["Status"].ToString() != string.Empty)
                    {
                        ddlTypeofBill.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                    }
                    else
                    {
                        ddlTypeofBill.SelectedValue = strSelect.ToString();
                    }

                    if (ds.Tables[0].Rows[0]["LumpSum"].ToString() != null && ds.Tables[0].Rows[0]["LumpSum"].ToString() != string.Empty)
                    {
                        if (ds.Tables[0].Rows[0]["LumpSum"].ToString() == "Y")
                        {
                            chkLumpSum.Checked = true;
                        }
                        else
                        {
                            chkLumpSum.Checked = false;
                        }
                    }
                    else
                    {
                        chkLumpSum.Checked = false;
                    }
                    txtBillingDetails.Text = ds.Tables[0].Rows[0]["BillingDetails"].ToString();

                    LoadTimeandExpenseEntryDetails(ds.Tables[0].Rows[0]["FileReferenceNo"].ToString());

                    Response.Cookies[Constants.FileRefNo].Value = ds.Tables[0].Rows[0]["FileReferenceNo"].ToString();

                    //For Fetching the data from SAP OINV table and update it in Web database
                    string sCmpyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    DataSet dsBalance = oFileReference.GetInvBalanceandPaymentDate(sCmpyCode, ds.Tables[0].Rows[0]["FileReferenceNo"].ToString());
                    if (dsBalance != null && ds.Tables.Count != 0)
                    {
                        if (dsBalance.Tables[0].Rows.Count > 0)
                        {
                            lblTotalInvAmount.Text = dsBalance.Tables[0].Rows[0]["TotalInvAmt"].ToString();
                            lblBalance.Text = dsBalance.Tables[0].Rows[0]["Balance"].ToString();
                            lblLastRecNum.Text = dsBalance.Tables[0].Rows[0]["LastRecNum"].ToString();
                            if (dsBalance.Tables[0].Rows[0]["LastPaymentDate"].ToString() != string.Empty || dsBalance.Tables[0].Rows[0]["LastPaymentDate"].ToString() != null)
                            {
                                lblLastPaymentDate.Text = Convert.ToDateTime(dsBalance.Tables[0].Rows[0]["LastPaymentDate"]).ToString(sDateFormat);
                                if (lblLastPaymentDate.Text == sDefaultDate)
                                {
                                    lblLastPaymentDate.Text = string.Empty;
                                }
                            }
                            else
                            {
                                lblLastPaymentDate.Text = string.Empty;
                            }
                        }
                        else
                        {
                            lblTotalInvAmount.Text = string.Empty;
                            lblBalance.Text = string.Empty;
                            lblLastPaymentDate.Text = string.Empty;
                            lblLastRecNum.Text = string.Empty;
                        }
                    }
                    else
                    {
                        lblTotalInvAmount.Text = string.Empty;
                        lblBalance.Text = string.Empty;
                        lblLastPaymentDate.Text = string.Empty;
                    }
                }
            }



            Response.Cookies[Constants.Update].Value = "1";
            if (grvExpenseEntry.Rows.Count > 0 || grvTimeEntry.Rows.Count > 0)
            {
                btnApproval.Enabled = true;
                btnApproval.Style["background-image"] = "url('Images/bgButton.png')";
                btnApproval.Attributes.Add("class", "ScreenICEButton");
            }
            else
            {
                btnApproval.Enabled = false;
                btnApproval.Style["background-image"] = "url('Images/bgButton_disable.png')";
                btnApproval.Attributes.Add("class", "ScreenICEButton disabled");
            }
            btnPreview.Enabled = true;
            btnPreview.Style["background-image"] = "url('Images/bgButton.png')";
            btnPreview.Attributes.Add("class", "ScreenICEButton");

            btnHoldingInv.Enabled = true;
            btnHoldingInv.Style["background-image"] = "url('Images/bgButton.png')";
            btnHoldingInv.Attributes.Add("class", "ScreenICEButton");

            btnTimeSheet.Enabled = true;
            btnTimeSheet.Style["background-image"] = "url('Images/bgButton.png')";
            btnTimeSheet.Attributes.Add("class", "ScreenICEButton");
        }

        protected void grvClub_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvClub, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void grvClub_SelectedIndexChanged(object sender, EventArgs e)
        {
            //foreach (GridViewRow row in grvClub.Rows)
            //{
            //    if (row.RowIndex == grvClub.SelectedIndex)
            //    {
            //        row.BackColor = ColorTranslator.FromHtml("#3FBBEE");
            //    }
            //    else
            //    {
            //        row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            //    }
            //}
            int index = grvClub.SelectedRow.RowIndex;
            string id = grvClub.SelectedRow.Cells[0].Text;
            string country = grvClub.SelectedRow.Cells[1].Text;
            string city = grvClub.SelectedRow.Cells[2].Text;
            string name = grvClub.SelectedRow.Cells[3].Text;
            string code = grvClub.SelectedRow.Cells[4].Text;

            mpePrevClub.Hide();
            txtClubBP.Text = code.Trim();
            if (name.Contains("&#39;"))
            {
                txtCO.Text = name.Replace("&#39;", "\'");
            }
            else
            {
                txtCO.Text = name.Trim();
            }
            GetClubBPContact(code.Trim().ToString());
        }

        protected void grvClub_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var var1 = e.NewPageIndex;
                var var2 = this.grvClub.PageIndex;
                this.grvClub.PageIndex = e.NewPageIndex;
                DataTable dtResult = (DataTable)ViewState["BPDetails"];
                grvClub.DataSource = dtResult;
                grvClub.DataBind();
                mpePrevClub.Show();
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

        //protected void grvClub_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    BindClubGridView(e.SortExpression, sortOrder);
        //}

        protected void grvTimeEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var var1 = e.NewPageIndex;
                var var2 = this.grvTimeEntry.PageIndex;
                this.grvTimeEntry.PageIndex = e.NewPageIndex;
                DataSet dsResult = (DataSet)ViewState["TimeEntry"];
                grvTimeEntry.DataSource = dsResult;
                grvTimeEntry.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void grvExpenseEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var var1 = e.NewPageIndex;
                var var2 = this.grvExpenseEntry.PageIndex;
                this.grvExpenseEntry.PageIndex = e.NewPageIndex;
                DataSet dsResult = (DataSet)ViewState["ExpenseEntry"];
                grvExpenseEntry.DataSource = dsResult;
                grvExpenseEntry.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void grvTimeEntry_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                BindTimeGridView(e.SortExpression, sortOrderTime);
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }

        }

        protected void grvExpenseEntry_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                BindExpenseGridView(e.SortExpression, sortOrderExpense);
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }

        }

        protected void ddlClub_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlClub.Text != strSelect.ToString())
                {
                    string sSAPDBName = Convert.ToString(Request.Cookies[Constants.SAPDBName].Value);
                    string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    txtYear.Text = oFileReference.GetNumberingSeriesYear(sCompanyCode, ddlClub.SelectedValue);
                    txtClubBP.Text = oFileReference.GetDefaultClubBP(ddlClub.SelectedValue, sCompanyCode); //ddlTask.SelectedItem.ToString().Trim();
                    if (txtClubBP.Text != string.Empty || txtClubBP.Text != "")
                    {
                        DataSet dsBPName = oClub.GetBPName(sSAPDBName, txtClubBP.Text);
                        txtCO.Text = dsBPName.Tables[0].Rows[0]["Name"].ToString();
                        GetClubBPContact(txtClubBP.Text.ToString());
                    }
                }
                else
                {
                    txtYear.Text = string.Empty;
                    txtClubBP.Text = string.Empty;
                    txtCO.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void txtVessel_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtVessel.Text == string.Empty)
                {
                    txtMember.Text = string.Empty;
                }
                else
                {
                    txtMember.Text = "The master and owner of " + txtVessel.Text;
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnClubBP_Click(object sender, EventArgs e)
        {

            string sSAPDBName = Convert.ToString(Request.Cookies[Constants.SAPDBName].Value);
            DataSet ds = oClub.GetBPDetails(sSAPDBName);
            ViewState["BPDetails"] = ds.Tables[0];
            grvClub.DataSource = ds;
            grvClub.DataBind();
            mpePrevClub.Show();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            mpePrevClub.Hide();
        }

        protected void btnGridSearch_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["BPDetails"];
            // DataTable dt = ds.Tables[0];

            string sSearchText = txtGridSearch.Text.Trim();
            DataRow[] filteredRows = dt.Select("Country LIKE '%" + sSearchText + "%' OR City LIKE '%" + sSearchText + "%' OR Code LIKE '%" + sSearchText + "%' OR Name LIKE '%" + sSearchText + "%'");
            DataTable dtFilter = filteredRows.CopyToDataTable();

            ViewState["BPDetails"] = dtFilter;

            grvClub.DataSource = dtFilter;
            grvClub.DataBind();
            mpePrevClub.Show();
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

        public string sortOrderTime
        {
            get
            {
                if (ViewState["sortOrderTime"].ToString() == "desc")
                {
                    ViewState["sortOrderTime"] = "asc";
                }
                else
                {
                    ViewState["sortOrderTime"] = "desc";
                }

                return ViewState["sortOrderTime"].ToString();
            }
            set
            {
                ViewState["sortOrderTime"] = value;
            }
        }

        public string sortOrderExpense
        {
            get
            {
                if (ViewState["sortOrderExpense"].ToString() == "desc")
                {
                    ViewState["sortOrderExpense"] = "asc";
                }
                else
                {
                    ViewState["sortOrderExpense"] = "desc";
                }

                return ViewState["sortOrderExpense"].ToString();
            }
            set
            {
                ViewState["sortOrderExpense"] = value;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //if ((txtVessel.Text == string.Empty || txtVessel.Text == "") && (txtVessel.Text == string.Empty || txtVessel.Text == ""))
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly key in the data to search";
                //    grvSearch.DataBind();
                //}
                //else
                //{
                Response.Cookies[Constants.IsUpdate].Expires = DateTime.Now;
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                DataSet sResult = oFileReference.SearchFileReference(txtVesselSearch.Text, ddlClubSearch.SelectedValue, txtMemberSearch.Text, txtFileRefSearch.Text, txtClaimHandlerSearch.Text, txtYearSearch.Text, ddlFileStatusSearch.SelectedValue, sUserCode, sCompanyCode);
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

        protected void btnSave_Click(object sender, EventArgs e)
        {

            string sMessage = string.Empty;
            string sFileRefNo = string.Empty;
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                //if (txtIncidentDate.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the Incident Date";
                //    return;
                //}
                if (txtVessel.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Vessel";
                    return;
                }

                if (ddlClub.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the Club";
                    return;
                }
                //if (ddlBillTo.Text.ToString() == strSelect.ToString())
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly select the Bill To";
                //    return;
                //}
                //if (txtFileReference.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the File Reference";
                //    return;
                //}
                if (txtClaimHandler.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Claim Handler";
                    return;
                }
                if (txtYear.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Year";
                    return;
                }
                //if (txtClubReference.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the Club Reference";
                //    return;
                //}
                if (txtPeriodDateFrom.Text.ToString() == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Enter the Period DateFrom";
                    return;
                }
                //if (txtPeriodDateTo.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the Period DateTo";
                //    return;
                //}
                
                if (txtPeriodDateFrom.Text.ToString() != string.Empty && txtPeriodDateTo.Text.ToString() != string.Empty)
                {
                    DateTime dFromdate = DateTime.ParseExact(txtPeriodDateFrom.Text, sDateFormat, null);
                    DateTime dTodate = DateTime.ParseExact(txtPeriodDateTo.Text, sDateFormat, null);
                    //DateTime dFromdate = Convert.ToDateTime(txtPeriodDateFrom.Text);
                    //DateTime dTodate = Convert.ToDateTime(txtPeriodDateTo.Text);
                    if (dTodate < dFromdate)
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Peroid Date To should not be less than Period Date From";
                        return;
                    }
                }
                //if (txtMember.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the Member";
                //    return;
                //}
                //if (txtAddress.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the Address";
                //    return;
                //}
                //if (txtContactName.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the Contact Name";
                //    return;
                //}
                //if (txtEmailId.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the Email";
                //    return;
                //}
                //if (txtPlaceofIncident.Text.ToString() == string.Empty)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Enter the Incident place";
                //    return;
                //}


                //if (ddlStatus.Text.ToString() == strSelect.ToString())
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Kindly Select the Status";
                //    return;
                //}
                if (ddlFileStatus.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the File Status";
                    return;
                }
                if (txtEmailId.Text != string.Empty)
                {
                    var result = "@,.".Split(',');
                    foreach (var item in result)
                    {
                        if (!txtEmailId.Text.Contains(item))
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Kindly enter valid email";
                            return;
                        }
                    }
                }

                DataTable dt = new DataTable();
                dt.TableName = "tbl_FileReference";
                dt = CreateTableColumns(dt);
                string sResult = string.Empty;
                if (Request.Cookies[Constants.Update] == null)
                {
                    sResult = oFileReference.CreateFileReference(dt);
                }
                else
                {
                    if (Request.Cookies[Constants.Update].Value == string.Empty || Request.Cookies[Constants.Update].Value == "")
                    {
                        sResult = oFileReference.CreateFileReference(dt);
                    }
                    else
                    {
                        sResult = oFileReference.UpdateFileReference(dt);
                        Response.Cookies[Constants.Update].Value = string.Empty;
                    }
                }
                //The following part of code is to split the file reference Number from the Result and to show in the UI 
                if (sResult.Contains("|"))
                {
                    string[] sArray = sResult.Split('|');
                    sMessage = sArray[0].Trim();
                    sFileRefNo = sArray[1].Trim();
                }
                else
                {
                    sMessage = sResult;
                }
                if (sMessage == Constants.Insert)
                {
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    ClearFields();
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "File Reference Created successfully - " + sFileRefNo;
                    lblSuccess.Focus();
                }
                else if (sMessage == Constants.Update)
                {
                    //Response.Cookies[Constants.Update].Expires = DateTime.Now;
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    ClearFields();
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "File Reference updated successfully - " + sFileRefNo;
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
                //CrystalReportViewer1.Visible = false;
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PreviewBilling.aspx", false);
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnTimeSheet_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ViewTimeSheetReport.aspx", false);
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            try
            {
                string sLumpSum = string.Empty;
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                if (ddlTypeofBill.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the Type of Bill";
                    return;
                }

                //Here is to update the Bill Status and Approval Status in File Reference table
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                string sFileRefNo = Convert.ToString(Request.Cookies[Constants.FileRefNo].Value);

                if (chkLumpSum.Checked == true)
                {
                    sLumpSum = "Y";
                }
                else
                {
                    sLumpSum = "N";
                }

                string sResult = oFileReference.SendToApproval(sUserCode, sCompanyCode, ddlTypeofBill.SelectedValue, sFileRefNo, txtBillingDetails.Text.ToString(), sLumpSum);
                if (sResult == "SUCCESS")
                {
                    ClearFields();
                    LoadTimeandExpenseEntryDetails(sFileRefNo);
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Send for Approval successfully";
                    lblSuccess.Focus();
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnHoldingInv_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;

                //Here is to get the Holding Invoice details from the holding company
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                string sFileRefNo = Convert.ToString(Request.Cookies[Constants.FileRefNo].Value);

                DataSet sResult = oFileReference.ExtractHoldingInv(sCompanyCode, sFileRefNo, sSQLFormat);
                ViewState["ExtractHoldingInv"] = sResult.Tables[0];
                grvHoldingInv.DataSource = sResult;
                grvHoldingInv.DataBind();
                if (sResult != null && sResult.Tables.Count > 0)
                {
                    if (sResult.Tables[0].Rows.Count > 0)
                    {
                        btnHoldingInvPreview.Enabled = true;
                        btnHoldingInvPreview.Style["background-image"] = "url('Images/bgButton.png')";
                        btnHoldingInvPreview.Attributes.Add("class", "ScreenICEButton");
                    }
                    else
                    {
                        btnHoldingInvPreview.Enabled = false;
                        btnHoldingInvPreview.Style["background-image"] = "url('Images/bgButton_disable.png')";
                        btnHoldingInvPreview.Attributes.Add("class", "ScreenICEButton disabled");
                    }
                }
                mpeHoldingInv.Show();
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

        private void BindClubGridView(string sortExp, string sortDir)
        {
            DataTable dt = (DataTable)ViewState["BPDetails"];

            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView();
                dv = dt.DefaultView;

                if (sortExp != string.Empty)
                {
                    dv.Sort = string.Format("{0} {1}", sortExp, sortDir);
                }

                grvClub.DataSource = dv;
                grvClub.DataBind();
            }
        }

        private void BindTimeGridView(string sortExp, string sortDir)
        {
            DataSet dsResult = (DataSet)ViewState["TimeEntry"];
            DataTable dt = new DataTable();

            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                dt = dsResult.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    DataView dv = new DataView();
                    dv = dt.DefaultView;

                    if (sortExp != string.Empty)
                    {
                        dv.Sort = string.Format("{0} {1}", sortExp, sortDir);
                    }

                    grvTimeEntry.DataSource = dv;
                    grvTimeEntry.DataBind();
                }
            }
            else
            {
                grvTimeEntry.DataBind();
            }
        }

        private void BindExpenseGridView(string sortExp, string sortDir)
        {
            DataSet dsResult = (DataSet)ViewState["ExpenseEntry"];
            DataTable dt = new DataTable();

            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                dt = dsResult.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    DataView dv = new DataView();
                    dv = dt.DefaultView;

                    if (sortExp != string.Empty)
                    {
                        dv.Sort = string.Format("{0} {1}", sortExp, sortDir);
                    }

                    grvExpenseEntry.DataSource = dv;
                    grvExpenseEntry.DataBind();
                }
            }
        }

        public void GetDropdownValues()
        {
            string sCmpyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
            ddlClub.DataSource = oFileReference.GetClubDetails(sCmpyCode);
            ddlClub.DataTextField = "Name";
            ddlClub.DataValueField = "Code";
            ddlClub.DataBind();

            ddlClubSearch.DataSource = oFileReference.GetClubDetails(sCmpyCode);
            ddlClubSearch.DataTextField = "Name";
            ddlClubSearch.DataValueField = "Code";
            ddlClubSearch.DataBind();

            //string sSAPDBName = Convert.ToString(Request.Cookies[Constants.SAPDBName].Value);
            //ddlClubBP.DataSource = oClub.GetBPCode(sSAPDBName);
            //ddlClubBP.DataTextField = "Name";
            //ddlClubBP.DataValueField = "Code";
            //ddlClubBP.DataBind();

            ddlFileStatusSearch.Items.Insert(0, new ListItem("-- Select --", "-- Select --"));
            ddlFileStatusSearch.Items.Insert(1, new ListItem("OPEN", "OPEN"));
            ddlFileStatusSearch.Items.Insert(2, new ListItem("INACTIVE", "INACTIVE"));
            ddlFileStatusSearch.SelectedIndex = 0;

            ddlFileStatus.Items.Insert(0, new ListItem("-- Select --", "-- Select --"));
            ddlFileStatus.Items.Insert(1, new ListItem("OPEN", "OPEN"));
            ddlFileStatus.Items.Insert(2, new ListItem("INACTIVE", "INACTIVE"));
            ddlFileStatus.SelectedIndex = 1;

            //ddlStatus.Items.Insert(0, new ListItem("-- Select --", "-- Select --"));
            //ddlStatus.Items.Insert(1, new ListItem("INTERIM", "INTERIM"));
            //ddlStatus.Items.Insert(2, new ListItem("FINAL", "FINAL"));
            //ddlStatus.SelectedIndex = 0;

            //ddlBillTo.Items.Insert(0, new ListItem("-- Select --", "-- Select --"));
            //ddlBillTo.Items.Insert(1, new ListItem("CLUB", "CLUB"));
            //ddlBillTo.Items.Insert(2, new ListItem("MEMBER", "MEMBER"));
            //ddlBillTo.SelectedIndex = 1;

            ddlTypeofBill.Items.Insert(0, new ListItem("-- Select --", "-- Select --"));
            ddlTypeofBill.Items.Insert(1, new ListItem("INTERIM", "INTERIM"));
            ddlTypeofBill.Items.Insert(2, new ListItem("FINAL", "FINAL"));
            ddlTypeofBill.SelectedIndex = 0;
        }

        private DataTable CreateTableColumns(DataTable dt)
        {
            try
            {
                dt.Columns.Add("Id");
                dt.Columns.Add("IncidentDate");
                dt.Columns.Add("Vessel");
                dt.Columns.Add("ClubID");
                dt.Columns.Add("ClubBP");
                dt.Columns.Add("ClubBPContact");
                dt.Columns.Add("Member");
                dt.Columns.Add("MemberAdd1");
                dt.Columns.Add("MemberAdd2");
                dt.Columns.Add("MemberAdd3");
                dt.Columns.Add("MemberAdd4");
                //dt.Columns.Add("BillTo");
                //dt.Columns.Add("BillingAddress");
                dt.Columns.Add("FileReferenceNo");
                dt.Columns.Add("ClaimHandler");
                dt.Columns.Add("year");
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("ClubReference");
                dt.Columns.Add("CO");
                //dt.Columns.Add("Address");
                dt.Columns.Add("ContactName");
                dt.Columns.Add("Email");
                dt.Columns.Add("VATNumber");
                dt.Columns.Add("Description");
                dt.Columns.Add("IncidentPlace");
                dt.Columns.Add("FileStatus");
                //dt.Columns.Add("Status");
                dt.Columns.Add("UserCode");
                dt.Columns.Add("PeriodDateFrom");
                dt.Columns.Add("PeriodDateTo");
                dt.Columns.Add("VoyageNumber");
                dt.Columns.Add("CloseOn");
                dt.Columns.Add("LocationStore");

                dt.Rows.Add();
                dt.Rows[0]["Id"] = lblId.Text;
                DateTime dIncidentdate = DateTime.ParseExact(txtIncidentDate.Text, sDateFormat, null);
                dt.Rows[0]["IncidentDate"] = dIncidentdate;
                dt.Rows[0]["Vessel"] = txtVessel.Text;
                dt.Rows[0]["ClubID"] = ddlClub.SelectedValue;

                dt.Rows[0]["ClubBP"] = txtClubBP.Text;

                if (ddlClubBPContact.SelectedValue != strSelect.ToString())
                {
                    dt.Rows[0]["ClubBPContact"] = ddlClubBPContact.SelectedValue;
                }
                else
                {
                    dt.Rows[0]["ClubBPContact"] = string.Empty;
                }
                dt.Rows[0]["Member"] = txtMember.Text;
                dt.Rows[0]["MemberAdd1"] = txtAddressL1.Text;
                dt.Rows[0]["MemberAdd2"] = txtAddressL2.Text;
                dt.Rows[0]["MemberAdd3"] = txtAddressL3.Text;
                dt.Rows[0]["MemberAdd4"] = txtAddressL4.Text;
                //dt.Rows[0]["BillTo"] = ddlBillTo.SelectedValue;
                //dt.Rows[0]["BillingAddress"] = txtBillingAddress.Text;
                if (Request.Cookies[Constants.Update] != null)
                {
                    if (Request.Cookies[Constants.Update].Value != string.Empty || Request.Cookies[Constants.Update].Value != "")
                    {
                        string sFileRefNo = Convert.ToString(Request.Cookies[Constants.FileRefNo].Value);
                        dt.Rows[0]["FileReferenceNo"] = sFileRefNo;
                    }
                }

                //dt.Rows[0]["FileReferenceNo"] = txtFileReference.Text;
                dt.Rows[0]["ClaimHandler"] = txtClaimHandler.Text;
                dt.Rows[0]["year"] = txtYear.Text;
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                dt.Rows[0]["CompanyCode"] = sCompanyCode;
                dt.Rows[0]["ClubReference"] = txtClubReference.Text;
                dt.Rows[0]["CO"] = txtCO.Text;
                //dt.Rows[0]["Address"] = txtAddress.Text;
                dt.Rows[0]["ContactName"] = txtContactName.Text;
                dt.Rows[0]["Email"] = txtEmailId.Text;
                dt.Rows[0]["VATNumber"] = txtVATNo.Text;
                dt.Rows[0]["Description"] = txtDescription.Text;
                dt.Rows[0]["IncidentPlace"] = txtPlaceofIncident.Text;
                dt.Rows[0]["FileStatus"] = ddlFileStatus.SelectedValue;
                //dt.Rows[0]["Status"] = ddlStatus.SelectedValue;
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                dt.Rows[0]["UserCode"] = sUserCode;
                DateTime dPeriodDateFrom = DateTime.ParseExact(txtPeriodDateFrom.Text, sDateFormat, null);
                dt.Rows[0]["PeriodDateFrom"] = dPeriodDateFrom;

                DateTime? dPeriodDateTo = txtPeriodDateTo.Text == string.Empty ? (DateTime?)null : DateTime.ParseExact(txtPeriodDateTo.Text, sDateFormat, null);
                dt.Rows[0]["PeriodDateTo"] = dPeriodDateTo;
                dt.Rows[0]["VoyageNumber"] = txtVoyageNumber.Text;
                DateTime? dCloseOn = txtCloseOn.Text == string.Empty ? (DateTime?)null : DateTime.ParseExact(txtCloseOn.Text, sDateFormat, null);
                dt.Rows[0]["CloseOn"] = dCloseOn;
                dt.Rows[0]["LocationStore"] = txtLocationStore.Text;

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
                ddlFileStatusSearch.SelectedIndex = 0;
                ddlClub.SelectedIndex = 0;
                txtClubBP.Text = string.Empty;
                ddlClubBPContact.SelectedIndex = 0;
                //ddlStatus.SelectedIndex = 0;
                ddlFileStatus.SelectedIndex = 0;
                //ddlBillTo.SelectedIndex = 0;

                txtIncidentDate.Text = string.Empty;
                txtVessel.Text = string.Empty;
                txtMember.Text = string.Empty;
                txtAddressL1.Text = string.Empty;
                txtAddressL2.Text = string.Empty;
                txtAddressL3.Text = string.Empty;
                txtAddressL4.Text = string.Empty;
                //txtBillingAddress.Text = string.Empty;

                //txtFileReference.Text = string.Empty;
                txtClaimHandler.Text = string.Empty;
                txtYear.Text = string.Empty;
                txtClubReference.Text = string.Empty;
                txtCO.Text = string.Empty;
                //txtAddress.Text = string.Empty;

                txtContactName.Text = string.Empty;
                txtEmailId.Text = string.Empty;
                txtVATNo.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtPlaceofIncident.Text = string.Empty;

                txtPeriodDateFrom.Text = string.Empty;
                txtPeriodDateTo.Text = string.Empty;
                txtVoyageNumber.Text = string.Empty;
                txtCloseOn.Text = string.Empty;
                txtLocationStore.Text = string.Empty;
                txtBillingDetails.Text = string.Empty;

                grvSearch.DataBind();
                grvTimeEntry.DataBind();
                grvExpenseEntry.DataBind();

                lblBillAmt.Text = string.Empty;
                lblChargeableAmt.Text = string.Empty;
                lblGrandTotal.Text = string.Empty;

                lblLastRecNum.Text = string.Empty;
                lblLastPaymentDate.Text = string.Empty;
                lblBalance.Text = string.Empty;
                lblTotalInvAmount.Text = string.Empty;

                btnApproval.Enabled = false;
                btnApproval.Style["background-image"] = "url('Images/bgButton_disable.png')";
                btnApproval.Attributes.Add("class", "ScreenICEButton disabled");

                btnPreview.Enabled = false;
                btnPreview.Style["background-image"] = "url('Images/bgButton_disable.png')";
                btnPreview.Attributes.Add("class", "ScreenICEButton disabled");

                btnHoldingInv.Enabled = false;
                btnHoldingInv.Style["background-image"] = "url('Images/bgButton_disable.png')";
                btnHoldingInv.Attributes.Add("class", "ScreenICEButton disabled");

                btnTimeSheet.Enabled = false;
                btnTimeSheet.Style["background-image"] = "url('Images/bgButton_disable.png')";
                btnTimeSheet.Attributes.Add("class", "ScreenICEButton disabled");

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

        public void LoadTimeandExpenseEntryDetails(string sFileRefNo)
        {
            try
            {
                DataTable table = new DataTable();
                DataTable table1 = new DataTable();
                object sumObject = new object();
                object sumObject1 = new object();
                DataSet dsTimeEntry = new DataSet();
                DataSet dsExpenseEntry = new DataSet();
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                dsTimeEntry = oFileReference.GetTimeEntryRecords(sCompanyCode, sUserCode, sFileRefNo);
                dsExpenseEntry = oFileReference.GetExpenseEntryRecords(sCompanyCode, sUserCode, sFileRefNo);

                grvTimeEntry.DataSource = dsTimeEntry;
                grvTimeEntry.DataBind();
                if (dsTimeEntry != null && dsTimeEntry.Tables.Count > 0)
                {
                    table = dsTimeEntry.Tables[0];
                    sumObject = table.Compute("Sum(BillAmount)", "");
                }
                else
                {
                    sumObject = 0;
                }
                ViewState["TimeEntry"] = dsTimeEntry;
                lblBillAmt.Text = sumObject.ToString();

                grvExpenseEntry.DataSource = dsExpenseEntry;
                grvExpenseEntry.DataBind();
                if (dsExpenseEntry != null && dsExpenseEntry.Tables.Count > 0)
                {
                    table1 = dsExpenseEntry.Tables[0];
                    sumObject1 = table1.Compute("Sum(ChargableAmt)", "");
                }
                else
                {
                    sumObject1 = 0;
                }
                ViewState["ExpenseEntry"] = dsExpenseEntry;
                lblChargeableAmt.Text = sumObject1.ToString();
                lblGrandTotal.Text = Convert.ToString(Convert.ToDecimal(lblBillAmt.Text == "" ? "0" : lblBillAmt.Text) + Convert.ToDecimal(lblChargeableAmt.Text == "" ? "0" : lblChargeableAmt.Text));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetClubBPContact(string sBPCode)
        {
            string sSAPDBName = Convert.ToString(Request.Cookies[Constants.SAPDBName].Value);
            DataSet ds = oFileReference.GetBPContact(sSAPDBName, sBPCode);
            ddlClubBPContact.DataSource = ds;
            ddlClubBPContact.DataTextField = "BPContact";
            ddlClubBPContact.DataValueField = "BPContact";
            ddlClubBPContact.DataBind();

            string sDefaultBPContact = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["DefaultBPContact"].ToString();
            if (sDefaultBPContact == "" || sDefaultBPContact == strSelect.ToString())
            {
                ddlClubBPContact.SelectedValue = strSelect.ToString();
            }
            else
            {
                ddlClubBPContact.SelectedValue = sDefaultBPContact;
            }
        }

        protected void grvHoldingInv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var var1 = e.NewPageIndex;
                var var2 = this.grvClub.PageIndex;
                this.grvHoldingInv.PageIndex = e.NewPageIndex;
                DataTable dtResult = (DataTable)ViewState["ExtractHoldingInv"];
                grvHoldingInv.DataSource = dtResult;
                grvHoldingInv.DataBind();
                mpeHoldingInv.Show();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void grvHoldingInv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in grvHoldingInv.Rows)
                {
                    if (row.RowIndex == grvHoldingInv.SelectedIndex)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#3FBBEE");
                    }
                    else
                    {
                        row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    }
                }

                string docEntry = grvHoldingInv.SelectedRow.Cells[2].Text;
                string type = grvHoldingInv.SelectedRow.Cells[0].Text;
                mpeHoldingInv.Show();
                Response.Cookies[Constants.HoldingInvDocEntry].Value = docEntry;
                Response.Cookies[Constants.HoldingInvType].Value = type;
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void grvHoldingInv_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvHoldingInv, "Select$" + e.Row.RowIndex);
                    e.Row.Attributes["style"] = "cursor:pointer";
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }

        }

        protected void btnHoldingInvPreview_Click(object sender, EventArgs e)
        {
            try
            {
                mpeHoldingInv.Hide();
                Response.Redirect("PreviewHoldingInv.aspx", false);
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnHoldingInvClose_Click(object sender, EventArgs e)
        {
            try
            {
                mpeHoldingInv.Hide();
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