using DomainServices;
using Infrastructure.EFModel;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
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
        private readonly ISieveProcessor sieveProcessor;
        private readonly IClanarineRepository clanarineRepository;
        private readonly IOsobeRepository osobeRepository;

        public RacuniRepository(CountryclubContext ctx, ISieveProcessor sieveProcessor,
            IClanarineRepository clanarineRepository,
            IOsobeRepository osobeRepository)
        {
            this.ctx = ctx;
            this.sieveProcessor = sieveProcessor;
            this.clanarineRepository = clanarineRepository;
            this.osobeRepository = osobeRepository;
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

        public async Task<DomainModel.Racun> GetTekuciRacun(SieveModel criteria)
        {
            var query = ctx.Racun.AsNoTracking();
            if (criteria != null)
            {
                query = sieveProcessor.Apply(criteria, query);
            }
            var data = await query
                                .Select(u => new DomainModel.Racun
                                {
                                    IdRacun = u.IdRacun,
                                    Ime = u.IdOsobaNavigation.Ime,
                                    Prezime = u.IdOsobaNavigation.Prezime,
                                    IdClanarina = u.IdClanarina,
                                    NazivClanarina = u.IdClanarinaNavigation.NazivClanarina,
                                    CijenaClanarina = u.IdClanarinaNavigation.CijenaClanarina,
                                    CijenaUkupno = u.Ukupno,
                                    Placeno = u.RacunPlacen,
                                    IdOsoba = u.IdOsoba,
                                    DatumRacuna = u.DatumRacuna,
                                })
                                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<IList<DomainModel.Racun>> GetRacuni(SieveModel criteria)
        {
            var query = ctx.Racun.AsNoTracking();
            if (criteria != null)
            {
                query = sieveProcessor.Apply(criteria, query);
            }
            var data = await query
                                .Select(u => new DomainModel.Racun
                                {
                                    IdRacun = u.IdRacun,
                                    Ime = u.IdOsobaNavigation.Ime,
                                    Prezime = u.IdOsobaNavigation.Prezime,
                                    CijenaUkupno = u.Ukupno,
                                    Placeno = u.RacunPlacen,
                                    IdOsoba = u.IdOsoba,
                                    DatumRacuna = u.DatumRacuna,
                                })
                                .ToListAsync();

            return data;
        }

        public async Task<List<int>> GetClanoviSRacunima()
        {
            var clanoviBezRacuna = await ctx.Racun
                                        .Where(x => x.DatumRacuna.Month == DateTime.Now.Month)
                                        .Where(x => x.DatumRacuna.Year == DateTime.Now.Year)
                                        .Select(x => x.IdOsoba)
                                        .ToListAsync();

            return clanoviBezRacuna;
        }

        public async Task<int> SaveRacun(DomainModel.Racun racun)
        {
            if (racun.IdRacun == null)
            {
                return await AddRacun(racun);
            }
            else
            {
                await UpdateRacun(racun);
                return racun.IdRacun.Value;
            }
        }

        public async Task UpdateCijenaRacuna(int idRacun, decimal cijena)
        {
            var query = ctx.Racun.AsQueryable();
            var entity = await query.FirstOrDefaultAsync(p => p.IdRacun == idRacun);
            if (entity != null)
            {
                entity.Ukupno += cijena;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task UpdateRacun(DomainModel.Racun racun)
        {
            decimal cijena = 0.00M;
            foreach (var item in racun.Rezervacije)
            {
                racun.CijenaUkupno += item.Cijena;
            }
            var query = ctx.Racun.AsQueryable();
            var entity = await query.FirstOrDefaultAsync(p => p.IdRacun == racun.IdRacun);
            if (entity != null)
            {
                entity.RacunPlacen = racun.Placeno;
                entity.Ukupno = cijena;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task SaveAll(List<DomainModel.Racun> racuni)
        {
            List<Racun> racuniList = new List<Racun>();
            foreach(var r in racuni)
            {
                Racun racun = new Racun
                {
                    IdClanarina = (int)r.IdClanarina,
                    IdOsoba = r.IdOsoba,
                    DatumRacuna = r.DatumRacuna,
                    Ukupno = r.CijenaUkupno,
                    RacunPlacen = r.Placeno
                };
                racuniList.Add(racun);
            }
            ctx.AddRange(racuniList);
            await ctx.SaveChangesAsync();
        }

        public async Task<int> AddRacun(DomainModel.Racun racun)
        {
            decimal cijena = 0.00M;
            foreach(var item in racun.Rezervacije)
            {
                cijena += item.Cijena;
            }
            cijena += racun.CijenaClanarina.Value;
            var entity = new EFModel.Racun
            {
                DatumRacuna = DateTime.Now.Date,
                IdClanarina = racun.IdClanarina.Value,
                RacunPlacen = false,
                Ukupno = cijena,
                IdOsoba = racun.IdOsoba
            };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();

            return entity.IdRacun;
        }
    }
}
