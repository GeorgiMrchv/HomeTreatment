using HomeTreatment.Web.BusinessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HomeTreatment.Data;
using HomeTreatment.Data.Repository;
using HomeTreatment.Web.Sample_test;
using HomeTreatment.Business.Services;

namespace HomeTreatment.Web
{
    public class Startup
    {      

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<OfficeEquipmentConfig>(Configuration.GetSection("OfficeEquipmentConfig")); // appsettings + class

            services.AddControllersWithViews();

            services.RegisterDataServices(Configuration);

            services.AddScoped<IRepository, Repository>(); // pri service az trqbva da definiram scope, glaven scope

            // From: HomeTreatment.Business
            services.AddTransient<IUsersService, Users>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.LogoutPath = "/Authentication/Logout";
            });

            services.Configure<LoadHistory>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting(); 

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Administration",
                    pattern: "Administration/EditAdministrator/{id}",
                    defaults: new { controller = "Admin", action = "Edit" }
                    );


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                
            });
        }
    }
}

//mapcontrollerroute - mvc go dobavq za da moje da mi naglasi avtomati4no route-ovete podhodqshti za kontrollerite
// po spicifi4nite nad po obshtite


