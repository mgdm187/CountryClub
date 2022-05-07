using System;
using DomainModel.Validation.Requests;
using Infrastructure.EFModel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ValidationRequestHandlers
{
    public class CheckEmailRequestHandler : IRequestHandler<CheckEmail, bool>
    {
        private readonly CountryclubContext ctx;

        public CheckEmailRequestHandler(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> Handle(CheckEmail request, CancellationToken cancellationToken)
        {
            bool alreadyExists = await ctx.Osoba
                                          .Where(p => p.Email == request.Email)
                                          .Where(p => p.IdOsoba != request.Osoba.IdOsoba)
                                          .AnyAsync();
            return !alreadyExists;
        }
    }
}
