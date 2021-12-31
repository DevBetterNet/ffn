using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dev.Core;
using Dev.Core.Domain.WebApps;

namespace Dev.Services.WebApps
{
    public interface IWebAppMappingService
    {
        /// <summary>
        /// Apply webApp mapping to the passed query
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports webApp mapping</typeparam>
        /// <param name="query">Query to filter</param>
        /// <param name="webAppId">WebApp identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the filtered query
        /// </returns>
        Task<IQueryable<TEntity>> ApplyWebAppMapping<TEntity>(IQueryable<TEntity> query, Guid webAppId) where TEntity : BaseEntity, IWebAppMappingSupported;

        /// <summary>
        /// Deletes a webApp mapping record
        /// </summary>
        /// <param name="webAppMapping">WebApp mapping record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteWebAppMappingAsync(WebAppMapping webAppMapping);

        /// <summary>
        /// Gets webApp mapping records
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports webApp mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the webApp mapping records
        /// </returns>
        Task<IList<WebAppMapping>> GetWebAppMappingsAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IWebAppMappingSupported;

        /// <summary>
        /// Inserts a webApp mapping record
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports webApp mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="webAppId">WebApp id</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertWebAppMappingAsync<TEntity>(TEntity entity, Guid webAppId) where TEntity : BaseEntity, IWebAppMappingSupported;

        /// <summary>
        /// Find webApp identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports webApp mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the webApp identifiers
        /// </returns>
        Task<Guid[]> GetWebAppsIdsWithAccessAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IWebAppMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in the current webApp (mapped to this webApp)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports webApp mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IWebAppMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in a webApp (mapped to this webApp)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports webApp mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="webAppId">WebApp identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync<TEntity>(TEntity entity, Guid webAppId) where TEntity : BaseEntity, IWebAppMappingSupported;
    }
}
