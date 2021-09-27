using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Web.Shop.Areas.Controllers
{
    public class ChatController : PublicController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
