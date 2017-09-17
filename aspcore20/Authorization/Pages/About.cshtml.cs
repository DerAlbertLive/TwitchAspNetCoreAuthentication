using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.Pages
{
    
    [Authorize]
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }

        public bool IsAuthenticated => User.Identity.IsAuthenticated;
        public IEnumerable<Claim> Claims => User.Claims;
    }
}
