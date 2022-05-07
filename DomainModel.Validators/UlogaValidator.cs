using DomainModel.Validation.Requests;
using FluentValidation;
using MediatR;

namespace DomainModel.Validation
{
    public class UlogaValidator : AbstractValidator<Uloga>
    {
        private readonly IMediator mediator;

        public UlogaValidator(IMediator mediator)
        {
            this.mediator = mediator;
            RuleFor(p => p.NazivUloga)
              .NotEmpty()
              .MaximumLength(20)
              .DependentRules(() => RuleFor(p => p.NazivUloga)
                                      .MustAsync(NazivMoraBitiJedinstven)
                                      .WithMessage("Naziv uloge mora biti jedinstven.")
                              );
        }

        private async Task<bool> NazivMoraBitiJedinstven(Uloga Uloga, string naziv, CancellationToken cancellationToken)
        {
            return await mediator.Send(new CheckNazivUloge(Uloga.IdUloga, naziv));
        }
    }
}