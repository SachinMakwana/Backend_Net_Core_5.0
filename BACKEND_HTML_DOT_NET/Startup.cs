using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Providers.Entities;

namespace BACKEND_HTML_DOT_NET
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
            
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.LoginPath = "/";
                option.ExpireTimeSpan = TimeSpan.FromSeconds(1000);
                option.Events = new CookieAuthenticationEvents()
                {
                    OnSigningIn = async context =>
                    {
                        await Task.CompletedTask;
                    },
                    OnSignedIn = async context =>
                    {
                        await Task.CompletedTask;
                    },
                    OnValidatePrincipal = async context =>
                    {
                        await Task.CompletedTask;
                    }
                };
            });
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromSeconds(30);
            });
            services.AddAuthorization(option =>
            {
                option.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("ADMIN"));
                option.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("FACULTY"));
            });
            services.ConfigureApplicationCookie(option =>
            {
                option.AccessDeniedPath = new PathString("/AccessDenied");
            });

            var identitySettingsSection =
                Configuration.GetSection("AppIdentitySettings");
            services.Configure<AppIdentitySettings>(identitySettingsSection);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class AppIdentitySettings
    {
        public string apiBaseUrl { get; set; }
        public string imageBaseUrl { get; set; }
    }
}
