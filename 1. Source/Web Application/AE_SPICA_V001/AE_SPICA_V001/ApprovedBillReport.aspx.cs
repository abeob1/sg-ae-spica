﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using AE_SPICA.DAL;
using CrystalDecisions.Shared;
using System.Configuration;

namespace AE_SPICA_V001
{

    public partial class ApprovedBillReport : System.Web.UI.Page
    {
        #region Objects
        public string strSelect = "-- Select --";
        public clsReports oReports = new clsReports();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies[Constants.ScreenName].Value = "Approved Bill Report";
                if (!IsPostBack)
                {
                    ClearFields();
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

                Response.Cookies[Constants.ABRFromDate].Value = txtFromDate.Text;
                Response.Cookies[Constants.ABRToDate].Value = txtToDate.Text;
                Response.Redirect("ViewApprovedBillReport.aspx", false);

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
        }

        #endregion
    }
}