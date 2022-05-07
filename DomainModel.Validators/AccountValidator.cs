using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Validation
{
    public class AccountValidator : AbstractValidator<AccountInfo>
    {
        private readonly IMediator mediator;

        public AccountValidator(IMediator mediator)
        {
            this.mediator = mediator;
            RuleFor(p => p.Username).NotEmpty().WithMessage("Korisničko ime je obavezno polje.");
            RuleFor(p => p.Lozinka).NotEmpty().WithMessage("Lozinka je obavezno polje.");
            
        }
    }
}
