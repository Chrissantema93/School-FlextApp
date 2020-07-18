using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Flext.Models
{
    public class ImageUploadForm
    {
        public IFormFile Image { get; set; }
        public int StoelId { get; set; }
    }
}
