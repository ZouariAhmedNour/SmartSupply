using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.EventMetiers
{
    public record GetEventMetiersQuery() : IRequest<List<EventMetier>>;
}
