using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Produits
{
    public record GetProduitByIdQuery(int Id) : IRequest<Produit?>;
}
