using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class OsobaList
    {
        public int? IdOsoba { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public Status Status { get; set; }
        public string Email { get; set; }
    }
}
