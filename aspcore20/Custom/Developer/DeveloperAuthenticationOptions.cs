using System;
using System.Security.Claims;
using Custom.Developer.Events;
using Microsoft.AspNetCore.Authentication;

namespace Custom.Developer
{
    public class DeveloperAuthenticationOptions : AuthenticationSchemeOptions
    {
        public DeveloperAuthenticationOptions()
        {
            Claims = Array.Empty<ClaimModel>();
            Events = new DeveloperAuthenticationEvents();
        }

        /// <summary>Instance used for events</summary>
        public new DeveloperAuthenticationEvents Events
        {
            get => (DeveloperAuthenticationEvents) base.Events;
            set => base.Events = value;
        }

        public string Name { get; set; }

        public ClaimModel[] Claims { get; set; }

        internal bool Enabled { get; set; }

        internal bool IsInitialized { get; set; }

        internal ClaimsPrincipal Principal { get; set; }
    }
}