using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.EventMetiers
{
    public record GetEventMetierByIdQuery(int Id) : IRequest<EventMetier?>;
}
