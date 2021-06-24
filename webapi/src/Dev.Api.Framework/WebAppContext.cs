using Dev.Core;
using Dev.Core.Domain.WebApps;
using Dev.Data;
using Dev.Services.WebApps;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dev.Api.Framework
{
    public class WebAppContext : IWebAppContext
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<WebApp> _webAppRepository;
        private readonly IWebAppService _webAppService;
        private WebApp _cachedWebApp;
        private Guid _cachedActiveWebAppScopeConfiguration;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="webAppRepository">WebApp repository</param>
        /// <param name="webAppService">WebApp service</param>
        public WebAppContext(IHttpContextAccessor httpContextAccessor,
                             IRepository<WebApp> webAppRepository,
                             IWebAppService webAppService)
        {
            _httpContextAccessor = httpContextAccessor;
            _webAppRepository = webAppRepository;
            _webAppService = webAppService;
        }

        #endregion


        public async Task<Guid> GetActiveWebAppScopeConfigurationAsync()
        {
            if (_cachedActiveWebAppScopeConfiguration != Guid.Empty)
                return _cachedActiveWebAppScopeConfiguration;

            //ensure that we have 2 (or more) webApps
            if ((await _webAppService.GetAllWebAppsAsync()).Count > 1)
            {
                //do not inject IWorkContext via constructor because it'll cause circular references
                //var currentCustomer = await EngineContext.Current.Resolve<IWorkContext>().GetCurrentUserAsync();

                ////try to get webApp identifier from attributes
                //var webAppId = await _genericAttributeService
                //    .GetAttributeAsync<int>(currentCustomer, DevUserDefaults.AdminAreaWebAppScopeConfigurationAttribute);

                //_cachedActiveWebAppScopeConfiguration = (await _webAppService.GetWebAppByIdAsync(webAppId))?.Id ?? 0;
            }
            else
                _cachedActiveWebAppScopeConfiguration = Guid.Empty;

            return _cachedActiveWebAppScopeConfiguration;
        }

        public WebApp GetCurrentWebApp()
        {
            if (_cachedWebApp != null)
                return _cachedWebApp;

            //try to determine the current webApp by HOST header
            string host = _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Host];

            //we cannot call async methods here. otherwise, an application can hang. so it's a workaround to avoid that
            var allWebApps = _webAppRepository.GetAll(query =>
            {
                return from s in query orderby s.DisplayOrder, s.Id select s;
            }, cache => default);

            var webApp = allWebApps.FirstOrDefault(s => _webAppService.ContainsHostValue(s, host));

            if (webApp == null)
                //load the first found webApp
                webApp = allWebApps.FirstOrDefault();

            _cachedWebApp = webApp ?? throw new Exception("No webApp could be loaded");

            return _cachedWebApp;
        }

        public async Task<WebApp> GetCurrentWebAppAsync()
        {
            if (_cachedWebApp != null)
                return _cachedWebApp;

            //try to determine the current webApp by HOST header
            string host = _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Host];

            var allWebApps = await _webAppService.GetAllWebAppsAsync();
            var webApp = allWebApps.FirstOrDefault(s => _webAppService.ContainsHostValue(s, host));

            if (webApp == null)
                //load the first found webApp
                webApp = allWebApps.FirstOrDefault();

            _cachedWebApp = webApp ?? throw new Exception("No WebApp could be loaded");

            return _cachedWebApp;
        }
    }
}
