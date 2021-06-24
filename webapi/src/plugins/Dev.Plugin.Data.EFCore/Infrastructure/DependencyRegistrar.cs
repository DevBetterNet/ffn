using Dev.Core.Configuration;
using Dev.Core.Infrastructure;
using Dev.Data;
using Dev.Plugin.Data.EFCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Plugin.Data.EFCore.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 999;

        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            //database
            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();
            DatabaseConfig databaseConfig = new DatabaseConfig();
            if (appSettings.AdditionalData.ContainsKey("Databases"))
            {
                databaseConfig = appSettings.AdditionalData["Databases"].ToObject<DatabaseConfig>();
            }

            if (databaseConfig.DataProvider == "mysql")
            {
                dbContextOptionsBuilder.UseMySql(databaseConfig.DataConnectionString, ServerVersion.AutoDetect(databaseConfig.DataConnectionString));

                services.AddScoped<IDevDbContext>(p => new DevDbContext(dbContextOptionsBuilder.Options));
            }

            services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
        }
    }
}
