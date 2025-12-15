using MediatR;
using SmartSupply.Domain.Models;


namespace SmartSupply.Application.Commands.Commandes
{
    public record CreateCommandeCommand(
        string ClientNom,
        string ClientEmail,
        decimal MontantTotal,
        List<LigneCommande> Lignes
    ) : IRequest<int?>; // Retour : Id de la commande ou null si échec
}