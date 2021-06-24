using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dev.Plugin.Sys.Core.Controllers
{
    public class HomeController : PublicController
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok("Get");
        }

    }
}
