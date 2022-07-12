using Dev.Core;
using Dev.Core.Caching;
using Dev.Core.Configuration;
using Dev.Core.Domain.Configuration;
using Dev.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Dev.Services.Configuration;

public class SettingService : ISettingService
{
    #region Fields

    private readonly IRepository<Setting> _settingRepository;
    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor

    public SettingService(IRepository<Setting> settingRepository,
        IStaticCacheManager staticCacheManager)
    {
        _settingRepository = settingRepository;
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Gets all settings
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the settings
    /// </returns>
    protected virtual async Task<IDictionary<string, IList<Setting>>> GetAllSettingsDictionaryAsync()
    {
        return await _staticCacheManager.GetAsync(DevConfigurationDefaults.SettingsAllAsDictionaryCacheKey, async () =>
        {
            var settings = await GetAllSettingsAsync();

            var dictionary = new Dictionary<string, IList<Setting>>();
            foreach (var s in settings)
            {
                var resourceName = s.Name.ToLowerInvariant();
                var settingForCaching = new Setting
                {
                    Id = s.Id,
                    Name = s.Name,
                    Value = s.Value,
                    WebAppId = s.WebAppId
                };
                if (!dictionary.ContainsKey(resourceName))
                    //first setting
                    dictionary.Add(resourceName, new List<Setting>
                    {
                        settingForCaching
                    });
                else
                    //already added
                    //most probably it's the setting with the same name but for some certain webApp (webAppId > 0)
                    dictionary[resourceName].Add(settingForCaching);
            }

            return dictionary;
        });
    }

    /// <summary>
    /// Set setting value
    /// </summary>
    /// <param name="type">Type</param>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <param name="webAppId">WebApp identifier</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task SetSettingAsync(Type type, string key, object value, Guid webAppId = new Guid(), bool clearCache = true)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        key = key.Trim().ToLowerInvariant();
        var valueStr = TypeDescriptor.GetConverter(type).ConvertToInvariantString(value);

        var allSettings = await GetAllSettingsDictionaryAsync();
        var settingForCaching = allSettings.ContainsKey(key) ?
            allSettings[key].FirstOrDefault(x => x.WebAppId == webAppId) : null;
        if (settingForCaching != null)
        {
            //update
            var setting = await GetSettingByIdAsync(settingForCaching.Id);
            setting.Value = valueStr;
            await UpdateSettingAsync(setting, clearCache);
        }
        else
        {
            //insert
            var setting = new Setting
            {
                Name = key,
                Value = valueStr,
                WebAppId = webAppId
            };
            await InsertSettingAsync(setting, clearCache);
        }
    }
    /// <summary>
    /// Adds a setting
    /// </summary>
    /// <param name="setting">Setting</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task InsertSettingAsync(Setting setting, bool clearCache = true)
    {
        await _settingRepository.InsertAsync(setting);

        //cache
        if (clearCache)
            await ClearCacheAsync();
    }

    /// <summary>
    /// Updates a setting
    /// </summary>
    /// <param name="setting">Setting</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task UpdateSettingAsync(Setting setting, bool clearCache = true)
    {
        if (setting == null)
            throw new ArgumentNullException(nameof(setting));

        await _settingRepository.UpdateAsync(setting);

        //cache
        if (clearCache)
            await ClearCacheAsync();
    }


    #endregion

    public async Task ClearCacheAsync()
    {
        await _staticCacheManager.RemoveByPrefixAsync(DevEntityCacheDefaults<Setting>.Prefix);
    }

    public async Task DeleteSettingAsync(Setting setting)
    {
        await _settingRepository.DeleteAsync(setting);

        //cache
        await ClearCacheAsync();
    }

    public async Task DeleteSettingAsync<T>() where T : ISettings, new()
    {
        var settingsToDelete = new List<Setting>();
        var allSettings = await GetAllSettingsAsync();
        foreach (var prop in typeof(T).GetProperties())
        {
            var key = typeof(T).Name + "." + prop.Name;
            settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
        }

        await DeleteSettingsAsync(settingsToDelete);
    }

    public async Task DeleteSettingAsync<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, Guid webAppId = default) where T : ISettings, new()
    {
        var key = GetSettingKey(settings, keySelector);
        key = key.Trim().ToLowerInvariant();

        var allSettings = await GetAllSettingsDictionaryAsync();
        var settingForCaching = allSettings.ContainsKey(key) ?
            allSettings[key].FirstOrDefault(x => x.WebAppId == webAppId) : null;
        if (settingForCaching == null)
            return;

        //update
        var setting = await GetSettingByIdAsync(settingForCaching.Id);
        await DeleteSettingAsync(setting);
    }

    public async Task DeleteSettingsAsync(IList<Setting> settings)
    {
        await _settingRepository.DeleteAsync(settings);

        //cache
        await ClearCacheAsync();
    }

    public async Task<IList<Setting>> GetAllSettingsAsync()
    {
        var settings = await _settingRepository.GetAllAsync(query =>
        {
            return from s in query
                   orderby s.Name, s.WebAppId
                   select s;
        }, cache => default);

        return settings;
    }

    public async Task<Setting> GetSettingAsync(string key, Guid webAppId = default, bool loadSharedValueIfNotFound = false)
    {
        if (string.IsNullOrEmpty(key))
            return null;

        var settings = await GetAllSettingsDictionaryAsync();
        key = key.Trim().ToLowerInvariant();
        if (!settings.ContainsKey(key))
            return null;

        var settingsByKey = settings[key];
        var setting = settingsByKey.FirstOrDefault(x => x.WebAppId == webAppId);

        //load shared value?
        if (setting == null && webAppId != Guid.Empty && loadSharedValueIfNotFound)
            setting = settingsByKey.FirstOrDefault(x => x.WebAppId == Guid.Empty);

        return setting != null ? await GetSettingByIdAsync(setting.Id) : null;
    }

    public async Task<Setting> GetSettingByIdAsync(Guid settingId)
    {
        return await _settingRepository.GetByIdAsync(settingId, cache => default);
    }

    public async Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default, Guid webAppId = default, bool loadSharedValueIfNotFound = false)
    {
        if (string.IsNullOrEmpty(key))
            return defaultValue;

        var settings = await GetAllSettingsDictionaryAsync();
        key = key.Trim().ToLowerInvariant();
        if (!settings.ContainsKey(key))
            return defaultValue;

        var settingsByKey = settings[key];
        var setting = settingsByKey.FirstOrDefault(x => x.WebAppId == webAppId);

        //load shared value?
        if (setting == null && webAppId != Guid.Empty && loadSharedValueIfNotFound)
            setting = settingsByKey.FirstOrDefault(x => x.WebAppId == Guid.Empty);

        return setting != null ? CommonHelper.To<T>(setting.Value) : defaultValue;
    }

    public string GetSettingKey<TSettings, T>(TSettings settings, Expression<Func<TSettings, T>> keySelector) where TSettings : ISettings, new()
    {
        throw new NotImplementedException();
    }

    public async Task<T> LoadSettingAsync<T>(Guid webAppId = default) where T : ISettings, new()
    {
        return (T)await LoadSettingAsync(typeof(T), webAppId);
    }

    public async Task<ISettings> LoadSettingAsync(Type type, Guid webAppId = default)
    {
        var settings = Activator.CreateInstance(type);

        foreach (var prop in type.GetProperties())
        {
            // get properties we can read and write to
            if (!prop.CanRead || !prop.CanWrite)
                continue;

            var key = type.Name + "." + prop.Name;
            //load by webApp
            var setting = await GetSettingByKeyAsync<string>(key, webAppId: webAppId, loadSharedValueIfNotFound: true);
            if (setting == null)
                continue;

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                continue;

            if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                continue;

            var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

            //set property
            prop.SetValue(settings, value, null);
        }

        return settings as ISettings;
    }

    public async Task SaveSettingAsync<T>(T settings, Guid webAppId = default) where T : ISettings, new()
    {
        /* We do not clear cache after each setting update.
         * This behavior can increase performance because cached settings will not be cleared 
         * and loaded from database after each update */
        foreach (var prop in typeof(T).GetProperties())
        {
            // get properties we can read and write to
            if (!prop.CanRead || !prop.CanWrite)
                continue;

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                continue;

            var key = typeof(T).Name + "." + prop.Name;
            var value = prop.GetValue(settings, null);
            if (value != null)
                await SetSettingAsync(prop.PropertyType, key, value, webAppId, false);
            else
                await SetSettingAsync(key, string.Empty, webAppId, false);
        }

        //and now clear cache
        await ClearCacheAsync();
    }

    public async Task SaveSettingAsync<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, Guid webAppId = default, bool clearCache = true) where T : ISettings, new()
    {
        if (keySelector.Body is not MemberExpression member)
            throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

        var propInfo = member.Member as PropertyInfo;
        if (propInfo == null)
            throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

        var key = GetSettingKey(settings, keySelector);
        var value = (TPropType)propInfo.GetValue(settings, null);
        if (value != null)
            await SetSettingAsync(key, value, webAppId, clearCache);
        else
            await SetSettingAsync(key, string.Empty, webAppId, clearCache);
    }

    public async Task SaveSettingOverridablePerWebAppAsync<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, bool overrideForWebApp, Guid webAppId = default, bool clearCache = true) where T : ISettings, new()
    {
        if (overrideForWebApp || webAppId == Guid.Empty)
            await SaveSettingAsync(settings, keySelector, webAppId, clearCache);
        else if (webAppId != Guid.Empty)
            await DeleteSettingAsync(settings, keySelector, webAppId);
    }

    public async Task SetSettingAsync<T>(string key, T value, Guid webAppId = default, bool clearCache = true)
    {
        await SetSettingAsync(typeof(T), key, value, webAppId, clearCache);
    }

    public async Task<bool> SettingExistsAsync<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, Guid webAppId = default) where T : ISettings, new()
    {
        var key = GetSettingKey(settings, keySelector);

        var setting = await GetSettingByKeyAsync<string>(key, webAppId: webAppId);
        return setting != null;
    }
}
