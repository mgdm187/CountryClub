using DomainModel;

namespace DomainServices
{
    public interface IUlogeRepository
    {
        Task<IList<Uloga>> GetUloge();
        Task<Uloga> GetUlogaById(int ulogaId);
        Task<int> CreateUloga(string ulogaNaziv);
        Task UpdateNazivUloge(int ulogaId, string naziv);
    }
}