using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Entrepots;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Entrepots
{
    public class GetEntrepotByIdHandler : IRequestHandler<GetEntrepotByIdQuery, Entrepot?>
    {
        private readonly SmartSupplyDbContext _db;

        public GetEntrepotByIdHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<Entrepot?> Handle(GetEntrepotByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Entrepots.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        }
    }
}
