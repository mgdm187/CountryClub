using DomainModel.Validation.Requests;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Validation
{
    public class ClanarinaValidator : AbstractValidator<Clanarina>
    {
        private readonly IMediator mediator;

        public ClanarinaValidator(IMediator mediator)
        {
            this.mediator = mediator;
            RuleFor(p => p.NazivClanarina)
              .NotEmpty()
              .MaximumLength(100)
              .DependentRules(() => RuleFor(p => p.NazivClanarina)
                                      .MustAsync(NazivMoraBitiJedinstven)
                                      .WithMessage("Naziv članarine mora biti jedinstven.")
                              );
            RuleFor(p => p.CijenaClanarina)
                .NotNull()
                .InclusiveBetween(100, 1000).WithMessage("Cijena mora biti između 100 i 1000 kuna.");
        }

        private async Task<bool> NazivMoraBitiJedinstven(Clanarina clanarina, string naziv, CancellationToken cancellationToken)
        {
            return await mediator.Send(new CheckNazivClanarine(clanarina, naziv));
        }
    }
}
