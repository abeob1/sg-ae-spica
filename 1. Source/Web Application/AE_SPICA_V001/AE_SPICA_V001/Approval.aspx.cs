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
    public partial class Approval : System.Web.UI.Page
    {
        #region Objects
        public string strSelect = "-- Select --";
        public clsApproval oApproval = new clsApproval();
        public clsFileReference oFileReference = new clsFileReference();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies[Constants.ScreenName].Value = "Approval";
                if (!IsPostBack)
                {
                    string sUserName = Convert.ToString(Request.Cookies[Constants.UserName].Value);
                    lblUsername.Text = sUserName;
                    ApproveData();
                    DataSet ds = GetApprovedMonth();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtMonth.Text = ds.Tables[0].Rows[0]["LastApprovedMonth"].ToString();
                            lblApprovedAmountBase.Text = ds.Tables[0].Rows[0]["ApprovedMonthBase"].ToString();
                        }
                        else
                        {
                            txtMonth.Text = string.Empty;
                            lblApprovedAmountBase.Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void grvFile_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var var1 = e.NewPageIndex;
                var var2 = this.grvFile.PageIndex;
                this.grvFile.PageIndex = e.NewPageIndex;

                DataTable tblRecords = (DataTable)ViewState["ApprovalData"];
                grvFile.DataSource = tblRecords;
                grvFile.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void lnkPeviewBilling_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow grdrow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Label lblFileRef = (Label)grdrow.FindControl("lblSpicaRefNo");
                Response.Cookies[Constants.FileRefNo].Value = lblFileRef.Text;
                Response.Redirect("PreviewBilling_Approval.aspx", false);
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void lnkBillingDetails_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow grdrow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Label lblFileRefNo = (Label)grdrow.FindControl("lblSpicaRefNo");
                LoadTimeandExpenseEntryDetails(lblFileRefNo.Text);
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            mpePopup.Hide();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields();
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);

                string ids = string.Empty;
                string fileReference = string.Empty;
                if (txtMonth.Text == string.Empty)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly select the month to Approve";
                    return;
                }
                int iChecked = 0;
                foreach (GridViewRow val in grvFile.Rows)
                {
                    CheckBox chkItems = (CheckBox)val.FindControl("chkSelect");
                    Label lblId = (Label)val.FindControl("lblID");
                    Label lblSpicaRefNo = (Label)val.FindControl("lblSpicaRefNo");
                    if (chkItems.Checked)
                    {
                        iChecked = iChecked + 1;
                        ids = ids + lblId.Text + ",";
                        fileReference = fileReference + lblSpicaRefNo.Text + ",";
                    }
                }

                if (iChecked == 0)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly select Vessels to Approve";
                    lblerror.Focus();
                }
                else
                {
                    string sResult = oApproval.ApproveSelectedVessel(txtMonth.Text, ids.TrimEnd(','), fileReference.TrimEnd(','), sCompanyCode, sUserCode);
                    if (sResult == "SUCCESS")
                    {
                        ApproveData();
                        DataSet ds = GetApprovedMonth();
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                txtMonth.Text = ds.Tables[0].Rows[0]["LastApprovedMonth"].ToString();
                                lblApprovedAmountBase.Text = ds.Tables[0].Rows[0]["ApprovedMonthBase"].ToString();
                            }
                            else
                            {
                                txtMonth.Text = string.Empty;
                                lblApprovedAmountBase.Text = string.Empty;
                            }
                        }
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Selected Vessels Approved Successfully";
                        lblSuccess.Focus();
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = sResult;
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields();
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);

                string ids = string.Empty;
                string fileReference = string.Empty;
                int iChecked = 0;
                foreach (GridViewRow val in grvFile.Rows)
                {
                    CheckBox chkItems = (CheckBox)val.FindControl("chkSelect");
                    Label lblId = (Label)val.FindControl("lblID");
                    Label lblSpicaRefNo = (Label)val.FindControl("lblSpicaRefNo");
                    if (chkItems.Checked)
                    {
                        iChecked = iChecked + 1;
                        ids = ids + lblId.Text + ",";
                        fileReference = fileReference + lblSpicaRefNo.Text + ",";
                    }
                }

                if (iChecked == 0)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly select Vessels to Reject";
                    lblerror.Focus();
                }
                else
                {
                    string sResult = oApproval.RejectSelectedVessel(ids.TrimEnd(','), fileReference.TrimEnd(','), sCompanyCode, sUserCode);
                    if (sResult == "UPDATE")
                    {
                        ApproveData();
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Selected Vessels Rejected Successfully";
                        lblSuccess.Focus();
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = sResult;
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected void grvTimeEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { }

        protected void grvExpenseEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { }

        protected void ApproveData()
        {
            try
            {
                DataSet dsApproval = new DataSet();
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                dsApproval = oApproval.GetApprovalDetails(sCompanyCode, sUserCode);
                ViewState["ApprovalData"] = dsApproval.Tables[0];
                grvFile.DataSource = dsApproval;
                grvFile.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        protected DataSet GetApprovedMonth()
        {
            DataSet dsApprovedMonth = new DataSet();
            try
            {
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                dsApprovedMonth = oApproval.GetLastApprovedMonth(sCompanyCode, sUserCode);
            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
            return dsApprovedMonth;
        }

        public void LoadTimeandExpenseEntryDetails(string sID)
        {
            try
            {
                //DataTable table = new DataTable();
                //DataTable table1 = new DataTable();
                object sumObject = new object();
                object sumObject1 = new object();
                DataSet dsTimeEntry = new DataSet();
                DataSet dsExpenseEntry = new DataSet();
                string sUserCode = Convert.ToString(Request.Cookies[Constants.UserCode].Value);
                string sCompanyCode = Convert.ToString(Request.Cookies[Constants.CompanyCode].Value);
                dsTimeEntry = oApproval.GetTimeEntryRecords(sCompanyCode, sUserCode);
                dsExpenseEntry = oApproval.GetExpenseEntryRecords(sCompanyCode, sUserCode);

                DataView dv = (DataView)dsTimeEntry.Tables[0].DefaultView;
                dv.RowFilter = "ID = '" + sID + "'";
                DataTable dt = dv.ToTable();
                grvTimeEntry.DataSource = dt;
                grvTimeEntry.DataBind();
                if (dt != null && dt.Rows.Count > 0)
                {
                    //table = dsTimeEntry.Tables[0];
                    sumObject = dt.Compute("Sum(BillAmount)", "");
                }
                else
                {
                    sumObject = 0;
                }
                ViewState["TimeEntry1"] = dt;
                lblBillAmt.Text = sumObject.ToString();

                DataView dv1 = (DataView)dsExpenseEntry.Tables[0].DefaultView;
                dv1.RowFilter = "ID = '" + sID + "'";
                DataTable dt1 = dv1.ToTable();
                grvExpenseEntry.DataSource = dt1;
                grvExpenseEntry.DataBind();
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    //table1 = dsExpenseEntry.Tables[0];
                    sumObject1 = dt1.Compute("Sum(ChargableAmt)", "");
                }
                else
                {
                    sumObject1 = 0;
                }
                ViewState["ExpenseEntry1"] = dt1;
                lblChargeableAmt.Text = sumObject1.ToString();
                //lblGrandTotal.Text = Convert.ToString(Convert.ToDecimal(lblBillAmt.Text == "" ? "0" : lblBillAmt.Text) + Convert.ToDecimal(lblChargeableAmt.Text == "" ? "0" : lblChargeableAmt.Text));

            }
            catch (Exception ex)
            {
                lblerror.Visible = true;
                lblerror.Text = ex.Message.ToString();
            }
        }

        public void clearFields()
        {
            try
            {
                lblerror.Visible = false;
                lblerror.Text = string.Empty;
                lblSuccess.Visible = true;
                lblSuccess.Text = string.Empty;
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