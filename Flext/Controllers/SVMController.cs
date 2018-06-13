using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Flext.Models;
using Newtonsoft.Json;

namespace Flext.Controllers
{
    public class SVMController : Controller
    {
        static HttpClient client = new HttpClient();
        private IDescriptionRepository IDescriptionRepo;

        public SVMController(IDescriptionRepository IDescriptRepo)
        {
            IDescriptionRepo = IDescriptRepo;
        }

        public async Task<string> Call()
        {

            double result;

            IEnumerable<ImageDescription> detections = IDescriptionRepo.Detecties
                .GroupBy(x => x.StoelId)
                .Select(g => g.First())
                .Distinct();

            StoelInfo[] stoelenInfo = new StoelInfo[8];

            foreach (ImageDescription item in detections)
            {
                string response = await client.GetStringAsync("http://svmtesting.azurewebsites.net/api/values?jsontags=" +
                    item.Tags);
                Console.WriteLine(response);
                try { result = Double.Parse(response); }
                catch { return "An Error has occured. de responce kon niet worden omgezet tot int"; }
                Console.WriteLine(result);
                if (result > 0) { stoelenInfo[item.StoelId-1] = new StoelInfo { Bezet = true }; }
                else { stoelenInfo[item.StoelId] = new StoelInfo { Bezet = false }; }
            }

            string overzicht = JsonConvert.SerializeObject(stoelenInfo);

            return overzicht;

            //string response = await client.GetStringAsync("http://svmtesting.azurewebsites.net/api/values?jsontags="+tags);

            //return response.ToString();
        }

        
    }


}
