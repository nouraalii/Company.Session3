using Company.Session3.BLL.Interfaces;
using Company.Session3.BLL.Repositiories;
using Company.Session3.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Company.Session3.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); //Register Built-in MVC Services
            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); //Allow DI for DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>(); //Allow DI for EmployeeRepository
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });  //Allow DI for CompanyDbContext

            var app = builder.Build();

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
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
