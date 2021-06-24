using Dev.Core.Domain.WebApps;
using System;
using System.Threading.Tasks;

namespace Dev.Core
{
    public interface IWebAppContext
    {
        /// <summary>
        /// Gets the current store
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<WebApp> GetCurrentWebAppAsync();

        /// <summary>
        /// Gets the current store
        /// </summary>
        WebApp GetCurrentWebApp();

        /// <summary>
        /// Gets active store scope configuration
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<Guid> GetActiveWebAppScopeConfigurationAsync();
    }
}
