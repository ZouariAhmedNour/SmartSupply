using MediatR;
using SmartSupply.Application.Common;
using SmartSupply.Domain.Models;


namespace SmartSupply.Application.Queries.Commandes
{
    public record GetCommandeByIdQuery(int Id) : IRequest<Result<Commande>>
    {
    }
}
