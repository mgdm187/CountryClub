using System.ComponentModel.DataAnnotations;

namespace CountryClubMVC.Models
{
    public class KorisnickiRacunViewModel
    {
        public int? IdOsoba { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "Ime")]
        public string Ime { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Prezime")]
        public string Prezime { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum rođenja")]
        public DateTime DatumRodenja { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Adresa")]
        public string Adresa { get; set; }
        [Required]
        [MaxLength(5)]
        [Display(Name = "Poštanski broj")]
        public string Pbr { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Mjesto")]
        public string Mjesto { get; set; }
        [MaxLength(20)]
        [Display(Name = "Telefon")]
        public string? Telefon { get; set; }
        [MaxLength(11)]
        [Display(Name = "Oib")]
        public string? Oib { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "Ponovi lozinku")]
        public string PonoviLozinku { get; set; }
    }
}
