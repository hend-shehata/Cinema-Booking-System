using Cinema_Booking_System.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.MapStaticAssets();

            app.MapControllerRoute(
             name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
      .WithStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
