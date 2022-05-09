using System.ComponentModel.DataAnnotations;

namespace CountryClubMVC.Models
{
    public class RezervacijeViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = true)]
        public DateOnly Datum { get; set; }
        public List<Usluge>? Usluge { get; set; }
    }

    public class Usluge
    {
        public int Usluga { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan Pocetak { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan Zavrsetak { get; set; }
        
    }

    public class RezViewModel
    {
        public string Datum { get; set; }
        public List<UslugeString> Usluge { get; set; }
    }

    public class UslugeString
    {
        public string Usluga { get; set; }
        public string Pocetak { get; set; }
        public string Zavrsetak { get; set; }
    }
}
