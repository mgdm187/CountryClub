using MediatR;

namespace DomainModel.Validation.Requests
{
    public record CheckNazivUloge(int IdUloga, string NazivUloga) : IRequest<bool>
    {

    }
}
