using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildCompleteEcommerceWithASPNETCoreMVC
{
    public class Startup
    {

        public IConfiguration configuration { get; }

        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/admin/login/index";
                    //options.LogoutPath = "/admin/login/signout/";
                    options.AccessDeniedPath = "/admin/login/accessdenied/";

                    //options.LoginPath = new PathString("/admin/login/index");
                    //options.LogoutPath = new PathString("/admin/login/signout/");
                    //options.AccessDeniedPath = new PathString("/admin/login/accessdenied/");
                });

            services.AddSession();

            services.AddMvc();

            services.AddMvc().AddSessionStateTempDataProvider(); //Helpers

            var connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(option =>
                    option.UseLazyLoadingProxies().UseSqlServer(connection)
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSession();

            app.UseStaticFiles();

            app.UseMvc(router =>
            {
                router.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
