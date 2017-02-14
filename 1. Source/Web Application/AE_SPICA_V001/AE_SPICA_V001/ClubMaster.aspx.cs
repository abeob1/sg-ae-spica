using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AE_SPICA.DAL;
using System.Drawing;
using System.IO;

namespace AE_SPICA_V001
{
    public partial class ClubMaster : System.Web.UI.Page
    {
        #region Objects
        public clsClub oClub = new clsClub();
        public string strSelect = "-- Select --";

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                Response.Cookies[Constants.ScreenName].Value = "Club Master";
                Response.Cookies[Constants.Update].Value = string.Empty;
                if (!IsPostBack)
                {
                    grvSearch.DataBind();
                    string sSAPDBName = Convert.ToString(Request.Cookies[Constants.SAPDBName].Value);
                    DataSet ds = oClub.GetBPCode(sSAPDBName);
                    ddlClubBP.DataSource = ds;
                    ddlClubBP.DataTextField = "Name";
                    ddlClubBP.DataValueField = "Code";
                    ddlClubBP.DataBind();
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
                //if (txtClubSearch.Text == string.Empty || txtClubSearch.Text == "")
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
                string sCmpyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                DataSet sResult = oClub.SearchClub(txtClubSearch.Text, sCmpyCode);
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
                    grvSearch.DataBind();
                    lblerror.Visible = true;
                    lblerror.Text = "Search result doesnot exist";
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
            try
            {
                lblerror.Text = string.Empty;
                lblSuccess.Text = string.Empty;
                if (txtClubCode.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the Club Code";
                    return;
                }
                if (txtClubName.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly enter the Club Name";
                    return;
                }
                if (ddlClubBP.Text.ToString() == strSelect.ToString())
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select the Club BP";
                    return;
                }

                DataTable dt = new DataTable();
                dt.TableName = "tbl_Club";
                dt = CreateTableColumns(dt);
                string sResult = string.Empty;
                if (Request.Cookies[Constants.Update].Value == string.Empty || Request.Cookies[Constants.Update].Value == "")
                {
                    string sCmpyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                    sResult = oClub.CreateClub(dt, sCmpyCode);
                }
                else
                {
                    sResult = oClub.UpdateClub(dt);
                }
                if (sResult == Constants.Insert)
                {
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    ClearFields();
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Club created successfully";
                    lblSuccess.Focus();
                }
                else if (sResult == Constants.Update)
                {
                    Response.Cookies[Constants.Update].Expires = DateTime.Now;
                    lblerror.Visible = false;
                    lblerror.Text = string.Empty;
                    ClearFields();
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Club updated successfully";
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
            string code = grvSearch.SelectedRow.Cells[0].Text;
            string Name = grvSearch.SelectedRow.Cells[1].Text;
            string Address = grvSearch.SelectedRow.Cells[2].Text;
            string BPCode = grvSearch.SelectedRow.Cells[3].Text;

            txtClubCode.Text = code;
            txtClubName.Text = StringDecode(Name);
            
            if (Address != "&nbsp;")
            {
                txtClubAddress.Text = StringDecode(Address);
            }
            else
            {
                txtClubAddress.Text = string.Empty;
            }
            if (BPCode != "&nbsp;")
            {
                ddlClubBP.SelectedValue = BPCode;
            }
            else
            {
                ddlClubBP.SelectedValue = strSelect.ToString();
            }

            txtClubCode.ReadOnly = true;
            Response.Cookies[Constants.Update].Value = "1";

            //string message = "Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
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
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("ClubCode");
                dt.Columns.Add("ClubName");
                dt.Columns.Add("Address");
                dt.Columns.Add("ClubBP");

                dt.Rows.Add();
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                dt.Rows[0]["CompanyCode"] = sCompanyCode;
                dt.Rows[0]["ClubCode"] = txtClubCode.Text;
                dt.Rows[0]["ClubName"] = txtClubName.Text;
                dt.Rows[0]["Address"] = txtClubAddress.Text;
                dt.Rows[0]["ClubBP"] = ddlClubBP.SelectedValue;
                
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
                txtClubSearch.Text = string.Empty;
                txtClubCode.Text = string.Empty;
                txtClubName.Text = string.Empty;
                txtClubAddress.Text = string.Empty;
                ddlClubBP.SelectedIndex = -1;
                grvSearch.DataBind();
                if (Response.Cookies[Constants.Update] != null)
                {
                    Response.Cookies[Constants.Update].Expires = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void AssignValues(DataTable dataTable)
        {
            try
            {
                txtClubCode.Text = dataTable.Rows[0]["ClubCode"].ToString();
                txtClubName.Text = dataTable.Rows[0]["ClubName"].ToString();
                txtClubAddress.Text = dataTable.Rows[0]["Address"].ToString();
                ddlClubBP.SelectedValue = dataTable.Rows[0]["ClubBP"].ToString();

                txtClubCode.ReadOnly = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string StringDecode(string sInput)
        {
            StringWriter myWriter = new StringWriter();
            HttpUtility.HtmlDecode(sInput, myWriter);
            String result = myWriter.ToString();
            return result;
        }
        #endregion
    }
}