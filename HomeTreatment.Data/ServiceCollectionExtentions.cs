using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace HomeTreatment.Data
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection RegisterDataServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HomeTreatmentDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("HomeTreatmentConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<HomeTreatmentDbContext>();

            return services;

            
        }
    }
}
