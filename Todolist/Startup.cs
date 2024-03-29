using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todolist.Services;

namespace Todolist
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
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection").Replace("%CONTENTROOTPATH%", Directory.GetCurrentDirectory())));
            
            services.AddScoped<ITaskListsService, TaskListsService>();
            services.AddScoped<ITasksService, TasksService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddHttpContextAccessor();
            
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("fully permissive", configurePolicy => configurePolicy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyOrigin());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
                })
                .AddGoogle("Google", options =>
                {
                    options.ClientId = Configuration.GetSection("GoogleClientSecret")["auth:google:clientid"];
                    options.ClientSecret = Configuration.GetSection("GoogleClientSecret")["auth:google:clientsecret"];
                    options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/signin-google");
                    options.Events = new OAuthEvents
                    {
                        OnRemoteFailure = (RemoteFailureContext context) =>
                        {
                            context.Response.Redirect("/api/account/denied");
                            context.HandleResponse();
                            return System.Threading.Tasks.Task.CompletedTask;
                        }
                    };
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
            
            app.UseAuthentication();
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Lax
            });
            app.UseCors("fully permissive");
            app.UseMvcWithDefaultRoute();
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
