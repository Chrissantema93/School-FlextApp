using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Flext.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace Flext.Controllers
{
    public class ImageController : Controller
    {

        const string subscriptionKey = "fe233b1c64cc4844a5c6b16bb5dac3bd";
        const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/analyze";
        private IDescriptionRepository IDescriptionRepo;

        public ImageController(IDescriptionRepository IDescriptRepo)
        {
            this.IDescriptionRepo = IDescriptRepo;
        }
        
        

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> aquireFiles(ImageUploadForm form) //TODO: add parameter voor stoelID
        {
            //hoeveelheid bytes de requested images waren
            long size = form.Image.Length;

            // full path to file in temp location
            // dit slaat een .temp bestand op in je temp file directory, af en toe leeg maken anders staat je pc zo vol
            string filePath = Path.GetTempFileName();
            
            if (size > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await form.Image.CopyToAsync(stream);
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            ProcessJson(await MakeAnalysisRequest(filePath), form.Image.FileName); //TODO paramenter voor stoelID

            return RedirectToAction("Overzicht","Home");

        }
        

        private async Task<string> MakeAnalysisRequest(string imageFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();
                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                // Request parameters. A third optional parameter is "details".
                string requestParameters = "visualFeatures=Categories,Description";
                // Assemble the URI for the REST API Call.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;
                // Request body. Posts a locally stored JPEG image.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }
                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                // Display the JSON response.
                return contentString;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return null;
            }
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        private void ProcessJson(string Json, string filename)
        {
            JObject obj = JObject.Parse(Json);

            //TODO stoelID meegeven in model

            IDescriptionRepo.SaveToDB(
                new ImageDescription
                {
                    ImageWidth = Convert.ToInt16(obj["metadata"]["width"]),
                    ImageHeihgt = Convert.ToInt16(obj["metadata"]["height"]),
                    RequestId = obj["requestId"].ToString(),
                    Tags = JsonConvert.SerializeObject(obj["description"]["tags"].ToList()),
                    Timestamp = DateTime.Now,
                    FileName = filename,
                    Format = obj["metadata"]["format"].ToString()
                }
            );
        }
    }

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}
