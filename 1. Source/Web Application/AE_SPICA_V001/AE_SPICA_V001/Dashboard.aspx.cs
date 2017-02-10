using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AE_SPICA_V001
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.Cookies[Constants.ScreenName] != null)
                    {
                        Request.Cookies[Constants.ScreenName].Value = "Dashboard";
                        Request.Cookies[Constants.ScreenName].Expires = DateTime.Now.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}