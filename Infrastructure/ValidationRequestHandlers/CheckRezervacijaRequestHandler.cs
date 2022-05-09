using DomainModel.Validation.Requests;
using Infrastructure.EFModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ValidationRequestHandlers
{
    public class CheckRezervacijaRequestHandler : IRequestHandler<CheckRezervacija, bool>
    {
        private readonly CountryclubContext ctx;

        public CheckRezervacijaRequestHandler(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> Handle(CheckRezervacija request, CancellationToken cancellationToken)
        {
            bool alreadyExists = await ctx.Rezervacija
                                          .Where(p => p.IdOsoba == request.Rezervacija.OsobaId)
                                          .Where(p => p.DatumPocetka <= request.Rezervacija.DatumPocetka)
                                          .Where(p => p.DatumZavrsetka >= request.Rezervacija.DatumPocetka)
                                          .AnyAsync();

            if (!alreadyExists)
            {
                alreadyExists = await ctx.Rezervacija
                                          .Where(p => p.IdOsoba == request.Rezervacija.OsobaId)
                                          .Where(p => p.DatumPocetka <= request.Rezervacija.DatumZavrsetka)
                                          .Where(p => p.DatumZavrsetka >= request.Rezervacija.DatumZavrsetka)
                                          .AnyAsync();
            }

            return !alreadyExists;
        }
    }
}
