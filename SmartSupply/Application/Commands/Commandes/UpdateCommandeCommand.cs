using MediatR;

namespace SmartSupply.Application.Commands.Commandes
{
    public record UpdateCommandeCommand(
        int Id,
        string ClientNom,
        string ClientEmail,
        string Statut
    ) : IRequest<bool>; // bool indique succès ou échec
}
