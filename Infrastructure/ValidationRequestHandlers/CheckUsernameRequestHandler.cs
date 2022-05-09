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
    public class CheckUsernameRequestHandler : IRequestHandler<CheckUsername, bool>
    {
        private readonly CountryclubContext ctx;

        public CheckUsernameRequestHandler(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> Handle(CheckUsername request, CancellationToken cancellationToken)
        {
            bool alreadyExists = await ctx.Osoba
                                          .Where(p => p.Username == request.Username)
                                          .Where(p => p.IdOsoba != request.Osoba.IdOsoba)
                                          .AnyAsync();

            return !alreadyExists;
        }
    }
}
