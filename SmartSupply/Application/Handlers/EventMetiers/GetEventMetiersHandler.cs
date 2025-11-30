using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.EventMetiers;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.EventMetiers
{
    public class GetEventMetiersHandler : IRequestHandler<GetEventMetiersQuery, List<EventMetier>>
    {
        private readonly SmartSupplyDbContext _db;

        public GetEventMetiersHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<List<EventMetier>> Handle(GetEventMetiersQuery request, CancellationToken cancellationToken)
        {
            return await _db.Events
                .AsNoTracking()
                .OrderByDescending(x => x.DateEvent)
                .ToListAsync(cancellationToken);
        }
    }
}
