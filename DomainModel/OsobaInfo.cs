using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class OsobaInfo
    {
        public int IdOsoba { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public int IdUloga { get; set; }
        public DateTime DatumRodenja { get; set; }
    }
}
