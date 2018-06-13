using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Flext.Models;


namespace svm.Controllers
{
    public class SVMMainController : Controller
    {
        IDescriptionRepository DescriptRepo;

        public SVMMainController(IDescriptionRepository idrepo)
        {
            DescriptRepo = idrepo;
        }
        
        [HttpGet]
        private IActionResult svmcall()
        {
            string jsonstring = DescriptRepo.Detecties.FirstOrDefault().Tags;

            return Ok("http://svmtesting.azurewebsites.net/api/values?jsontags=" + jsonstring);
        }

        public static string HttpGet(string URI)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }
    }
}
