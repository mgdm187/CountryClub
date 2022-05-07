using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices
{
    public interface IRezervacijeRepository
    {
        Task<Rezervacija> GetRezervacija(int idRezervacija);
        Task<IList<Rezervacija>> GetRezervacije(int idOsoba);
        Task SaveRezervacija(Rezervacija rezervacija);
        Task DeleteRezervacija(int idRezervacija);
    }
}
