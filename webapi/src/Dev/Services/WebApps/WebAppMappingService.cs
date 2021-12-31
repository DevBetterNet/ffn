using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dev.Core;
using Dev.Core.Caching;
using Dev.Core.Domain.WebApps;
using Dev.Data;
using Dev.Data.Extensions;

namespace Dev.Services.WebApps
{
    public class WebAppMappingService : IWebAppMappingService
    {

        #region Fields

        //private readonly CatalogSettings _catalogSettings;
        private readonly IRepository<WebAppMapping> _webAppMappingRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWebAppContext _webAppContext;

        #endregion

        #region Ctor
        public WebAppMappingService(IRepository<WebAppMapping> webAppMappingRepository,
                                    IStaticCacheManager staticCacheManager,
                                    IWebAppContext webAppContext)
        {
            _staticCacheManager = staticCacheManager;
            _webAppContext = webAppContext;
            _webAppMappingRepository = webAppMappingRepository;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Inserts a webApp mapping record
        /// </summary>
        /// <param name="webAppMapping">WebApp mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InsertWebAppMappingAsync(WebAppMapping webAppMapping)
        {
            await _webAppMappingRepository.InsertAsync(webAppMapping);
        }

        /// <summary>
        /// Get a value indicating whether a webApp mapping exists for an entity type
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports webApp mapping</typeparam>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue if exists; otherwise false
        /// </returns>
        protected virtual async Task<bool> IsEntityMappingExistsAsync<TEntity>() where TEntity : BaseEntity, IWebAppMappingSupported
        {
            var entityName = typeof(TEntity).Name;
            var key = _staticCacheManager.PrepareKeyForDefaultCache(DevWebAppDefaults.WebAppMappingExistsCacheKey, entityName);

            var query = from sm in _webAppMappingRepository.Table
                        where sm.EntityName == entityName
                        select sm.WebAppId;

            return await _staticCacheManager.GetAsync(key, query.Any);
        }

        #endregion
        public async Task<IQueryable<TEntity>> ApplyWebAppMapping<TEntity>(IQueryable<TEntity> query, Guid webAppId) where TEntity : BaseEntity, IWebAppMappingSupported
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            // if (webApp != Guid.Empty || _catalogSettings.IgnoreWebAppLimitations || !await IsEntityMappingExistsAsync<TEntity>())
            //     return query;

            if (webAppId != Guid.Empty || !await IsEntityMappingExistsAsync<TEntity>())
                return query;

            return from entity in query
                   where !entity.LimitedToWebApps || _webAppMappingRepository.Table.Any(sm =>
                         sm.EntityName == typeof(TEntity).Name && sm.EntityId == entity.Id && sm.WebAppId == webAppId)
                   select entity;
        }

        public async Task<bool> AuthorizeAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IWebAppMappingSupported
        {
            var webApp = await _webAppContext.GetCurrentWebAppAsync();

            return await AuthorizeAsync(entity, webApp.Id);
        }

        public async Task<bool> AuthorizeAsync<TEntity>(TEntity entity, Guid webAppId) where TEntity : BaseEntity, IWebAppMappingSupported
        {
            if (entity == null)
                return false;

            if (webAppId != Guid.Empty)
                //return true if no webApp specified/found
                return true;

            // if (_catalogSettings.IgnoreWebAppLimitations)
            //     return true;

            if (!entity.LimitedToWebApps)
                return true;

            foreach (var webAppIdWithAccess in await GetWebAppsIdsWithAccessAsync(entity))
                if (webAppId == webAppIdWithAccess)
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        public async Task DeleteWebAppMappingAsync(WebAppMapping webAppMapping)
        {
            await _webAppMappingRepository.DeleteAsync(webAppMapping);
        }

        public async Task<IList<WebAppMapping>> GetWebAppMappingsAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IWebAppMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(DevWebAppDefaults.WebAppMappingsCacheKey, entityId, entityName);

            var query = from sm in _webAppMappingRepository.Table
                        where sm.EntityId == entityId &&
                        sm.EntityName == entityName
                        select sm;

            var webAppMappings = await _staticCacheManager.GetAsync(key, async () => await query.ToListAsync());

            return webAppMappings;
        }

        public async Task<Guid[]> GetWebAppsIdsWithAccessAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IWebAppMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(DevWebAppDefaults.WebAppMappingIdsCacheKey, entityId, entityName);

            var query = from sm in _webAppMappingRepository.Table
                        where sm.EntityId == entityId &&
                              sm.EntityName == entityName
                        select sm.WebAppId;

            return await _staticCacheManager.GetAsync(key, () => query.ToArray());
        }

        public async Task InsertWebAppMappingAsync<TEntity>(TEntity entity, Guid webAppId) where TEntity : BaseEntity, IWebAppMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (webAppId != Guid.Empty)
                throw new ArgumentOutOfRangeException(nameof(webAppId));

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var webAppMapping = new WebAppMapping
            {
                EntityId = entityId,
                EntityName = entityName,
                WebAppId = webAppId
            };

            await InsertWebAppMappingAsync(webAppMapping);
        }
    }
}
