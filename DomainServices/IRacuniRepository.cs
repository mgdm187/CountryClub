using DomainModel;
using Sieve.Models;
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
        Task<IList<Racun>> GetRacuni(SieveModel criteria = null);
        Task<Racun> GetTekuciRacun(SieveModel criteria);
        Task<List<int>> GetClanoviSRacunima();
        Task UpdateCijenaRacuna(int idRacun, decimal cijena);
        Task<int> SaveRacun(Racun racun);
        Task SaveAll(List<DomainModel.Racun> racuni);

    }
}
