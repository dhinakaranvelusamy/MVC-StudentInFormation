using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentCRUD.Repo;

namespace StudentCRUD
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC support
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation(); // optional: allows live edit of Razor views

            // Register StudentRepository for dependency injection
            services.AddScoped<IStudentRepository, StudentRepository>();

            // Optional: Enable TempData with session
            services.AddSession();
        }

        // Configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Show detailed error pages
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Use custom error page in production
                app.UseExceptionHandler("/Student/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Serve CSS, JS, images

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession(); // if using session

            // Default route
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Student}/{action=Dashboard}/{id?}");
            });
        }
    }
}
