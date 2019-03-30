using System.Security.Claims;

namespace Custom.Developer.Events
{
    public class DeveloperPrincipalCreationContext
    {
        public DeveloperPrincipalCreationContext(ClaimsPrincipal claimsPrincipal)
        {
            Principal = claimsPrincipal;
        }

        public ClaimsPrincipal Principal { get; set; }
    }
}