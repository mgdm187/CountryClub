using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class RezerviranaUsluga
    {
        public int? IdRezervacija { get; set; }
        public int IdUsluga { get; set; }
        public string? NazivUsluge { get; set; }
        public DateTime Od { get; set; }
        public DateTime Do { get; set; }
        public int ProvedenoVrijeme { get; set; }
        public decimal Cijena { get; set; }
    }
}
