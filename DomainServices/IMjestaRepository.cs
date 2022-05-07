using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices
{
    public interface IMjestaRepository
    {
        Task<IList<Mjesto>> GetMjesta();
        Task<Mjesto> GetMjestoById(int idMjesto);
        Task<Mjesto> GetMjestoByPbr(string pbr);
        Task<int> SaveMjesto(Mjesto mjesto);
    }
}
