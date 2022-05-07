using DomainModel;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices
{
    public interface IOsobeRepository
    {
        Task<Osoba> GetOsobaById(int idOsoba);
        Task<Osoba> GetOsobaByUsername(string email);
        Task<IList<OsobaList>> GetOsobe();
        Task<int> SaveOsoba(Osoba osoba);
        Task UpdateStatus(int osobaId, int status);
        Task DeleteOsoba(int idOsoba);
    }
}
