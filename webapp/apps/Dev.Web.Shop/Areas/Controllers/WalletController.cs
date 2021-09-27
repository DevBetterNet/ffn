using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Web.Shop.Areas.Controllers
{
    public class WalletController : PublicController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Transactions()
        {
            return View();
        }

        public IActionResult Credit()
        {
            return View();
        }

        public IActionResult Bank()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }
}
