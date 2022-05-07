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
    public class MjestoValidator : AbstractValidator<Mjesto>
    {
        private readonly IMediator mediator;

        public MjestoValidator(IMediator mediator)
        {
            this.mediator = mediator;
            RuleFor(p => p.Pbr)
              .NotEmpty()
              .MaximumLength(5)
              .DependentRules(() => RuleFor(p => p.Pbr)
                                      .MustAsync(PostanskiBrojJedinstven)
                                      .WithMessage("Poštanski broj mora biti jedinstven.")
                              );
        }

        private async Task<bool> PostanskiBrojJedinstven(Mjesto mjesto, string pbr, CancellationToken cancellationToken)
        {
            return await mediator.Send(new CheckPbr(mjesto, pbr), cancellationToken);
        }
    }
}
