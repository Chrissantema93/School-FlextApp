using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flext.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Flext.Controllers
{
    public class HomeController : Controller
    {

        static HttpClient client = new HttpClient();
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
        public async Task<IActionResult> Overzicht()
        {
            double result;

            IEnumerable<ImageDescription> detections = DesciptionRepo.Detecties
                .GroupBy(x => x.StoelId)
                .Select(g => g.First())
                .Distinct();

            Console.WriteLine("sequence bevatte: " + detections.Count() + " elemeten");

            TafelStatus Status = new TafelStatus { Tafelnaam = "TestTafel", Stoelen = new List<StoelInfo>() };
            for (int i = 0; i < 8; i++)
            {
                if (detections.Where(x => x.StoelId == (i+1)).FirstOrDefault() != null)
                {
                    string response = await client.GetStringAsync("http://svmtesting.azurewebsites.net/api/values?jsontags=" +
                    detections.ElementAt(i).Tags);

                    //hier is de responce een nummer tussen de -1.0 en 1.0 maar dan in string vorm
                    Console.WriteLine(response);

                    //hier probeer ik die string naar double te converten
                    try { result = Double.Parse(response); }
                    catch { return NotFound(); }

                    //en hier komt die double er helemaal verkeert uit, de comma valt weg en het wordt een ander getal
                    Console.WriteLine(result);

                    if (result > 0) { Status.Stoelen.Add(new StoelInfo { Bezet = true }); }
                    else { Status.Stoelen.Add(new StoelInfo { Bezet = false }); }
                }
                else
                {
                    Status.Stoelen.Add(null);
                }


                ///////////////////////////////// ouwe taak, .. is nu half aangevult /////////////////////////////////////////

                // wat die hier doet is hij kijkt in de query die hierboven wordt uitgevoerd of er een record is voor de specifieke stoel, 
                // hij kijkt NIET naar de inhoud van record (de tags).
                // zolang de record er is zet hij hem op bezet.
                // hier moet dus de SVM tussen komen doormiddel van een api call ofzo. 
                // er moet dus nog in worden gebouwd dat er naar de inhoud word gekeken en aan de hand daarvan op bezet worden gezet.


                //string jsonstring = DescriptRepo.Detecties.FirstOrDefault().Tags;

                //return Ok("http://svmtesting.azurewebsites.net/api/values?jsontags=" + jsonstring);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                
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
