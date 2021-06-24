using Dev.Core.Configuration;
using Dev.Core.Infrastructure;
using Dev.Plugin.App.Blog.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Plugin.App.Blog.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 50;

        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddScoped<IBlogService, BlogService>();
        }
    }
}
