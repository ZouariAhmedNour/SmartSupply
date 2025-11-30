using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Commands.LigneCommandeCommand
{
    public record UpdateLigneCommandeCommand(
        int Id,
        int CommandeId,
        int ProduitId,
        int Quantite,
        decimal PrixUnitaire
    ) : IRequest<LigneCommande?>;
}
