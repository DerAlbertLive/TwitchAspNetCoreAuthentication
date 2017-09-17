using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public bool IsAuthenticated => User.Identity.IsAuthenticated;
        public IEnumerable<Claim> Claims => User.Claims;
        public IEnumerable<ClaimsIdentity> Identities => User.Identities;
    }
}
