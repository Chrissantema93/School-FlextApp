﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Flext.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Flext.Controllers
{
    [Route("api/[controller]")]
    public class ImageAPIController : Controller
    {
        private IDescriptionRepository IDescriptionRepo;
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // POST api/<controller>
        //[HttpPost]
        //public async Task<string> Post(ImageUploadForm files)
        //{
        //    var ctrl = new ImageController(IDescriptionRepo);
        //    return await ctrl.aquireFiles(files);
        //}
    }
}