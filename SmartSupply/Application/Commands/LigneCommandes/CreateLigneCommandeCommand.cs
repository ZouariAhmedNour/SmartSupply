using MediatR;

namespace SmartSupply.Application.Commands.LigneCommandes
{
    public record CreateLigneCommandeCommand(
        int CommandeId,
        int ProduitId,
        int Quantite,
        decimal PrixUnitaire
    ) : IRequest<bool>;
}
