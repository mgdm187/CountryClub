

using DomainModel;
using DomainServices;
using Infrastructure.EFModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ClanarineRepository : IClanarineRepository
    {
        private readonly CountryclubContext ctx;

        public ClanarineRepository(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<DomainModel.Clanarina> GetClanarinaByDatum(DateTime datum)
        {
            int age = 0;
            age = DateTime.Now.Subtract(datum).Days;
            age = age / 365;
            var data = await ctx.Clanarina
                            .Where(x => x.OdGodine <= age)
                            .Where(x => x.DoGodine > age)
                            .Select(c => new DomainModel.Clanarina
                            {
                                IdClanarina = c.IdClanarina,
                                NazivClanarina = c.NazivClanarina,
                                CijenaClanarina = c.CijenaClanarina,
                                OdGodine = c.OdGodine,
                                DoGodine = c.DoGodine
                            })
                            .FirstOrDefaultAsync();
            return data;
        }

        public async Task<IList<DomainModel.Clanarina>> GetClanarine()
        {
            var data = await ctx.Clanarina
                                .Select(u => new DomainModel.Clanarina
                                {
                                    IdClanarina = u.IdClanarina,
                                    NazivClanarina = u.NazivClanarina,
                                    CijenaClanarina = u.CijenaClanarina,
                                    OdGodine = u.OdGodine,
                                    DoGodine = u.DoGodine
                                })
                                .ToListAsync();
            return data;
        }

        public async Task<int> SaveClanarina(DomainModel.Clanarina clanarina)
        {
            if (clanarina.IdClanarina == null)
            {
                return await AddClanarina(clanarina);
            }
            else
            {
                await UpdateClanarina(clanarina);
                return clanarina.IdClanarina.Value;
            }
        }

        private async Task<int> AddClanarina(DomainModel.Clanarina clanarina)
        {
            var entity = new EFModel.Clanarina
            {
                NazivClanarina = clanarina.NazivClanarina,
                CijenaClanarina = clanarina.CijenaClanarina,
                OdGodine = clanarina.OdGodine,
                DoGodine = clanarina.DoGodine
            };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();
            return entity.IdClanarina;
        }

        public async Task UpdateClanarina(DomainModel.Clanarina clanarina)
        {
            var query = ctx.Clanarina.AsQueryable();
            var entity = await query.FirstOrDefaultAsync(p => p.IdClanarina == clanarina.IdClanarina);
            if (entity != null)
            {
                entity.NazivClanarina = clanarina.NazivClanarina;
                entity.CijenaClanarina = clanarina.CijenaClanarina;
                entity.OdGodine = clanarina.OdGodine;
                entity.DoGodine = clanarina.DoGodine;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<DomainModel.Clanarina> GetClanarinaById(int idClanarina)
        {
            var data = await ctx.Clanarina
                          .Where(r => r.IdClanarina == idClanarina)
                          .Select(r => new DomainModel.Clanarina
                          {
                              IdClanarina = r.IdClanarina,
                              NazivClanarina = r.NazivClanarina,
                              CijenaClanarina = r.CijenaClanarina,
                              OdGodine = r.OdGodine,
                              DoGodine = r.DoGodine
                          })
                          .FirstOrDefaultAsync();

            return data;
        }

        
    }
}
