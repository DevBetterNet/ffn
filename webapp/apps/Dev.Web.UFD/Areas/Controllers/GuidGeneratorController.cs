using Dev.Web.UFD.Areas.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Dev.Web.UFD.Areas.Controllers
{
    public class GuidGeneratorController : PublicController
    {
        public IActionResult Index()
        {
            GuidGeneratorModel guidGeneratorModel = new GuidGeneratorModel();
            return View(guidGeneratorModel);
        }

        [HttpPost]
        public IActionResult Index(GuidGeneratorModel model)
        {
            string guidsInString = string.Empty;
            for (int i = 0; i < model.NumberOfGuids; i++)
            {
                string guidItem = Guid.NewGuid().ToString();

                if (model.IsUpper)
                {
                    guidItem = guidItem.ToUpper();
                }

                if (model.HasHypens == false)
                {
                    guidItem = guidItem.Replace("-", string.Empty);
                }

                if (model.HasBraces)
                {
                    guidItem = "{" + guidItem + "}";
                }

                guidsInString += guidItem + System.Environment.NewLine;
            }

            model.GuidGenerateInString = guidsInString;
            return View(model);
        }
    }
}
