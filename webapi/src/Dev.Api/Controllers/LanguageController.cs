using System.Threading.Tasks;
using Dev.Core.Domain.Localization;
using Dev.Services.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Api.Controllers;

public class LanguageController : PublicController
{
    #region Fields
    private readonly ILanguageService _languageService; 
    #endregion

    public LanguageController(ILanguageService languageService )
    {
        _languageService = languageService;
    }

    
    [HttpGet("List")]
    public async Task<IActionResult> List(){
        var languages = await _languageService.GetAllLanguagesAsync();
        return Ok(languages);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync(Language language)
    {
        await _languageService.InsertLanguageAsync(language);
        return Ok(language);
    }
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateAsync(Language language)
    {
        await _languageService.UpdateLanguageAsync(language);
        return Ok(language);
    }
}
