using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HomeTreatment.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run(); // vutreshno zaedno s configureWebHostDefaults kazva po default nachin za vzemane na konfiguraciqta
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
