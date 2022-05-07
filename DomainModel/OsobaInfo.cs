using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public record OsobaInfo(int IdOsoba, string Ime, string Prezime)
    {
        public string PunoImeOsobe => $"{Ime}, {Prezime}";
    }
}
