using MediatR;

namespace SmartSupply.Application.Commands.Produits
{
    public record DeleteProduitCommand(int Id) : IRequest<bool>;
}
