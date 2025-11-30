using MediatR;

namespace SmartSupply.Application.Commands.Entrepots
{
    public record CreateEntrepotCommand(
        string Nom,
        string Adresse,
        int CapaciteMax
    ) : IRequest<bool>;
}
