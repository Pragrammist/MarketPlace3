using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HukSleva.Models;
using Microsoft.EntityFrameworkCore;
using HukSleva.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using UserDataBase;
using HukSleva.Services;
using HukSleva.ViewModels.BookController;

namespace HukSleva
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
            services.AddControllersWithViews();   
            string connection = Configuration.GetConnectionString("DefaultConnection");
            string connectionFilesDb = Configuration.GetConnectionString("FilesDb");
            services.AddDbContext<UserDb>(options => options.UseSqlServer(connection));
            services.AddScoped<AuthoValidationAttribute>();
            services.AddScoped<UserSelfInfModelAttribute>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/Index");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/SelfInfo");
                });
            services.AddDbContext<UserFilesDb>(options => options.UseSqlServer(connectionFilesDb));
            services.AddAuthorization(opts => {
                opts.AddPolicy("member", policy => {
                    policy.RequireRole("moderator", "admin");
                });
                opts.AddPolicy("all", policy => {
                    policy.RequireRole("modertor", "admin", "user");
                });
                opts.AddPolicy("isAdulthood", p => 
                {
                    p.RequireAssertion(context => 
                    {
                        var claimValue = context.User.Claims.FirstOrDefault(claim => claim.Type == "dateBirthday")?.Value;
                        if (claimValue == null)
                            return false;

                        var TodayDate = DateTime.Now;
                        var adulthoodDate = new DateTime(TodayDate.Year - 18, TodayDate.Month, TodayDate.Day);
                        var birthdayDate = DateTime.Parse(claimValue);
                        
                        if (birthdayDate <= adulthoodDate)
                        {
                            return true;
                        }
                        return false;
                    });
                });

            });
            services.AddSingleton<BookViewModelsCollectionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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
            //AccountController => Страницы для регестрации, авторизации, аунтефикации, акаунта
            //BooksController => Каталог, добавление книг
            //HomeController => Начальная страница

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            
        }
    }
}
