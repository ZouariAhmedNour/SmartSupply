using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Entrepots;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Entrepots
{
    public class GetEntrepotsHandler : IRequestHandler<GetEntrepotsQuery, List<Entrepot>>
    {
        private readonly SmartSupplyDbContext _db;

        public GetEntrepotsHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<List<Entrepot>> Handle(GetEntrepotsQuery request, CancellationToken cancellationToken)
        {
            return await _db.Entrepots.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
