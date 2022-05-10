using DomainModel;

namespace DomainServices
{
    public interface IClanarineRepository
    {
        Task<IList<Clanarina>> GetClanarine();
        Task<Clanarina> GetClanarinaById(int idClanarina);
        Task<int> SaveClanarina(Clanarina clanarina);
        Task<Clanarina> GetClanarinaByDatum(DateTime datum);
    }
}
