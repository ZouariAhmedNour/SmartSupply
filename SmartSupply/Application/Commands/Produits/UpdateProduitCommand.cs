using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Commands.Produits
{
    public record UpdateProduitCommand(
        int Id,
        string Nom,
        string Description,
        string CodeSKU,
        decimal PrixUnitaire
    ) : IRequest<Produit?>;
}
