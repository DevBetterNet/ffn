using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Core.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring services and middleware on application startup
    /// </summary>
    public interface IDevStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        /// <param name="hostEnvironment"></param>
        void Configure(IApplicationBuilder application, IWebHostEnvironment hostEnvironment);

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        int Order { get; }
    }
}