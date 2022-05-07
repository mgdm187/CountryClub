using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Rezervacija
    {
        public int IdRezervacija { get; set; }
        public int? IdRacun { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public decimal CijenaRezervacije { get; set; }
        public int OsobaId { get; set; }
        public string PunoIme { get; set; }
        public List<RezerviranaUsluga> Usluge { get; set; } = new();
    }
}
