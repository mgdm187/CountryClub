using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Validation.Requests
{
    public record CheckPbr(Mjesto mjesto, string pbr) : IRequest<bool>
    {

    }
}
