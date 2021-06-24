using Microsoft.AspNetCore.Mvc;

namespace Dev.Web.UFD.Areas.Controllers
{
    public class HomeController : PublicController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "GuidGenerator");
        }
    }
}
