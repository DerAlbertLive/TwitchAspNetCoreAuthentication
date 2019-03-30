using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace OpenId.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SecurityController : Controller
    {
        private readonly IOptionsFactory<OpenIdConnectOptions> _optionsFactory;

        public SecurityController(IOptionsFactory<OpenIdConnectOptions> optionsFactory)
        {
            _optionsFactory = optionsFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var authSchemes = User.FindFirst("auth_scheme");
            await HttpContext.SignOutAsync(authSchemes?.Value);
            return Content("");
        }

        [Authorize]
        public IActionResult Authenticate(string returnUrl = "~/")
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return NotFound();
        }

        public IActionResult Login(string returnUrl = "~/")
        {
            var options = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("External"),
                Items = {{"returnUrl", returnUrl}}
            };
            return Challenge(options, "ADC");
        }

        public async Task<IActionResult> External()
        {
            var authInfo = await HttpContext.AuthenticateAsync("External");
            if (authInfo == null)
            {
                return NotFound();
            }

            authInfo.Properties.Items.TryGetValue("returnUrl", out var returnUrl);


            var properties = new AuthenticationProperties();
            
            var idToken = await HttpContext.GetTokenAsync("External", "id_token");
            
            // für den autmatischen Single Signout braucht OpenIdConnect das id_token zur Bestätigung, das die Anwendung es anfordert
            // ohne id_token wird auf der OI Provider Seite überprüft.
            if (!string.IsNullOrWhiteSpace(idToken))
            {
                properties.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
            }

            var principal = authInfo.Principal;

            // Damit wird beim Logout wissen welches Scheme verwendet wurde, können wir es uns merken
            // oder aufwändig ermitteln. Der einfachheithalber merken wir es uns.
            authInfo.Properties.Items.TryGetValue(".AuthScheme", out var authScheme);
            principal.Identities.First().AddClaim(new Claim("auth_scheme", authScheme));

            // Wenn man Claims aus dem Access Token in den Cookie Claims haben will, so muss man diese hinzufügen.
            await AddAccessTokenClaimsToPrincipal(authScheme, principal);
            // nur für die Demonstration,
            await AddIdTokenClaimsToPrincipal(authScheme, principal);

            await HttpContext.SignInAsync("Cookies", principal, properties);
            await HttpContext.SignOutAsync("External");

            return LocalRedirect(returnUrl ?? "~/");
        }


        [HttpGet]
        public IActionResult Nope(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return RedirectToPage("/Error");
        }


        private Task AddAccessTokenClaimsToPrincipal(string authenticationScheme, ClaimsPrincipal principal)
        {
            return AddTokenClaimsToPrincipal(authenticationScheme, principal, "access_token");
        }

        private Task AddIdTokenClaimsToPrincipal(string authenticationScheme, ClaimsPrincipal principal)
        {
            return AddTokenClaimsToPrincipal(authenticationScheme, principal, "id_token");
        }

        private async Task AddTokenClaimsToPrincipal(string authenticationScheme, ClaimsPrincipal principal, string tokenName)
        {
            var jwtToken = await HttpContext.GetTokenAsync("External", tokenName);

            if (!string.IsNullOrWhiteSpace(jwtToken))
            {
                var token = new JwtSecurityToken(jwtToken);
                var claimsIdentity = new ClaimsIdentity(token.Claims, tokenName);

                // die OpenIdConnectOptions zum authenticationScheme ermittteln
                var options = _optionsFactory.Create(authenticationScheme);
                
                // Alle Claimsctions ausführen, diese löschen z.b. nicht benötigte Claims für Cookies.
                foreach (var action in options.ClaimActions)
                {
                    action.Run(null, claimsIdentity, null);
                }

                principal.AddIdentity(claimsIdentity);
            }
        }
    }
}