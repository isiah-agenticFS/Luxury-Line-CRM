using System;
using System.Data.Entity;
using System.Web;
using ScrubCRM.Data;

namespace ScrubCRM
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Database.SetInitializer(new ApplicationDbInitializer());

            // Force initializer to run (and seed) at startup rather than on
            // the first request, so the first admin login never races the seed.
            using (var db = new ApplicationDbContext())
            {
                db.Database.Initialize(force: false);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception == null) return;

            System.Diagnostics.Trace.TraceError(exception.ToString());

            // Avoid leaking stack traces / connection details to the browser.
            // customErrors in Web.config renders the friendly error page.
        }
    }
}
