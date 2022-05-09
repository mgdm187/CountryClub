using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class ListaRezervacija
    {
        public int IdRezervacija { get; set; }
        public DateTime Od { get; set; }
        public DateTime Do { get; set; }
        public decimal Cijena { get; set; }
        public int IdOsoba { get; set; }
    }
}
