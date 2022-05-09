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
    public class RezervacijaValidator : AbstractValidator<Rezervacija>
    {
        private readonly IMediator mediator;

        public RezervacijaValidator(IMediator mediator)
        {
            this.mediator = mediator;
            RuleFor(p => p.DatumPocetka).LessThan(p => p.DatumZavrsetka);

            RuleFor(p => p.DatumPocetka).NotEmpty()
                .DependentRules(() => RuleFor(p => p.DatumPocetka)
                            .MustAsync(JedinstvenaRezervacija)
                            .WithMessage("Rezervacija nije jedinstvena."));


        }

        private async Task<bool> JedinstvenaRezervacija(Rezervacija rezervacija, DateTime datum, CancellationToken cancellationToken)
        {
            return await mediator.Send(new CheckRezervacija(rezervacija, datum));
        }
    }
}
