using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using W09.Data;
using W09.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using W09.Helpers;

namespace W09
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {


            var configuration = ConfigurationHelper.GetConfiguration();


            //register dbcontext with SQL Server and lazy loading proxies

            services.AddDbContext<GameContext>(options =>
                options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLazyLoadingProxies()
            );

            services.AddTransient<GameEngine>();
            services.AddTransient<Menu>();
        }
    }
}
