using DomainServices;
using Infrastructure.EFModel;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Infrastructure
{
    public class OsobeRepository : IOsobeRepository
    {
        private readonly CountryclubContext ctx;

        public OsobeRepository(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<IList<DomainModel.OsobaList>> GetOsobe()
        {
            var data = await ctx.Osoba
                                .Select(u => new DomainModel.OsobaList
                                {
                                    IdOsoba = u.IdOsoba,
                                    Ime = u.Ime,
                                    Prezime = u.Prezime,
                                    Status = (DomainModel.Status)u.Status,
                                    Email = u.Email
                                })
                                .ToListAsync();
            return data;
        }

        public async Task<int> SaveOsoba(DomainModel.Osoba osoba)
        {
            if (osoba.IdOsoba == null)
            {
                return await AddOsoba(osoba);
            }
            else
            {
                await UpdateOsoba(osoba);
                return osoba.IdOsoba.Value;
            }
        }

        private async Task<int> AddOsoba(DomainModel.Osoba osoba)
        {
            var entity = new EFModel.Osoba
            {
                Ime = osoba.Ime,
                Prezime = osoba.Prezime,
                DatumRodenja = osoba.DatumRodenja,
                DatumPrijave = osoba.DatumPrijave,
                Email = osoba.Email,
                Username = osoba.Username,
                Lozinka = osoba.Lozinka,
                Oib = osoba.Oib,
                Telefon = osoba.Telefon,
                Status = (int)osoba.Status,
                IdUloga = osoba.IdUloga,
                IdMjesto = osoba.IdMjesto,
                Adresa = osoba.Adresa
            };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();
            return entity.IdOsoba;
        }

        public async Task UpdateOsoba(DomainModel.Osoba osoba)
        {
            var query = ctx.Osoba.AsQueryable();
            var entity = await query.FirstOrDefaultAsync(p => p.IdOsoba == osoba.IdOsoba);
            if (entity != null)
            {
                entity.Ime = osoba.Ime;
                entity.Prezime = osoba.Prezime;
                entity.Adresa = osoba.Adresa;
                entity.Email = osoba.Email;
                entity.Telefon = osoba.Telefon;
                entity.DatumRodenja = osoba.DatumRodenja;
                entity.IdMjesto = osoba.IdMjesto;
                entity.Oib = osoba.Oib;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task UpdateStatus(int osobaId, int status)
        {
            var query = ctx.Osoba.AsQueryable();
            var entity = await query.FirstOrDefaultAsync(p => p.IdOsoba == osobaId);
            if (entity != null)
            {
                entity.Status = status;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<DomainModel.Osoba> GetOsobaById(int idOsoba)
        {
            var data = await ctx.Osoba
                          .Where(r => r.IdOsoba == idOsoba)
                          .Select(r => new DomainModel.Osoba
                          {
                              IdOsoba = r.IdOsoba,
                              Ime = r.Ime,
                              Prezime = r.Prezime,
                              Adresa = r.Adresa,
                              Email = r.Email,
                              Telefon = r.Telefon,
                              DatumRodenja = r.DatumRodenja,
                              DatumPrijave = r.DatumPrijave,
                              Status = (DomainModel.Status)r.Status,
                              IdUloga = r.IdUloga,
                              IdMjesto = r.IdMjesto,
                              Username = r.Username,
                              Oib = r.Oib
                          })
                          .FirstOrDefaultAsync();
            if(data != null)
            {
                var uloga = await ctx.Uloga
                            .Where(u => u.IdUloga == data.IdUloga)
                            .Select(r => new DomainModel.Uloga
                            {
                                IdUloga = r.IdUloga,
                                NazivUloga = r.NazivUloga
                            })
                            .FirstOrDefaultAsync();
                data.nazivUloga = uloga.NazivUloga;
                var mjesto = await ctx.Mjesto
                                .Where(c => c.IdMjesto <= data.IdMjesto)
                                .Select(c => new DomainModel.Mjesto
                                {
                                    IdMjesto = c.IdMjesto,
                                    NazivMjesto = c.NazivMjesto,
                                    Pbr = c.Pbr
                                })
                                .FirstOrDefaultAsync();
                data.NazivMjesto = mjesto.NazivMjesto;
                data.Pbr = mjesto.Pbr;
                var rezervacije = await ctx.Rezervacija
                            .Where(u => u.IdOsoba == data.IdOsoba)
                            .Select(r => new DomainModel.ListaRezervacija
                            {
                                IdRezervacija = r.IdRezervacija,
                                Od = r.DatumPocetka,
                                Do = r.DatumZavrsetka,
                                Cijena = r.CijenaRezervacije,
                                IdOsoba = r.IdOsoba
                            })
                            .ToListAsync();
                data.Rezervacije = rezervacije;
            }

            return data;
        }

        public async Task<DomainModel.Osoba> GetOsobaByUsername(string username)
        {
            var data = await ctx.Osoba
                          .Where(r => r.Username == username)
                          .Select(r => new DomainModel.Osoba
                          {
                              IdOsoba = r.IdOsoba,
                              Ime = r.Ime,
                              Prezime = r.Prezime,
                              Adresa = r.Adresa,
                              Email = r.Email,
                              Telefon = r.Telefon,
                              DatumRodenja = r.DatumRodenja,
                              DatumPrijave = r.DatumPrijave,
                              Status = (DomainModel.Status)r.Status,
                              IdUloga = r.IdUloga,
                              IdMjesto = r.IdMjesto,
                              Username = r.Username,
                              Oib = r.Oib,
                              Lozinka = r.Lozinka
                          })
                          .FirstOrDefaultAsync();
            if (data != null)
            {
                var uloga = await ctx.Uloga
                            .Where(u => u.IdUloga == data.IdUloga)
                            .Select(r => new DomainModel.Uloga
                            {
                                IdUloga = r.IdUloga,
                                NazivUloga = r.NazivUloga
                            })
                            .FirstOrDefaultAsync();
                data.nazivUloga = uloga.NazivUloga;
                var mjesto = await ctx.Mjesto
                                .Where(c => c.IdMjesto <= data.IdMjesto)
                                .Select(c => new DomainModel.Mjesto
                                {
                                    IdMjesto = c.IdMjesto,
                                    NazivMjesto = c.NazivMjesto,
                                    Pbr = c.Pbr
                                })
                                .FirstOrDefaultAsync();
                data.NazivMjesto = mjesto.NazivMjesto;
                data.Pbr = mjesto.Pbr;
                var rezervacije = await ctx.Rezervacija
                            .Where(u => u.IdOsoba == data.IdOsoba)
                            .Select(r => new DomainModel.ListaRezervacija
                            {
                                IdRezervacija = r.IdRezervacija,
                                Od = r.DatumPocetka,
                                Do = r.DatumZavrsetka,
                                Cijena = r.CijenaRezervacije,
                                IdOsoba = r.IdOsoba
                            })
                            .ToListAsync();
                data.Rezervacije = rezervacije;
            }

            return data;
        }

        public async Task DeleteOsoba(int idOsoba)
        {
            DomainModel.Osoba osoba = await GetOsobaById(idOsoba);
            await UpdateStatus(idOsoba, 2);
        }
    }
}
