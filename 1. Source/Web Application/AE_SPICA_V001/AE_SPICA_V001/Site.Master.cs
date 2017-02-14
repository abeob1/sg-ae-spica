using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AE_SPICA_V001
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.Cookies[Constants.UserName] != null && Request.Cookies[Constants.ScreenName] != null)
                    {
                        lblUser.Text = Request.Cookies[Constants.UserName].Value.ToString().ToUpper();
                        lblScreenName.Text = Request.Cookies[Constants.ScreenName].Value.ToString();
                        lblCompany.Text = Request.Cookies[Constants.CompanyName].Value.ToString();
                        Response.Cookies[Constants.ScreenName].Expires = DateTime.Now;
                    }

                    string sRoleName = Request.Cookies[Constants.UserRoleName].Value.ToString();
                    string sRole1 = System.Configuration.ConfigurationManager.AppSettings["Role1"].ToString();
                    string sRole2 = System.Configuration.ConfigurationManager.AppSettings["Role2"].ToString();
                    string sRole3 = System.Configuration.ConfigurationManager.AppSettings["Role3"].ToString();
                    string sRole4 = System.Configuration.ConfigurationManager.AppSettings["Role4"].ToString();
                    if (sRoleName == sRole1 || sRoleName == sRole2)
                    {
                        leftMenuFiles.Visible = true;
                        leftMenuTE.Visible = true;
                        leftMenuEEntry.Visible = true;
                        if (sRoleName == sRole1)
                        {
                            leftMenuApp.Visible = true;
                            leftMenuSR.Visible = false;
                        }
                        else if (sRoleName == sRole2)
                        {
                            leftMenuApp.Visible = false;
                            leftMenuSR.Visible = true;
                        }
                        leftMenuReg.Visible = true;
                        leftMenuBR.Visible = true;
                        leftMenuCM.Visible = true;
                        leftMenuAS.Visible = true;
                        leftMenuRep.Visible = true;
                        LeftMenuListing.Visible = true;
                        leftMenuChangePassword.Visible = false;
                        leftMenuLO.Visible = true;
                    }
                    if (sRoleName == sRole3)
                    {
                        leftMenuFiles.Visible = true;
                        leftMenuTE.Visible = true;
                        leftMenuEEntry.Visible = true;
                        leftMenuApp.Visible = true;
                        leftMenuReg.Visible = false;
                        leftMenuBR.Visible = false;
                        leftMenuCM.Visible = true;
                        leftMenuAS.Visible = false;
                        leftMenuRep.Visible = true;
                        leftMenuSR.Visible = false;
                        LeftMenuListing.Visible = true;
                        leftMenuChangePassword.Visible = false;
                        leftMenuLO.Visible = true;
                    }
                    if (sRoleName == sRole4)
                    {
                        leftMenuFiles.Visible = true;
                        leftMenuTE.Visible = true;
                        leftMenuEEntry.Visible = true;
                        leftMenuApp.Visible = false;
                        leftMenuReg.Visible = false;
                        leftMenuBR.Visible = false;
                        leftMenuCM.Visible = false;
                        leftMenuAS.Visible = false;
                        leftMenuRep.Visible = false;
                        leftMenuSR.Visible = false;
                        LeftMenuListing.Visible = true;
                        leftMenuChangePassword.Visible = true;
                        leftMenuLO.Visible = true;
                    }
                }
            }
            catch (Exception)
            {
                Response.Cookies[Constants.SessionExpired].Value = "SessionExpired";
                Response.Redirect(Constants.LoginURL);
            }
        }
    }
}