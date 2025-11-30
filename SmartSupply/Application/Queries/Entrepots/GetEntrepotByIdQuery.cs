using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Entrepots
{
    public record GetEntrepotByIdQuery(int Id) : IRequest<Entrepot?>;
    
}
