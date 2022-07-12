using Dev.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Core.Infrastructure;

/// <summary>
/// Represents object for the configuring services and middleware on application startup
/// </summary>
public interface IDevStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    void ConfigureServices(IServiceCollection services, IConfiguration configuration, AppSettings appSettings);

    /// <summary>
    /// Configure the using of added middleware
    void Configure(IApplicationBuilder application, IWebHostEnvironment hostEnvironment, AppSettings appSettings);

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    int Order { get; }
}