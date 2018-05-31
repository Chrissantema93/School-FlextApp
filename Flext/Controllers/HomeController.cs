using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flext.Models;

namespace Flext.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Overzicht()
        {

            TafelStatus Status = new TafelStatus { Tafelnaam = "TestTafel", Stoelen = new List<StoelInfo>() };
            for (int i = 0; i < 8; i++)
            {
                Status.Stoelen.Add(new StoelInfo { Bezet = TorF() });
            }
            return View(Status);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool TorF()
        {
            return (new Random().Next(100) < 50);
        }
    }
}
