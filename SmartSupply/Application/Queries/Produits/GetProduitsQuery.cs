using MediatR;
using SmartSupply.Application.Common;
using SmartSupply.Application.DTOs;

namespace SmartSupply.Application.Queries.Produits
{
    public record GetProduitsQuery() : IRequest<Result<List<ProduitDto>>>;
}
