using Dev.Core.Configuration;
using Dev.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Plugin.Sys.Core.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 10;

        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {

        }
    }
}
