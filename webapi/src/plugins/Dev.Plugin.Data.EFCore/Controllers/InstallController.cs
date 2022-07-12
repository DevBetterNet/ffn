using Dev.Plugin.Data.EFCore.Data;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Plugin.Data.EFCore.Controllers;

public class InstallController : PublicController
{
    #region Fields

    private readonly IDevDbContext _devDbContext;

    #endregion Fields

    public InstallController(IDevDbContext devDbContext)
    {
        _devDbContext = devDbContext;
    }

    [HttpGet]
    public IActionResult Setup()
    {
        string message = "Database is OK";
        try
        {
            _devDbContext.SetupDatabase();
        }
        catch (System.Exception ex)
        {
            message = ex.Message;
        }
        return Ok(message);
    }

    [HttpGet()]
    [Route("scripts")]
    public IActionResult GenerateCreateScript()
    {
        return Ok(_devDbContext.GenerateCreateScript());
    }
}
