using System;
using System.Threading.Tasks;
using Dev.Core.Domain.WebApps;
using Dev.Services.WebApps;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Api.Controllers
{
    public class WebAppController : PublicController
    {
        #region Fields
        private readonly IWebAppService _webAppService;
        #endregion

        public WebAppController(IWebAppService webAppService)
        {
            _webAppService = webAppService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List(){
            var webApps = await _webAppService.GetAllWebAppsAsync();
            return Ok(webApps);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(WebApp webApp)
        {
            await _webAppService.InsertWebAppAsync(webApp);
            return Ok(webApp);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(WebApp webApp)
        {
            await _webAppService.UpdateWebAppAsync(webApp);
            return Ok(webApp);
        }
    }
}
