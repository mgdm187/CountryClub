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
    public class OsobaValidator : AbstractValidator<Osoba>
    {
        private readonly IMediator mediator;

        public OsobaValidator(IMediator mediator)
        {
            this.mediator = mediator;
            RuleFor(p => p.Ime).NotEmpty().MaximumLength(30);
            RuleFor(p => p.Prezime).NotEmpty().MaximumLength(50);
            RuleFor(p => p.Email).NotEmpty().MaximumLength(100)
               .DependentRules(() => RuleFor(p => p.Email)
                                      .MustAsync(EmailJedinstven)
                                      .WithMessage("Email mora biti jedinstven.")
                              );
            RuleFor(p => p.Username).NotEmpty().MaximumLength(30)
              .DependentRules(() => RuleFor(p => p.Username)
                                      .MustAsync(UsernameJedinstven)
                                      .WithMessage("Username mora biti jedinstven.")
                              );
            RuleFor(p => p.Oib).MaximumLength(11);
            RuleFor(p => p.DatumRodenja).NotEmpty()
                .DependentRules(() => RuleFor(p => p.DatumRodenja.Year)
                                        .InclusiveBetween(1910, DateTime.Now.Year)
                                        .WithMessage("Datum rođenja neispravan."));


        }

        private async Task<bool> UsernameJedinstven(Osoba osoba, string name, CancellationToken cancellationToken)
        {
            return await mediator.Send(new CheckUsername(osoba, name), cancellationToken);
        }

        private async Task<bool> EmailJedinstven(Osoba osoba, string name, CancellationToken cancellationToken)
        {
            return await mediator.Send(new CheckEmail(osoba, name), cancellationToken);
        }
    }
}
