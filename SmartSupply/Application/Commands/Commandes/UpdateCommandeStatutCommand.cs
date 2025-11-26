using MediatR;

namespace SmartSupply.Application.Commands.Commandes
{
    public record UpdateCommandeStatutCommand(
        int CommandeId,
        string Statut
    ) : IRequest<bool>;
}
