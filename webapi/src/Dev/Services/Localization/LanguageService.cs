using Dev.Core.Caching;
using Dev.Core.Domain.Localization;
using Dev.Data;
using Dev.Services.Configuration;
using Dev.Services.WebApps;
using System;
using System.Collections.Generic;
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

        public Task DeleteLanguageAsync(Language language)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Language>> GetAllLanguagesAsync(bool showHidden = false, Guid webAppId = default)
        {
            throw new NotImplementedException();
        }

        public Task<Language> GetLanguageByIdAsync(Guid languageId)
        {
            throw new NotImplementedException();
        }

        public string GetTwoLetterIsoLanguageName(Language language)
        {
            throw new NotImplementedException();
        }

        public Task InsertLanguageAsync(Language language)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLanguageAsync(Language language)
        {
            throw new NotImplementedException();
        }
    }
}
