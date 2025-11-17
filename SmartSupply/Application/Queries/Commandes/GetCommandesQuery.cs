using MediatR;
using SmartSupply.Application.Common;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Commandes
{
    public record GetCommandesQuery() : IRequest<Result<List<Commande>>>
    {
    }
}
