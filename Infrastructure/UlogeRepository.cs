using DomainServices;
using Infrastructure.EFModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class UlogeRepository : IUlogeRepository
    {
        private readonly CountryclubContext ctx;

        public UlogeRepository(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<int> CreateUloga(string ulogaNaziv)
        {
            var entity = new EFModel.Uloga
            {
                NazivUloga = ulogaNaziv
            };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();
            return entity.IdUloga;
        }

        public async Task<DomainModel.Uloga> GetUlogaById(int ulogaId)
        {
            var data = await ctx.Uloga
                          .Where(r => r.IdUloga == ulogaId)
                          .Select(r => new DomainModel.Uloga
                          {
                              IdUloga = r.IdUloga,
                              NazivUloga = r.NazivUloga
                          })
                          .FirstOrDefaultAsync();
            
            return data;
        }

        public async Task<IList<DomainModel.Uloga>> GetUloge()
        {
            var data = await ctx.Uloga
                                .Select(u => new DomainModel.Uloga
                                {
                                    IdUloga = u.IdUloga,
                                    NazivUloga = u.NazivUloga
                                })
                                .ToListAsync();
            return data;
        }

        public async Task UpdateNazivUloge(int ulogaId, string naziv)
        {
            var entity = await ctx.Uloga.FindAsync(ulogaId);
            if (entity != null)
            {
                entity.NazivUloga = naziv;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
