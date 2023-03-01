using Digiflex.Areas.Manage.Services;
using Digiflex.DAL;
using Digiflex.Models;
using Digiflex.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Terminal;
using System.Configuration;

namespace Digiflex
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DigiflexContext>( opt =>
            {
                opt.UseSqlServer("Server=CHIEF\\SQLEXPRESS;Database=DigiflexDB;Trusted_Connection=True");
            });

            builder.Services.AddScoped<SettingService>();
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequiredLength = 8;

                opt.User.RequireUniqueEmail = false;

            }).AddEntityFrameworkStores<DigiflexContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<AdminLayoutService>();
            builder.Services.AddScoped<MainLayoutService>();

            var app = builder.Build();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseStatusCodePagesWithReExecute("/Error/NotFound", "?code={0}");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=WaitRoom}/{id?}");

            app.Run();
        }
    }
}