using Microsoft.AspNetCore.Mvc;

namespace Dev.WebApiFramework.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
    }
}