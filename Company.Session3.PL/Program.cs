using Company.Session3.BLL;
using Company.Session3.BLL.Interfaces;
using Company.Session3.BLL.Repositiories;
using Company.Session3.DAL.Data.Contexts;
using Company.Session3.DAL.Models;
using Company.Session3.PL.Mapping;
using Company.Session3.PL.Services;
using Microsoft.AspNetCore.Identity;
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
            //builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); //Allow DI for DepartmentRepository
            //builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>(); //Allow DI for EmployeeRepository

            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });  //Allow DI for CompanyDbContext

            //Life Time
            //builder.Services.AddScoped();     //Create object life time per Request -UnReachable Object
            //builder.Services.AddTransient();  //Create object life time per Operation 
            //builder.Services.AddSingleton();  //Create object life time per Application

            builder.Services.AddScoped<IScopedService , ScopedService>(); //Per Request
            builder.Services.AddTransient<ITransientService , TransientService>(); //Per Operation
            builder.Services.AddSingleton<ISingletonService, SingletonService>(); //Per App

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

//class Test()
//{
//    public void Fun01()
//    {
//        //Statment01
//        //Statment02
//        //await  //Statment03 --> Take Time
//        //Statment04
//        //Statment05
//    }

//    public void Fun02()
//    {
//        //Statment01
//        //Statment02
//        //Statment03
//        //Statment04
//        //Statment05
//    }
//}