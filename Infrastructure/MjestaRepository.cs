using DomainServices;
using Infrastructure.EFModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MjestaRepository : IMjestaRepository
    {
        private readonly CountryclubContext ctx;

        public MjestaRepository(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<IList<DomainModel.Mjesto>> GetMjesta()
        {
            var data = await ctx.Mjesto
                                .Select(u => new DomainModel.Mjesto
                                {
                                    IdMjesto = u.IdMjesto,
                                    NazivMjesto = u.NazivMjesto,
                                    Pbr = u.Pbr
                                })
                                .ToListAsync();
            return data;
        }

        public async Task<int> SaveMjesto(DomainModel.Mjesto mjesto)
        {
            if (mjesto.IdMjesto == null)
            {
                return await AddMjesto(mjesto);
            }
            else
            {
                await UpdateMjesto(mjesto);
                return mjesto.IdMjesto.Value;
            }
        }

        private async Task<int> AddMjesto(DomainModel.Mjesto mjesto)
        {
            var entity = new EFModel.Mjesto
            {
                NazivMjesto = mjesto.NazivMjesto,
                Pbr = mjesto.Pbr
            };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();
            return entity.IdMjesto;
        }

        public async Task UpdateMjesto(DomainModel.Mjesto mjesto)
        {
            var query = ctx.Mjesto.AsQueryable();
            var entity = await query.FirstOrDefaultAsync(p => p.IdMjesto == mjesto.IdMjesto);
            if (entity != null)
            {
                entity.NazivMjesto = mjesto.NazivMjesto;
                entity.Pbr = mjesto.Pbr;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<DomainModel.Mjesto> GetMjestoById(int idMjesto)
        {
            var data = await ctx.Mjesto
                          .Where(r => r.IdMjesto == idMjesto)
                          .Select(r => new DomainModel.Mjesto
                          {
                              IdMjesto = r.IdMjesto,
                              Pbr = r.Pbr,
                              NazivMjesto = r.NazivMjesto
                          })
                          .FirstOrDefaultAsync();

            return data;
        }

        public async Task<DomainModel.Mjesto> GetMjestoByPbr(string pbr)
        {
            if (string.IsNullOrEmpty(pbr))
            {
                return null;
            }
            else
            {
                var data = await ctx.Mjesto
                          .Where(r => r.Pbr == pbr)
                          .Select(r => new DomainModel.Mjesto
                          {
                              IdMjesto = r.IdMjesto,
                              Pbr = r.Pbr,
                              NazivMjesto = r.NazivMjesto
                          })
                          .FirstOrDefaultAsync();

                return data;
            }
        }
    }
}
