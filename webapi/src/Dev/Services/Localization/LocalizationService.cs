using Dev.Core;
using Dev.Core.Configuration;
using Dev.Core.Domain.Localization;
using Dev.Core.Security;
using Dev.Services.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Services.Localization;

public class LocalizationService : ILocalizationService
{
    #region Fields

    #endregion

    #region Ctor
    public LocalizationService()
    {

    }
    #endregion

    public Task AddLocaleResourceAsync(IDictionary<string, string> resources, Guid languageId = default)
    {
        throw new NotImplementedException();
    }

    public Task AddOrUpdateLocaleResourceAsync(string resourceName, string resourceValue, string languageCulture = null)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLocaleResourceAsync(string resourceName)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLocaleResourcesAsync(IList<string> resourceNames, Guid languageId = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLocaleResourcesAsync(string resourceNamePrefix, Guid languageId = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLocaleStringResourceAsync(LocaleStringResource localeStringResource)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLocalizedPermissionNameAsync(PermissionRecord permissionRecord)
    {
        throw new NotImplementedException();
    }

    public Task<string> ExportResourcesToXmlAsync(Language language)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, KeyValuePair<Guid, string>>> GetAllResourceValuesAsync(Guid languageId, bool? loadPublicLocales)
    {
        throw new NotImplementedException();
    }

    public Task<LocaleStringResource> GetLocaleStringResourceByIdAsync(Guid localeStringResourceId)
    {
        throw new NotImplementedException();
    }

    public Task<LocaleStringResource> GetLocaleStringResourceByNameAsync(string resourceName, Guid languageId, bool logIfNotFound = true)
    {
        throw new NotImplementedException();
    }

    public Task<TPropType> GetLocalizedAsync<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector, Guid languageId = default, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true) where TEntity : BaseEntity, ILocalizedEntity
    {
        throw new NotImplementedException();
    }

    public Task<string> GetLocalizedEnumAsync<TEnum>(TEnum enumValue, Guid languageId = default) where TEnum : struct
    {
        throw new NotImplementedException();
    }

    public Task<string> GetLocalizedFriendlyNameAsync<TPlugin>(TPlugin plugin, int languageId, bool returnDefaultValue = true) where TPlugin : IPlugin
    {
        throw new NotImplementedException();
    }

    public Task<string> GetLocalizedPermissionNameAsync(PermissionRecord permissionRecord, Guid languageId = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetLocalizedSettingAsync<TSettings>(TSettings settings, Expression<Func<TSettings, string>> keySelector, Guid languageId, Guid webAppId, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true) where TSettings : ISettings, new()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetResourceAsync(string resourceKey)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetResourceAsync(string resourceKey, Guid languageId, bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false)
    {
        throw new NotImplementedException();
    }

    public Task ImportResourcesFromXmlAsync(Language language, StreamReader xmlStreamReader, bool updateExistingResources = true)
    {
        throw new NotImplementedException();
    }

    public Task InsertLocaleStringResourceAsync(LocaleStringResource localeStringResource)
    {
        throw new NotImplementedException();
    }

    public Task SaveLocalizedFriendlyNameAsync<TPlugin>(TPlugin plugin, Guid languageId, string localizedFriendlyName) where TPlugin : IPlugin
    {
        throw new NotImplementedException();
    }

    public Task SaveLocalizedPermissionNameAsync(PermissionRecord permissionRecord)
    {
        throw new NotImplementedException();
    }

    public Task SaveLocalizedSettingAsync<TSettings>(TSettings settings, Expression<Func<TSettings, string>> keySelector, int languageId, string value) where TSettings : ISettings, new()
    {
        throw new NotImplementedException();
    }

    public Task UpdateLocaleStringResourceAsync(LocaleStringResource localeStringResource)
    {
        throw new NotImplementedException();
    }
}
