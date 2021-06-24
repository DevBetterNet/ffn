using Dev.Core.Domain.WebApps;
using Dev.Data;
using Dev.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dev.Services.WebApps
{
    public class WebAppService : IWebAppService
    {
        #region Fields

        private readonly IRepository<WebApp> _webAppRepository;

        #endregion

        #region Ctor

        public WebAppService(IRepository<WebApp> webAppRepository)
        {
            _webAppRepository = webAppRepository;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Parse comma-separated Hosts
        /// </summary>
        /// <param name="webApp">WebApp</param>
        /// <returns>Comma-separated hosts</returns>
        protected virtual string[] ParseHostValues(WebApp webApp)
        {
            if (webApp == null)
                throw new ArgumentNullException(nameof(webApp));

            var parsedValues = new List<string>();
            if (string.IsNullOrEmpty(webApp.Hosts))
                return parsedValues.ToArray();

            var hosts = webApp.Hosts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var host in hosts)
            {
                var tmp = host.Trim();
                if (!string.IsNullOrEmpty(tmp))
                    parsedValues.Add(tmp);
            }

            return parsedValues.ToArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a webApp
        /// </summary>
        /// <param name="webApp">WebApp</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteWebAppAsync(WebApp webApp)
        {
            if (webApp == null)
                throw new ArgumentNullException(nameof(webApp));

            var allWebApps = await GetAllWebAppsAsync();
            if (allWebApps.Count == 1)
                throw new Exception("You cannot delete the only configured webApp");

            await _webAppRepository.DeleteAsync(webApp);
        }

        /// <summary>
        /// Gets all webApps
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the webApps
        /// </returns>
        public virtual async Task<IList<WebApp>> GetAllWebAppsAsync()
        {
            var result = await _webAppRepository.GetAllAsync(query =>
            {
                return from s in query orderby s.DisplayOrder, s.Id select s;
            }, cache => default);

            return result;
        }

        /// <summary>
        /// Gets a webApp 
        /// </summary>
        /// <param name="webAppId">WebApp identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the webApp
        /// </returns>
        public virtual async Task<WebApp> GetWebAppByIdAsync(Guid webAppId)
        {
            return await _webAppRepository.GetByIdAsync(webAppId, cache => default);
        }

        /// <summary>
        /// Inserts a webApp
        /// </summary>
        /// <param name="webApp">WebApp</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertWebAppAsync(WebApp webApp)
        {
            await _webAppRepository.InsertAsync(webApp);
        }

        /// <summary>
        /// Updates the webApp
        /// </summary>
        /// <param name="webApp">WebApp</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateWebAppAsync(WebApp webApp)
        {
            await _webAppRepository.UpdateAsync(webApp);
        }

        /// <summary>
        /// Indicates whether a webApp contains a specified host
        /// </summary>
        /// <param name="webApp">WebApp</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        public virtual bool ContainsHostValue(WebApp webApp, string host)
        {
            if (webApp == null)
                throw new ArgumentNullException(nameof(webApp));

            if (string.IsNullOrEmpty(host))
                return false;

            var contains = ParseHostValues(webApp).Any(x => x.Equals(host, StringComparison.InvariantCultureIgnoreCase));

            return contains;
        }

        /// <summary>
        /// Returns a list of names of not existing webApps
        /// </summary>
        /// <param name="webAppIdsNames">The names and/or IDs of the webApp to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of names and/or IDs not existing webApps
        /// </returns>
        public async Task<string[]> GetNotExistingWebAppsAsync(string[] webAppIdsNames)
        {
            if (webAppIdsNames == null)
                throw new ArgumentNullException(nameof(webAppIdsNames));

            var query = _webAppRepository.Table;
            var queryFilter = webAppIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = await query.Select(webApp => webApp.Name)
                .Where(webApp => queryFilter.Contains(webApp))
                .ToListAsync();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (!queryFilter.Any())
                return queryFilter.ToArray();

            //filtering by IDs
            filter = await query.Select(webApp => webApp.Id.ToString())
                .Where(webApp => queryFilter.Contains(webApp))
                .ToListAsync();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter.ToArray();
        }

        #endregion
    }
}
