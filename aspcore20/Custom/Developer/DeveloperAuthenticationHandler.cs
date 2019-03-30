using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Custom.Developer.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Custom.Developer
{
    public class DeveloperAuthenticationHandler : SignOutAuthenticationHandler<DeveloperAuthenticationOptions>
    {
        public DeveloperAuthenticationHandler(IOptionsMonitor<DeveloperAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Options.Enabled)
            {
                return AuthenticateResult.NoResult();
            }

            if (Options.Principal == null)
            {
                return AuthenticateResult.Fail("No principal.");
            }

            var ticket = new AuthenticationTicket(Options.Principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        protected override async Task InitializeHandlerAsync()
        {
            if (!Options.IsInitialized)
            {
                Options.Enabled = await EnsurePrincipal();
            }

            Options.IsInitialized = true;
        }

        async Task<bool> EnsurePrincipal()
        {
            if (Options.Principal == null)
            {
                var claimsPrincipal = CreatePrincipal();
                var context = new DeveloperPrincipalCreationContext(claimsPrincipal);
                await Events.OnCreatePrincipal(context);
                Options.Principal = context.Principal;

                return Options.Principal != null;
            }

            return true;
        }

        ClaimsPrincipal CreatePrincipal()
        {
            if (string.IsNullOrWhiteSpace(Options.Name))
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim("name", Options.Name)
            };

            claims.AddRange(Options.Claims.Select(c =>
                new Claim(c.Type, c.Value, null, typeof(DeveloperAuthenticationHandler).Name)));
            var identity = new ClaimsIdentity(claims, Scheme.Name, "name", "role");
            return new ClaimsPrincipal(identity);
        }

        protected override async Task HandleSignOutAsync(AuthenticationProperties properties)
        {
            properties = properties ?? new AuthenticationProperties();
            var context = new DeveloperSigningOutContext(properties);
            await Events.OnSigningOut(context);
            Options.Principal = null;
            Options.Enabled = Options.Principal != null;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var uri = BuildRedirectUri(OriginalPath);
            await EnsurePrincipal();
            Options.Enabled = Options.Principal != null;
            if (Options.Enabled)
            {
                Context.Response.Redirect(uri);
            }
        }

        protected new DeveloperAuthenticationEvents Events
        {
            get => (DeveloperAuthenticationEvents) base.Events;
            set => base.Events = value;
        }
    }
}