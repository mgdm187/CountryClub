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
    public class RezervacijeRepository : IRezervacijeRepository
    {
        private readonly CountryclubContext ctx;
        private readonly IRacuniRepository racuniRepository;

        public RezervacijeRepository(CountryclubContext ctx, IRacuniRepository racuniRepository)
        {
            this.ctx = ctx;
            this.racuniRepository = racuniRepository;
        }

        public async Task<IList<DomainModel.ListaRezervacija>> GetRezervacije(int idOsoba)
        {
            var data = await ctx.Rezervacija
                                .Where(x => x.IdOsoba == idOsoba)
                                .Select(u => new DomainModel.ListaRezervacija
                                {
                                    IdRezervacija = u.IdRezervacija,
                                    Od = u.DatumPocetka,
                                    Do = u.DatumZavrsetka,
                                    Cijena = u.CijenaRezervacije,
                                    IdOsoba = idOsoba
                                })
                                .ToListAsync();

            return data;
        }

        public async Task<IList<DomainModel.ListaRezervacija>> GetRezervacije()
        {
            var data = await ctx.Rezervacija
                                .Select(u => new DomainModel.ListaRezervacija
                                {
                                    IdRezervacija = u.IdRezervacija,
                                    Od = u.DatumPocetka,
                                    Do = u.DatumZavrsetka,
                                    Cijena = u.CijenaRezervacije,
                                    IdOsoba = u.IdOsoba
                                })
                                .ToListAsync();

            return data;
        }

        public async Task<int> SaveRezervacija(DomainModel.Rezervacija rezervacija)
        {
            var entity = new EFModel.Rezervacija
            {
                DatumKreiranja = DateTime.Now,
                DatumPocetka = rezervacija.DatumPocetka,
                DatumZavrsetka = rezervacija.DatumZavrsetka,
                CijenaRezervacije = rezervacija.CijenaRezervacije,
                IdOsoba = rezervacija.OsobaId,
                IdRacun = rezervacija.IdRacun.Value
            };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();
            foreach(DomainModel.RezerviranaUsluga usluga in rezervacija.Usluge)
            {
                var usl = new EFModel.RezerviranaUsluga
                {
                    IdRezervacija = entity.IdRezervacija,
                    IdUsluga = usluga.IdUsluga,
                    PocetakUsluge = usluga.Od,
                    ZavrsetakUsluge = usluga.Do
                };
                ctx.Add(usl);
                await ctx.SaveChangesAsync();
            }

            return entity.IdRezervacija;
        }

        public async Task<DomainModel.Rezervacija> GetRezervacija(int idRezervacija)
        {
            var data = await ctx.Rezervacija
                          .Where(r => r.IdRezervacija == idRezervacija)
                          .Select(r => new DomainModel.Rezervacija
                          {
                              OsobaId = r.IdOsoba,
                              IdRacun = r.IdRacun,
                              IdRezervacija = r.IdRezervacija,
                              CijenaRezervacije = r.CijenaRezervacije,
                              DatumKreiranja = r.DatumKreiranja,
                              DatumPocetka = r.DatumPocetka,
                              DatumZavrsetka = r.DatumZavrsetka,
                              PunoIme = r.IdOsobaNavigation.Ime + " " + r.IdOsobaNavigation.Prezime
                          })
                          .FirstOrDefaultAsync();
            if (data != null)
            {
                var usluge = await ctx.RezerviranaUsluga
                                .Where(c => c.IdRezervacija == data.IdRezervacija)
                                .Select(c => new DomainModel.RezerviranaUsluga
                                {
                                    IdRezervacija = c.IdRezervacija,
                                    IdUsluga = c.IdUsluga,
                                    NazivUsluge = c.IdUslugaNavigation.NazivUsluga,
                                    Od = c.PocetakUsluge,
                                    Do = c.ZavrsetakUsluge
                                })
                                .ToListAsync();
                data.Usluge = usluge;
            }

            return data;
        }


        public async Task<bool> DeleteRezervacija(int idRezervacija)
        {
            EFModel.Rezervacija rezervacija = await ctx.Rezervacija.FindAsync(idRezervacija);
            if(rezervacija.DatumPocetka.Date <= DateTime.Now.Date.AddDays(3))
            {
                return false;
            }
            ctx.Remove(rezervacija);
            await ctx.SaveChangesAsync();
            await racuniRepository.UpdateCijenaRacuna(rezervacija.IdRacun.Value, (rezervacija.CijenaRezervacije * -1));
            return true;
        }
    }
}
