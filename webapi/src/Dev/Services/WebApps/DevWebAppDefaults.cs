using System;
using Dev.Core.Caching;

namespace Dev.Services.WebApps
{
    public static class DevWebAppDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey WebAppMappingIdsCacheKey => new("Dev.webAppmapping.ids.{0}-{1}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey WebAppMappingsCacheKey => new("Dev.webAppmapping.{0}-{1}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity name
        /// </remarks>
        public static CacheKey WebAppMappingExistsCacheKey => new("Dev.webAppmapping.exists.{0}");

        #endregion
    }
}
