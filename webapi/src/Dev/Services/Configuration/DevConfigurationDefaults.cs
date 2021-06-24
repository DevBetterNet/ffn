using Dev.Core.Caching;
using Dev.Core.Domain.Configuration;

namespace Dev.Services.Configuration
{
    /// <summary>
    /// Represents default values related to configuration services
    /// </summary>
    public static partial class DevConfigurationDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllAsDictionaryCacheKey => new CacheKey("Dev.setting.all.dictionary.", DevEntityCacheDefaults<Setting>.Prefix);

        #endregion

        /// <summary>
        /// Gets the path to file that contains app settings
        /// </summary>
        public static string AppSettingsFilePath => "App_Data/appsettings.json";
    }
}
