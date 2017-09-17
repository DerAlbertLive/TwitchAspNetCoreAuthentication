using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenId.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public LoginModel(IAuthenticationSchemeProvider schemeProvider)
        {
            _schemeProvider = schemeProvider;
            Input = new InputModel();
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public async Task OnGet(string returnUrl)
        {
            await FillAuthenticationSchemes();
            Input.ReturnUrl = returnUrl;
        }

        private async Task FillAuthenticationSchemes()
        {
            var schemes = await GetExternalAuthenticationSchemes();

            Schemes = from s in schemes
                select new SelectListItem()
                {
                    Value = s.Name,
                    Text = s.DisplayName ?? s.Name
                };
        }

        private async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemes()
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();
            schemes = from s in schemes
                where typeof(IAuthenticationRequestHandler).IsAssignableFrom(s.HandlerType)
                select s;
            return schemes;
        }

        public IEnumerable<SelectListItem> Schemes { get; set; }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            var schemes = await GetExternalAuthenticationSchemes();
            if (schemes.All(s => s.Name != Input.AuthenticationScheme))
            {
                ModelState.AddModelError(nameof(Input.AuthenticationScheme), $"unknown Scheme {Input.AuthenticationScheme}");
            }
            
            if (!ModelState.IsValid)
            {
                await FillAuthenticationSchemes();
                return Page();
            }

            var options = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("External", "Security"),
                Items = { { "returnUrl", returnUrl } }
            };
            return Challenge(options, Input.AuthenticationScheme);
        }

        public class InputModel
        {
            [Required]
            [MaxLength(200)]
            [Display(Name = "Provider")]
            public string AuthenticationScheme { get; set; }
            
            public string ReturnUrl { get; set; }
        }
    }
}