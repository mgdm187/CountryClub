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
    public class CheckPbrRequestHandler : IRequestHandler<CheckPbr, bool>
    {
        private readonly CountryclubContext ctx;

        public CheckPbrRequestHandler(CountryclubContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> Handle(CheckPbr request, CancellationToken cancellationToken)
        {
            bool alreadyExists = await ctx.Mjesto
                                          .Where(p => p.Pbr == request.pbr)
                                          .Where(p => p.IdMjesto != request.mjesto.IdMjesto)
                                          .AnyAsync();
            return !alreadyExists;
        }
    }
}
