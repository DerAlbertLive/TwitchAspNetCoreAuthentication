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
            var config = Configuration.GetSection("Authentication:ADC");

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
                .AddOpenIdConnect("ADC", config["DisplayName"], o =>
                {
                    config.Bind(o);
                    var scopes = config["Scopes"];
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

                    o.CallbackPath =
                        "/signin-adc"; // Standard is signin-oidc, jedoch muss es PRO OpenIdConnect Provider einmalig sein
                    o.RemoteSignOutPath =
                        "/signout-adc"; // um direkt für weitere Provider gewappnet zu sein, auch bei nur einem Pfad festlegen
                    o.SignedOutCallbackPath = "/signedout-callback-adc"; // so spart man sich später anpassungen
                    o.SignInScheme = "External";
                    o.SignOutScheme = "Cookies";
                    o.SignedOutRedirectUri = "/";
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
            var config = Configuration.GetSection("Authentication:ADC2");

            return builder.AddOpenIdConnect("ADC2", config["DisplayName"], o =>
            {
                config.Bind(o);
                var scopes = config["Scopes"];
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

                o.CallbackPath =
                    "/signin-adc2"; // Standard is signin-oidc, jedoch muss es PRO OpenIdConnect Provider einmalig sein
                o.RemoteSignOutPath =
                    "/signout-adc2"; // um direkt für weitere Provider gewappnet zu sein, auch bei nur einem Pfad festlegen
                o.SignedOutCallbackPath = "/signedout-callback-adc2"; // so spart man sich später anpassungen
                o.SignInScheme = "External";
                o.SignOutScheme = "Cookies";
                o.SignedOutRedirectUri = "/";
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