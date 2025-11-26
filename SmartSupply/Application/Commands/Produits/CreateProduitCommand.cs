using MediatR;

namespace SmartSupply.Application.Commands.Produits
{
    public record CreateProduitCommand(
        string Nom,
        string Description,
        string CodeSKU,
        decimal PrixUnitaire
    ) : IRequest<int>;
}
