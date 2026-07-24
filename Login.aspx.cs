using System;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace ScrubCRM
{
    public partial class Login : Page
    {
        // Designer-generated control declarations (no separate .designer.cs
        // file is used in this project; these fields are wired up by ASP.NET's
        // ID-based control matching at parse time).
        protected TextBox txtEmail;
        protected TextBox txtPassword;
        protected Button btnLogin;
        protected Label lblError;

        private ApplicationSignInManager SignInManager
        {
            get { return Context.GetOwinContext().Get<ApplicationSignInManager>(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User != null && Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Admin/Dashboard.aspx", false);
            }
        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            lblError.Visible = false;

            var result = await SignInManager.PasswordSignInAsync(
                txtEmail.Text.Trim(), txtPassword.Text, isPersistent: false, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    Response.Redirect("~/Admin/Dashboard.aspx", false);
                    break;
                case SignInStatus.LockedOut:
                    lblError.Text = "This account is locked out. Try again in a few minutes.";
                    lblError.Visible = true;
                    break;
                default:
                    lblError.Text = "Invalid email or password.";
                    lblError.Visible = true;
                    break;
            }
        }
    }
}
