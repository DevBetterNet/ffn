using Dev.Core.Configuration;
using Dev.Core.Infrastructure;
using Dev.WebApi.Framework.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Api.Framework.Infrastructure
{
    public class CommonStartup : IDevStartup
    {
        public int Order => 9999; //should be loaded last

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment, AppSettings appSettings)
        {
            //Endpoints routing
            application.UseDevEndpoints();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, AppSettings appSettings)
        {
            //core api template
            services.AddControllers();

            services.AddMemoryCache();
        }
    }
}
