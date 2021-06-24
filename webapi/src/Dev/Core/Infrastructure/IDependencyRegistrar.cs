using Dev.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Core.Infrastructure
{
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings);

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        int Order { get; }
    }
}