using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Racun
    {
        public int? IdRacun { get; set; }
        public int IdOsoba { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRacuna { get; set; }
        public bool? Placeno { get; set; }
        public int? IdClanarina { get; set; }
        public string? NazivClanarina { get; set; }
        public decimal? CijenaClanarina { get; set; }
        public List<ListaRezervacija>? Rezervacije { get; set; }
        public decimal CijenaUkupno { get; set; }
    }
}
