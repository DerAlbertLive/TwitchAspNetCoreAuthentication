using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Cookie.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookie.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserClaimsService _userClaimsService;

        public LoginModel(IUserClaimsService userClaimsService)
        {
            _userClaimsService = userClaimsService;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public void OnGet(string returnUrl)
        {
            Input.ReturnUrl = returnUrl;
        }
        
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (string.Compare(Input.Username, Input.Password, StringComparison.Ordinal) == 0)
            {
                var claims = _userClaimsService.GetClaimsForUser(Input.Username);
                
                var identity = new ClaimsIdentity(claims, "Local", "name", "role");

                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));


                var redirectUri = returnUrl ?? "/";
                if (Url.IsLocalUrl(redirectUri))
                {
                    return Redirect(redirectUri);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unbekannter Benutzer");
            }
            return Page();
        }

        public class InputModel
        {
            [Required]
            [MaxLength(200)]
            public string Username { get; set; }

            [Required]
            [MaxLength(200)]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            
            public string ReturnUrl { get; set; }
        }
    }
}