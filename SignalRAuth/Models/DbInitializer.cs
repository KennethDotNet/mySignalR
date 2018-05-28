using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SignalRAuth.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAuth.Models
{
    public class DbInitializer
    {
        public static async Task Seed(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            context.Database.EnsureCreated();

            if(!context.Users.Any())
            {
                await userManager.CreateAsync(new ApplicationUser { UserName = "test", Email = "test@infoeasy.no", EmailConfirmed = true, Fullname = "Test user" }, "Password!123");
                await userManager.CreateAsync(new ApplicationUser { UserName = "test2", Email = "tes2t@infoeasy.no", EmailConfirmed = true, Fullname = "Test user2" }, "Password!123");
                await context.SaveChangesAsync();
            }

        }
    }
}
