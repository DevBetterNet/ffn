using Dev.Core;
using Dev.Core.Configuration;
using Dev.Core.Infrastructure;
using Dev.Services.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace Dev.WebApi.Framework.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services,
           IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {

            //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //create default file provider
            CommonHelper.DefaultFileProvider = new DevFileProvider(webHostEnvironment);
            //services.AddScoped<IDevFileProvider>(f => fileProvider);

            //var config = services.ConfigureStartupConfig<DevConfig>(configuration.GetSection("Dev"));
            //add configuration parameters
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            services.AddSingleton(appSettings);
            AppSettingsHelper.SaveAppSettings(appSettings);

            //initialize plugins
            var mvcCoreBuilder = services.AddMvcCore();
            mvcCoreBuilder.PartManager.InitializePlugins(appSettings);

            //create engine and configure service provider
            var engine = EngineContext.Create();
            engine.ConfigureServices(services, configuration);
            engine.RegisterDependencies(services, appSettings);
        }

        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);
            return config;
        }
    }
}