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
        Task<IList<ListaRezervacija>> GetRezervacije();
        Task<IList<ListaRezervacija>> GetRezervacije(int idOsoba);
        Task<int> SaveRezervacija(Rezervacija rezervacija);
        Task<bool> DeleteRezervacija(int idRezervacija);
    }
}
