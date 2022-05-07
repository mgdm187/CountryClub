using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Osoba
    {
        public int? IdOsoba { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string PunoIme => $"{Ime}, {Prezime}";
        public string Email { get; set; }
        public string? Lozinka { get; set; }
        public string Username { get; set; }
        public string? Oib { get; set; }
        public DateTime DatumRodenja { get; set; }
        public string? Telefon { get; set; }
        public DateTime DatumPrijave { get; set; }
        public string Adresa { get; set; }
        public int IdUloga { get; set; }
        public string nazivUloga { get; set; }
        public int IdMjesto { get; set; }
        public string NazivMjesto { get; set; }
        public string Pbr { get; set; }
        public Status Status { get; set; }
        public int? IdClanarina { get; set; }
        public string? ClanarinaNaziv { get; set; }
        public List<OsobaRacuni>? Racuni { get; set; }
        public List<ListaRezervacija>? Rezervacije { get; set; }
    }
}
