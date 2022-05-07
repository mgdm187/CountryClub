using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Usluga
    {
        public int? IdUsluga { get; set; }
        public string NazivUsluga { get; set; }
        public int Kapacitet { get; set; }
        public decimal CijenaUsluga { get; set; }
    }
}
