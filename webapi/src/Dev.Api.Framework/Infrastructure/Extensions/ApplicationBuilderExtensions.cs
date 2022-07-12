using Dev.Core.Configuration;
using Dev.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Dev.WebApi.Framework.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ConfigureRequestPipeline(this IApplicationBuilder application, IWebHostEnvironment environment)
    {
        AppSettings appSettings = Singleton<AppSettings>.Instance;

        EngineContext.Current.ConfigureRequestPipeline(application, environment, appSettings);
    }

    public static void UseDevEndpoints(this IApplicationBuilder application)
    {
        application.UseRouting();

        application.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}