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
    public class CheckNazivUslugeRequestHandler : IRequestHandler<CheckNazivUsluge, bool>
    {
        private readonly CountryclubContext ctx;

        public CheckNazivUslugeRequestHandler(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> Handle(CheckNazivUsluge request, CancellationToken cancellationToken)
        {
            bool alreadyExists = await ctx.Usluga
                                          .Where(p => p.NazivUsluga == request.NazivUsluga)
                                          .Where(p => p.IdUsluga != request.usluga.IdUsluga)
                                          .AnyAsync();
            return !alreadyExists;
        }
    }
}
