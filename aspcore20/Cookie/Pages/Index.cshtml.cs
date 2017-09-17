using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookie.Pages
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
