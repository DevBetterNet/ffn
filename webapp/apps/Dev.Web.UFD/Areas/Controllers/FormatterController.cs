using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Web.UFD.Areas.Controllers
{
    public class FormatterController : PublicController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
