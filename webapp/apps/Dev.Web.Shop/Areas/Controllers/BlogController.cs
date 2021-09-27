using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Web.Shop.Areas.Controllers
{
    public class BlogController : PublicController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Item()
        {
            return View();
        }
    }
}
