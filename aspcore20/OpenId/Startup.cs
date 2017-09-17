using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OpenId
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new OpenIdConfiguration();
            Configuration.GetSection("Authentication:ADC").Bind(config);

            var authBuilder = services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = "Cookies";
                    o.DefaultChallengeScheme = "Cookies";
                    o.DefaultSignInScheme = "Cookies";
                    o.DefaultSignOutScheme = "Cookies";
                })
                .AddCookie("Cookies", o =>
                {
                    o.LoginPath = "/Login";
                    o.Cookie.Name = "ADC.Authentication";
                })
                .AddCookie("External", o => { o.Cookie.Name = "ADC.External"; })
                .AddOpenIdConnect("ADC", config.DisplayName, o =>
                {
                    o.Authority = config.Authority;
                    o.ClientId = config.ClientId;
                    o.ClientSecret = config.ClientSecret;
                    o.ResponseType = config.ResponseType;
                    var scopes = config.Scopes;
                    if (!string.IsNullOrWhiteSpace(scopes))
                    {
                        o.Scope.Clear();
                        foreach (var scope in scopes.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
                        {
                            o.Scope.Add(scope);
                        }
                    }
                    o.TokenValidationParameters.NameClaimType = "name";
                    o.TokenValidationParameters.RoleClaimType = "role";

                    o.GetClaimsFromUserInfoEndpoint = true;
                    o.SaveTokens = true;
                    
                    // Standard ist signin-oidc, signout-oidc, signedout-callback-oidc, jedoch muss es je OpenIdConnect Configuration einmalig sein
                    // um direkt für weitere Provider gewappnet zu sein, auch bei nur einem Pfad festlegen
                    // so spart man sich später anpassungen, dies Pfade müssen dem OpenIdConnect Provider bekannt sein.
                    o.CallbackPath = "/signin-adc";
                    o.RemoteSignOutPath = "/signout-adc"; 
                    o.SignedOutCallbackPath = "/signedout-callback-adc";

                    o.SignInScheme = "External";
                    o.SignOutScheme = "Cookies";
                    o.SignedOutRedirectUri = "/"; // dahin wird nach dem erfolgtem Logout weitergeleitet
                    o.Events = new OpenIdConnectEvents()
                    {
                        OnRedirectToIdentityProviderForSignOut = async context =>
                        {
                            // für das Single Signout ein gemwerkets id_token also authoriserung für das Logout dem IdServer mitgeben
                            var idToken = await context.HttpContext.GetTokenAsync("id_token");
                            if (!string.IsNullOrWhiteSpace(idToken))
                            {
                                context.ProtocolMessage.IdTokenHint = idToken;
                            }
                        }
                    };
                });

            AddSecondOpenIdConfiguration(authBuilder);

            services.AddMvc();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }


        private AuthenticationBuilder AddSecondOpenIdConfiguration(AuthenticationBuilder builder)
        {
            var config = new OpenIdConfiguration();
                Configuration.GetSection("Authentication:ADC2").Bind(config);

            return builder.AddOpenIdConnect("ADC2", config.DisplayName, o =>
            {
                o.Authority = config.Authority;
                o.ClientId = config.ClientId;
                o.ClientSecret = config.ClientSecret;
                o.ResponseType = config.ResponseType;

                var scopes = config.Scopes;
                if (!string.IsNullOrWhiteSpace(scopes))
                {
                    o.Scope.Clear();
                    foreach (var scope in scopes.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        o.Scope.Add(scope);
                    }
                }
                o.TokenValidationParameters.NameClaimType = "name";
                o.TokenValidationParameters.RoleClaimType = "role";

                o.GetClaimsFromUserInfoEndpoint = true;
                o.SaveTokens = true;

                o.CallbackPath = "/signin-adc2"; 
                o.RemoteSignOutPath = "/signout-adc2"; 
                o.SignedOutCallbackPath = "/signedout-callback-adc2"; 
                o.SignInScheme = "External";
                o.SignOutScheme = "Cookies";
                o.SignedOutRedirectUri = "/Contact";
                o.Events = new OpenIdConnectEvents()
                {
                    OnRedirectToIdentityProviderForSignOut = async context =>
                    {
                        // für das Single Signout ein gemwerkets id_token also authoriserung für das Logout dem IdServer mitgeben
                        var idToken = await context.HttpContext.GetTokenAsync("id_token");
                        if (!string.IsNullOrWhiteSpace(idToken))
                        {
                            context.ProtocolMessage.IdTokenHint = idToken;
                        }
                    }
                };
            });
        }
    }
}