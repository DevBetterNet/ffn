using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Web.Shop.Areas.Controllers
{
    public  class ProductController : PublicController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();

        }

        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
        public IActionResult Shipping()
        {
            return View();
        }
    }
}
