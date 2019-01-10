using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Todolist.Models;

namespace Todolist
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _contentRootPath = env.ContentRootPath;
        }

        private string _contentRootPath = "";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDbContext<Context>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection").Replace("%CONTENTROOTPATH%", _contentRootPath)));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<Context>()
            //    .AddDefaultTokenProviders();
            //IdentityCookieAuthenticationBuilderExtensions.AddExternalCookie();
            //services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");
            services.AddAuthentication(options =>
                {
                    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
                })
                .AddGoogle("Google", options =>
                {
                     options.ClientId = Configuration["auth:google:clientid"];
                     options.ClientSecret = Configuration["auth:google:clientsecret"];
                     options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/signin-google");
                    //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "UserId");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "EmailAddress", ClaimValueTypes.Email);
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "Name");
                }).AddCookie(options => { options.Cookie.Name = IdentityConstants.ExternalScheme; });
            //    .AddCookie(options =>
            //    {
            //        options.CookieName = "MyCookie",
            //        options.
            //    });

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationScheme = "MyCookie",
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    LoginPath = new PathString("/account/login")
            //});


            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("fully permissive", configurePolicy => configurePolicy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
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
                app.UseHsts();
            }
            
            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseSpaStaticFiles();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action=Index}/{id?}");
            //});
            app.UseMvcWithDefaultRoute();
            app.UseCors("fully permissive");
            app.UseAuthentication();
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
                
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
