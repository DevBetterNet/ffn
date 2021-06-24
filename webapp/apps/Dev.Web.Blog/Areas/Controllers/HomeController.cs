using Microsoft.AspNetCore.Mvc;

namespace Dev.Web.Blog.Controllers
{
    [Area("Blog")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
