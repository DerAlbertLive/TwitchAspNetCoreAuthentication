using System;
using System.Threading.Tasks;

namespace Custom.Developer.Events
{
    public class DeveloperAuthenticationEvents
    {
        public Func<DeveloperSigningOutContext, Task> OnSigningOut { get; set; } = context => Task.CompletedTask;

        public Func<DeveloperPrincipalCreationContext, Task> OnCreatePrincipal { get; set; } =
            context => Task.CompletedTask;
    }
}