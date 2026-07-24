using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace ScrubCRM.Models
{
    // Single role ("Administrator") is used for this MVP. The Identity tables
    // (AspNetRoles / AspNetUserRoles) still exist so more roles can be added later.
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
