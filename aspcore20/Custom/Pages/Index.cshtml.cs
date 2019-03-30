using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Custom.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public bool IsAuthenticated => User.Identity.IsAuthenticated;
        public IEnumerable<ClaimsIdentity> Identities => User.Identities;
    }
}
