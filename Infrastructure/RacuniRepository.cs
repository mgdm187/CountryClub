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
    public class RacuniRepository : IRacuniRepository
    {
        private readonly CountryclubContext ctx;

        public RacuniRepository(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<DomainModel.Racun> GetRacun(int idRacun)
        {
            var data = await ctx.Racun
                                .Where(x => x.IdRacun == idRacun)
                                .Select(u => new DomainModel.Racun
                                {
                                    IdRacun = u.IdRacun,
                                    IdClanarina = u.IdClanarina,
                                    Placeno = u.RacunPlacen,
                                    CijenaUkupno = u.Ukupno,
                                    IdOsoba = u.IdOsoba,
                                    Ime = u.IdOsobaNavigation.Ime,
                                    Prezime = u.IdOsobaNavigation.Prezime,
                                    NazivClanarina = u.IdClanarinaNavigation.NazivClanarina,
                                    CijenaClanarina = u.IdClanarinaNavigation.CijenaClanarina
                                })
                                .FirstOrDefaultAsync();
            if(data != null)
            {
                var rezervacije = await ctx.Rezervacija
                                    .Where(x => x.IdRacun == data.IdRacun)
                                    .Select(u => new DomainModel.ListaRezervacija
                                    {
                                        IdRezervacija = u.IdRezervacija,
                                        IdOsoba = u.IdOsoba,
                                        Cijena = u.CijenaRezervacije,
                                        Od = u.DatumPocetka,
                                        Do = u.DatumZavrsetka
                                    })
                                    .ToListAsync();
                data.Rezervacije = rezervacije;
            }

            return data;
        }

        public async Task<IList<DomainModel.Racun>> GetRacuni(int idOsoba)
        {
            var data = await ctx.Racun
                                .Where(x => x.IdOsoba == idOsoba)
                                .Select(u => new DomainModel.Racun
                                {
                                    IdRacun = u.IdRacun,
                                    Ime = u.IdOsobaNavigation.Ime,
                                    Prezime = u.IdOsobaNavigation.Prezime,
                                    CijenaUkupno = u.Ukupno,
                                    IdOsoba = u.IdOsoba,
                                    DatumRacuna = u.DatumRacuna,
                                })
                                .ToListAsync();

            return data;
        }

        public async Task<IList<DomainModel.Racun>> GetRacuni()
        {
            var data = await ctx.Racun
                                .Select(u => new DomainModel.Racun
                                {
                                    IdRacun = u.IdRacun,
                                    Ime = u.IdOsobaNavigation.Ime,
                                    Prezime = u.IdOsobaNavigation.Prezime,
                                    CijenaUkupno = u.Ukupno,
                                    IdOsoba = u.IdOsoba,
                                    DatumRacuna = u.DatumRacuna,
                                })
                                .ToListAsync();

            return data;
        }

        public async Task<int> SaveRacun(DomainModel.Racun racun)
        {
            var entity = new EFModel.Racun
            {
                DatumRacuna = DateTime.Now.Date,
                IdClanarina = racun.IdClanarina.Value,
                RacunPlacen = null,
                Ukupno = racun.CijenaUkupno,
                IdOsoba = racun.IdOsoba
            };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();

            return entity.IdRacun;
        }
    }
}
