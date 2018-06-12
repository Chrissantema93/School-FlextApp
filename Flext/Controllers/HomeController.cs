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


        IDescriptionRepository DesciptionRepo;
        public HomeController(IDescriptionRepository idr)
        {
            DesciptionRepo = idr;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Overzicht()
        {
            //hier worden alle detecties uit de databaase gehaald die niet ouder zijn dan 5 minuten
            var query = DesciptionRepo.Detecties.Where(x => x.Timestamp > DateTime.Now.AddMinutes(-5));


            TafelStatus Status = new TafelStatus { Tafelnaam = "TestTafel", Stoelen = new List<StoelInfo>() };
            for (int i = 0; i < 8; i++)
            {
                // wat die hier doet is hij kijkt in de query die hierboven wordt uitgevoerd of er een record is voor de specifieke stoel, 
                // hij kijkt NIET naar de inhoud van record (de tags).
                // zolang de record er is zet hij hem op bezet.
                // hier moet dus de SVM tussen komen doormiddel van een api call ofzo. 
                // er moet dus nog in worden gebouwd dat er naar de inhoud word gekeken en aan de hand daarvan op bezet worden gezet.


                //string jsonstring = DescriptRepo.Detecties.FirstOrDefault().Tags;

                //return Ok("http://svmtesting.azurewebsites.net/api/values?jsontags=" + jsonstring);


                if (query.Where(x => x.StoelId == i+1).FirstOrDefault() != null)
                {
                    Status.Stoelen.Add(new StoelInfo { Bezet = true });
                }
                else
                {
                    Status.Stoelen.Add(new StoelInfo { Bezet = false });
                }
                
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
