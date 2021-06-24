using Dev.Core.Domain.WebApps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dev.Services.WebApps
{
    public interface IWebAppService
    {
        /// <summary>
        /// Deletes a webApp
        /// </summary>
        /// <param name="webApp">WebApp</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteWebAppAsync(WebApp webApp);

        /// <summary>
        /// Gets all webApps
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the webApps
        /// </returns>
        Task<IList<WebApp>> GetAllWebAppsAsync();

        /// <summary>
        /// Gets a webApp 
        /// </summary>
        /// <param name="webAppId">WebApp identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the webApp
        /// </returns>
        Task<WebApp> GetWebAppByIdAsync(Guid webAppId);

        /// <summary>
        /// Inserts a webApp
        /// </summary>
        /// <param name="webApp">WebApp</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertWebAppAsync(WebApp webApp);

        /// <summary>
        /// Updates the webApp
        /// </summary>
        /// <param name="webApp">WebApp</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateWebAppAsync(WebApp webApp);

        /// <summary>
        /// Indicates whether a webApp contains a specified host
        /// </summary>
        /// <param name="webApp">WebApp</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        bool ContainsHostValue(WebApp webApp, string host);

        /// <summary>
        /// Returns a list of names of not existing webApps
        /// </summary>
        /// <param name="webAppIdsNames">The names and/or IDs of the webApp to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of names and/or IDs not existing webApps
        /// </returns>
        Task<string[]> GetNotExistingWebAppsAsync(string[] webAppIdsNames);
    }
}
