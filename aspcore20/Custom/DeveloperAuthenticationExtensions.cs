using System;
using Custom.Developer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Custom
{
    public static class DeveloperAuthenticationExtensions
    {
        /// <summary>
        /// Add implicit authentication via the the settings of Authentication:Developer
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddDeveloperAuthentication(this AuthenticationBuilder builder, IConfiguration configuration)
            => builder.AddDeveloperAuthentication(configuration, null);

        /// <summary>
        /// Add implicit authentication via the the settings of Authentication:Developer
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddDeveloperAuthentication(this AuthenticationBuilder builder, IConfiguration configuration, Action<DeveloperAuthenticationOptions> configureOptions)
       => builder.AddDeveloperAuthentication(DeveloperAuthenticationDefaults.Scheme, configuration, configureOptions);


        static AuthenticationBuilder AddDeveloperAuthentication(this AuthenticationBuilder builder, string authenticationScheme, IConfiguration configuration, Action<DeveloperAuthenticationOptions> configureOptions)
            => builder.AddDeveloperAuthentication(authenticationScheme, null, configuration , configureOptions: configureOptions);

        static AuthenticationBuilder AddDeveloperAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string displayName, IConfiguration configuration, Action<DeveloperAuthenticationOptions> configureOptions)
        {
            builder.Services.Configure<DeveloperAuthenticationOptions>(authenticationScheme, configuration.GetSection("Authentication:Developer"));
            builder.Services.ConfigureOptions<AuthenticationPostConfigureOptions>();
 

            return builder.AddScheme<DeveloperAuthenticationOptions, DeveloperAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}