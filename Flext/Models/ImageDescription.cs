using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flext.Models
{
    //TODO : attribuut maken voor stoelID
    public class ImageDescription
    {
        public int ID { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeihgt { get; set; }
        public string RequestId { get; set; }
        public string Tags { get; set; }
        public DateTime Timestamp { get; set; }
        public string FileName { get; set; }
        public string Format { get; set; }
    }
}
