using DomainModel.Validation.Requests;
using Infrastructure.EFModel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ValidationRequestHandlers
{
    public class CheckNazivUlogeRequestHandler : IRequestHandler<CheckNazivUloge, bool>
    {
        private readonly CountryclubContext ctx;

        public CheckNazivUlogeRequestHandler(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> Handle(CheckNazivUloge request, CancellationToken cancellationToken)
        {
            bool alreadyExists = await ctx.Uloga
                                          .Where(p => p.NazivUloga == request.NazivUloga)
                                          .Where(p => p.IdUloga != request.IdUloga)
                                          .AnyAsync();
            return !alreadyExists;
        }
    }
}
