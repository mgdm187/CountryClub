using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices
{
    public interface IRacuniRepository
    {
        Task<Racun> GetRacun(int idRacun);
        Task<IList<Racun>> GetRacuni(int idOsoba);
        Task<IList<Racun>> GetRacuni();
        Task SaveRacun(Racun racun);

    }
}
