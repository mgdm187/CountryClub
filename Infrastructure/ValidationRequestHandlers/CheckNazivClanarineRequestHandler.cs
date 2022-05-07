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
    public class CheckNazivClanarineRequestHandler : IRequestHandler<CheckNazivClanarine, bool>
    {
        private readonly CountryclubContext ctx;

        public CheckNazivClanarineRequestHandler(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> Handle(CheckNazivClanarine request, CancellationToken cancellationToken)
        {
            bool alreadyExists = await ctx.Clanarina
                                          .Where(p => p.NazivClanarina == request.NazivClanarina)
                                          .Where(p => p.IdClanarina != request.clanarina.IdClanarina)
                                          .AnyAsync();
            return !alreadyExists;
        }
    }
}
