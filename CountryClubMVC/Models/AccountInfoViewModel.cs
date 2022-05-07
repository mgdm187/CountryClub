using System.ComponentModel.DataAnnotations;

namespace CountryClubMVC.Models
{
    public class AccountInfoViewModel
    {
        public int? IdOsoba { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; }
        [Required]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; }
    }
}
