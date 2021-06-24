using System;
using Dev.Core.Caching;
using Dev.Core.Domain.Localization;

namespace Dev.Services.Localization
{
    public class DevLocalizationDefaults
    {
        #region Locales

        /// <summary>
        /// Gets a prefix of locale resources for the admin area
        /// </summary>
        public static string AdminLocaleStringResourcesPrefix => "Admin.";

        /// <summary>
        /// Gets a prefix of locale resources for enumerations 
        /// </summary>
        public static string EnumLocaleStringResourcesPrefix => "Enums.";

        /// <summary>
        /// Gets a prefix of locale resources for permissions 
        /// </summary>
        public static string PermissionLocaleStringResourcesPrefix => "Permission.";

        /// <summary>
        /// Gets a prefix of locale resources for plugin friendly names 
        /// </summary>
        public static string PluginNameLocaleStringResourcesPrefix => "Plugins.FriendlyName.";

        #endregion

        #region Caching defaults

        #region Languages

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : webApp ID
        /// {1} : show hidden records?
        /// </remarks>
        public static CacheKey LanguagesAllCacheKey => new("Dev.language.all.{0}-{1}", LanguagesByStorePrefix, DevEntityCacheDefaults<Language>.AllPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : webApp ID
        /// </remarks>
        public static string LanguagesByStorePrefix => "Dev.language.all.{0}";

        #endregion

        #region Locales

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public static CacheKey LocaleStringResourcesAllPublicCacheKey => new("Dev.localestringresource.bylanguage.public.{0}", DevEntityCacheDefaults<LocaleStringResource>.Prefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public static CacheKey LocaleStringResourcesAllAdminCacheKey => new("Dev.localestringresource.bylanguage.admin.{0}", DevEntityCacheDefaults<LocaleStringResource>.Prefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public static CacheKey LocaleStringResourcesAllCacheKey => new("Dev.localestringresource.bylanguage.{0}", DevEntityCacheDefaults<LocaleStringResource>.Prefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : resource key
        /// </remarks>
        public static CacheKey LocaleStringResourcesByNameCacheKey => new("Dev.localestringresource.byname.{0}-{1}", LocaleStringResourcesByNamePrefix, DevEntityCacheDefaults<LocaleStringResource>.Prefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public static string LocaleStringResourcesByNamePrefix => "Dev.localestringresource.byname.{0}";

        #endregion

        #region Localized properties

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : entity ID
        /// {2} : locale key group
        /// {3} : locale key
        /// </remarks>
        public static CacheKey LocalizedPropertyCacheKey => new("Dev.localizedproperty.value.{0}-{1}-{2}-{3}");

        #endregion

        #endregion
    }
}
