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
    public class UslugeRepository : IUslugeRepository
    {
        private readonly CountryclubContext ctx;

        public UslugeRepository(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<IList<DomainModel.Usluga>> GetUsluge()
        {
            var data = await ctx.Usluga
                                .Select(u => new DomainModel.Usluga
                                {
                                    IdUsluga = u.IdUsluga,
                                    NazivUsluga = u.NazivUsluga,
                                    Kapacitet = u.Kapacitet,
                                    CijenaUsluga = u.CijenaPoSatu
                                })
                                .ToListAsync();
            return data;
        }

        public async Task<int> SaveUsluga(DomainModel.Usluga usluga)
        {
            if (usluga.IdUsluga == null)
            {
                return await AddUsluga(usluga);
            }
            else
            {
                await UpdateUsluga(usluga);
                return usluga.IdUsluga.Value;
            }
        }

        private async Task<int> AddUsluga(DomainModel.Usluga usluga)
        {
            var entity = new EFModel.Usluga
            {
                NazivUsluga = usluga.NazivUsluga,
                CijenaPoSatu = usluga.CijenaUsluga,
                Kapacitet = usluga.Kapacitet
            };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();
            return entity.IdUsluga;
        }

        public async Task UpdateUsluga(DomainModel.Usluga usluga)
        {
            var query = ctx.Usluga.AsQueryable();
            var entity = await query.FirstOrDefaultAsync(p => p.IdUsluga == usluga.IdUsluga);
            if (entity != null)
            {
                entity.NazivUsluga = usluga.NazivUsluga;
                entity.CijenaPoSatu = usluga.CijenaUsluga;
                entity.Kapacitet = usluga.Kapacitet;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<DomainModel.Usluga> GetUslugaById(int idUsluga)
        {
            var data = await ctx.Usluga
                          .Where(r => r.IdUsluga == idUsluga)
                          .Select(r => new DomainModel.Usluga
                          {
                              IdUsluga = r.IdUsluga,
                              NazivUsluga = r.NazivUsluga,
                              CijenaUsluga = r.CijenaPoSatu,
                              Kapacitet = r.Kapacitet
                          })
                          .FirstOrDefaultAsync();

            return data;
        }
    }
}
