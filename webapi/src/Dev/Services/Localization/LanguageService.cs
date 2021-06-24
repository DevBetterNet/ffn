using Dev.Core.Caching;
using Dev.Core.Domain.Localization;
using Dev.Data;
using Dev.Data.Extensions;
using Dev.Services.Configuration;
using Dev.Services.WebApps;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Dev.Services.Localization
{
    public class LanguageService : ILanguageService
    {
        #region Fields

        private readonly IRepository<Language> _languageRepository;
        private readonly ISettingService _settingService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWebAppMappingService _webAppMappingService;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        public LanguageService(IRepository<Language> languageRepository,
                               ISettingService settingService,
                               IStaticCacheManager staticCacheManager,
                               IWebAppMappingService webAppMappingService,
                               LocalizationSettings localizationSettings)
        {
            _languageRepository = languageRepository;
            _settingService = settingService;
            _staticCacheManager = staticCacheManager;
            _webAppMappingService = webAppMappingService;
            _localizationSettings = localizationSettings;
        }

        public virtual async Task DeleteLanguageAsync(Language language)
        {
             if (language == null)
                throw new ArgumentNullException(nameof(language));
            
            //update default admin area language (if required)
            if (_localizationSettings.DefaultAdminLanguageId == language.Id)
                foreach (var activeLanguage in await GetAllLanguagesAsync())
                {
                    if (activeLanguage.Id == language.Id) 
                        continue;

                    _localizationSettings.DefaultAdminLanguageId = activeLanguage.Id;
                    await _settingService.SaveSettingAsync(_localizationSettings);
                    break;
                }

            await _languageRepository.DeleteAsync(language);
        }

        public virtual async Task<IList<Language>> GetAllLanguagesAsync(bool showHidden = false, Guid webAppId = default)
        {
           //cacheable copy
            var key = _staticCacheManager.PrepareKeyForDefaultCache(DevLocalizationDefaults.LanguagesAllCacheKey, webAppId, showHidden);
            
            var languages = await _staticCacheManager.GetAsync(key, async () =>
            {
                var allLanguages = await _languageRepository.GetAllAsync(query =>
                {
                    if (!showHidden)
                        query = query.Where(l => l.Published);
                    query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);

                    return query;
                });

                //store mapping
                if (webAppId != Guid.Empty)
                    allLanguages = await allLanguages
                        .WhereAwait(async l => await _webAppMappingService.AuthorizeAsync(l, webAppId))
                        .ToListAsync();

                return allLanguages;
            });

            return languages;
        }

        public async Task<Language> GetLanguageByIdAsync(Guid languageId)
        {
           return await _languageRepository.GetByIdAsync(languageId, cache => default);
        }

        public string GetTwoLetterIsoLanguageName(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (string.IsNullOrEmpty(language.LanguageCulture))
                return "en";

            var culture = new CultureInfo(language.LanguageCulture);
            var code = culture.TwoLetterISOLanguageName;

            return string.IsNullOrEmpty(code) ? "en" : code;
        }

        public async Task InsertLanguageAsync(Language language)
        {
             await _languageRepository.InsertAsync(language);
        }

        public async Task UpdateLanguageAsync(Language language)
        {
             await _languageRepository.UpdateAsync(language);
        }
    }
}
