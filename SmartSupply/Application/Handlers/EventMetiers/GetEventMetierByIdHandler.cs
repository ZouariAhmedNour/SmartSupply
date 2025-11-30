using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.EventMetiers;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.EventMetiers
{
    public class GetEventMetierByIdHandler : IRequestHandler<GetEventMetierByIdQuery, EventMetier?>
    {
        private readonly SmartSupplyDbContext _db;

        public GetEventMetierByIdHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<EventMetier?> Handle(GetEventMetierByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        }
    }
}
