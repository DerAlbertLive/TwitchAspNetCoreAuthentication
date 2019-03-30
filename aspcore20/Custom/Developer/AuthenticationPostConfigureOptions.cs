using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Custom.Developer
{
    public class AuthenticationPostConfigureOptions : IPostConfigureOptions<AuthenticationOptions>
    {
        public AuthenticationPostConfigureOptions(IOptionsFactory<DeveloperAuthenticationOptions> factory)
        {
            _factory = factory;
        }

        public void PostConfigure(string name, AuthenticationOptions options)
        {
            var developerScheme = options.Schemes
                .FirstOrDefault(b => b.HandlerType == typeof(DeveloperAuthenticationHandler))?.Name;

            if (string.IsNullOrWhiteSpace(developerScheme))
            {
                return;
            }

            var developerOptions = _factory.Create(developerScheme);

            if (string.IsNullOrWhiteSpace(developerOptions?.Name))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(options.DefaultScheme))
            {
                options.DefaultScheme = developerScheme;
            }
        }

        readonly IOptionsFactory<DeveloperAuthenticationOptions> _factory;
    }
}