using AdminDashboardWorkshop.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository._Identity;
using Talabat.Repository.Data;
using Talabat.Repository.GenericRepository.Data;

namespace AdminDashboardWorkshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var WebApplicationBuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            WebApplicationBuilder.Services.AddControllersWithViews();
            WebApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });
            WebApplicationBuilder.Services.AddDbContext<AppllicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });
            WebApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<AppllicationIdentityDbContext>();

            WebApplicationBuilder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            WebApplicationBuilder.Services.AddAutoMapper(typeof(MapsProfile));

            var app = WebApplicationBuilder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Admin}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
