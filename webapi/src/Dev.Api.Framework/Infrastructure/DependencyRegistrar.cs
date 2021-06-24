using Dev.Core;
using Dev.Core.Caching;
using Dev.Core.Configuration;
using Dev.Core.Infrastructure;
using Dev.Services.Configuration;
using Dev.Services.Localization;
using Dev.Services.Users;
using Dev.Services.WebApps;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Dev.Api.Framework.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 0;

        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddSingleton<IDevFileProvider, DevFileProvider>();

            //cache manager
            services.AddSingleton<ILocker, MemoryCacheManager>();
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();

            //work context
            services.AddScoped<IWorkContext, WebWorkContext>();
            //web app context
            services.AddScoped<IWebAppContext, WebAppContext>();

            //services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IWebAppMappingService, WebAppMappingService>();
            services.AddScoped<IWebAppService, WebAppService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILocalizationService, LocalizationService>();

            //register all settings
            var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
            foreach (var setting in settings)
            {
                services.AddScoped(setting, serviceProvider =>
                {
                    Guid webAppId = serviceProvider.GetRequiredService<IWebAppContext>().GetCurrentWebApp()?.Id ?? Guid.Empty;
                    return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting, webAppId).Result;
                });
            }
        }
    }
}
