using Dev.Plugin.Sys.Auth.Models;
using Dev.Plugin.Sys.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Plugin.Sys.Auth.Controllers;

public class UserController : ControllerBase
{


    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Login(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Email or password is incorrect" });

        return Ok(response);
    }
}
