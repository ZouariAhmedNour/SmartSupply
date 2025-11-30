using MediatR;

namespace SmartSupply.Application.Commands.EventMetiers
{
    public record CreateEventMetierCommand(
        string TypeEvent,
        string Donnees
    ) : IRequest<bool>;
}
