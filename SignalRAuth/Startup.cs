using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRAuth.Auth;
using SignalRAuth.Hubs;
using SignalRAuth.Models;

namespace SignalRAuth
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton(typeof(ConnectionList));
            services.AddSignalR(options => { options.EnableDetailedErrors = true; options.KeepAliveInterval = TimeSpan.FromSeconds(10); });
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
             {
                 options.User.RequireUniqueEmail = false;
                 options.Password.RequiredLength = 1;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireNonAlphanumeric = false;
             }
            ).AddEntityFrameworkStores<AppDbContext>(); 

            services.AddAuthentication().AddCookie(builder => builder.Cookie.Name = "someName");
            services.AddAntiforgery(options => options.Cookie.Name = "someName");
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseAuthentication(); //Must be before mappings (url) and hubs
         
            app.UseSignalR(routes =>
            {
                routes.MapHub<TestHub>("/test");
                routes.MapHub<TestHubTwo>("/testme");
            });
            app.UseStatusCodePages();



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            }
            );
        }
    }
}
