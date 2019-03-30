using Microsoft.AspNetCore.Authentication;

namespace Custom.Developer.Events
{
    public class DeveloperSigningOutContext
    {
        public AuthenticationProperties Properties { get; }

        public DeveloperSigningOutContext(AuthenticationProperties properties)
        {
            Properties = properties;
        }
    }
}