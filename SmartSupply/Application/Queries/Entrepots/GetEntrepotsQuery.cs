using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Entrepots
{
    public record GetEntrepotsQuery() : IRequest<List<Entrepot>>;
    
}
