using Authorization.Security;
using Authorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization
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
            services.AddScoped<IUserClaimsService, UserClaimsService>();

            services.AddAuthentication(o => { o.DefaultScheme = "Cookies"; })
                .AddCookie("Cookies", o =>
                {
                    o.Cookie.Name = "ADC.AuthorizationDemo.Auth";
                    o.LoginPath = "/Login";
                    o.AccessDeniedPath = "/Denied";
                });

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("Is16", builder => { builder.AddRequirements(new MinimumAgeRequirement(16)); });

                    options.AddPolicy("InvoiceReader",
                        builder => { builder.RequireClaim("invoice", "read", "write"); });
                    options.AddPolicy("InvoiceWriter", builder => { builder.RequireClaim("invoice", "write"); });
                })
                .AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>()
                .AddScoped<IAuthorizationHandler, InvoiceAuthorizationRequirementHandler>();
            
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
    }
}