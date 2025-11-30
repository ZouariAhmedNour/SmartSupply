using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.LigneCommandes
{
    public record GetLigneCommandeByIdQuery(int Id) : IRequest<LigneCommande?>;
}
