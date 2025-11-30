using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.LigneCommandes
{
    public record GetLigneCommandesQuery() : IRequest<List<LigneCommande>>;
}
