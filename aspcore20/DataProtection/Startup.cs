using System.IO;
using DataProtection.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataProtection
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

            services.AddDataProtection()
                .SetApplicationName("ASP.DataProtectionSample")
                .PersistKeysToFileSystem(new DirectoryInfo(@"c:\temp\keys"));
            
            services.AddAuthentication(o =>
                {
                    o.DefaultScheme = "Cookies";
                })
                .AddCookie("Cookies", o =>
                {
                    o.Cookie.Name = "ADC.DataProtectionDemo.Auth";
                    o.LoginPath = "/Login";
                    o.AccessDeniedPath = "/Denied";
                });
            services.AddMvc()
                .AddRazorPagesOptions(o =>
                {
                    o.Conventions.AuthorizePage("/Encrypt");
                    o.Conventions.AuthorizePage("/Decrypt");
                });
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