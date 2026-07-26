using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.Owin;

namespace ScrubCRM.Admin
{
    public partial class SiteMaster : MasterPage
    {
        protected Literal litUserName;
        protected LinkButton btnSignOut;

        protected void Page_Init(object sender, EventArgs e)
        {
            // Authorization check shared by every Admin page through this master.
            // Web.config also denies anonymous access to /Admin as a second layer.
            if (Context.User == null || !Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            litUserName.Text = Server.HtmlEncode(Context.User.Identity.Name);
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
            Response.Redirect("~/Login.aspx", false);
        }
    }
}
