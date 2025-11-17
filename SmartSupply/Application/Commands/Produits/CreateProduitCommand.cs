using MediatR;
using SmartSupply.Application.Common;

namespace SmartSupply.Application.Commands.Produits
{
    public record CreateProduitCommand(string Nom, string Description, string CodeSKU, decimal PrixUnitaire) : IRequest<Result<int>>;

}
