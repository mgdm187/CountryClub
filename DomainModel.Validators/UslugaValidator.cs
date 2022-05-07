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
    public class UslugaValidator : AbstractValidator<Usluga>
    {
        private readonly IMediator mediator;

        public UslugaValidator(IMediator mediator)
        {
            this.mediator = mediator;
            RuleFor(p => p.NazivUsluga)
              .NotEmpty()
              .MaximumLength(100)
              .DependentRules(() => RuleFor(p => p.NazivUsluga)
                                      .MustAsync(NazivMoraBitiJedinstven)
                                      .WithMessage("Naziv usluge mora biti jedinstven.")
                              );
        }

        private async Task<bool> NazivMoraBitiJedinstven(Usluga Usluga, string naziv, CancellationToken cancellationToken)
        {
            return await mediator.Send(new CheckNazivUsluge(Usluga, naziv));
        }
    }
}
