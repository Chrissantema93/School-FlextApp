using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flext.Models
{
    public class TafelStatus
    {
        public string Tafelnaam { get; set; }
        public List<StoelInfo> Stoelen { get; set; }
    }

    public class StoelInfo
    {
        public bool Bezet { get; set; }
        //public Account User { get; set; } voor als we nog accounts gaan toevoegen.
    }
}
