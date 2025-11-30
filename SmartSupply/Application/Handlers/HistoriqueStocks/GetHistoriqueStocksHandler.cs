using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.HistoriqueStocks;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.HistoriqueStocks
{
    public class GetHistoriqueStocksHandler : IRequestHandler<GetHistoriqueStocksQuery, List<HistoriqueStock>>
    {
        private readonly SmartSupplyDbContext _db;

        public GetHistoriqueStocksHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<List<HistoriqueStock>> Handle(GetHistoriqueStocksQuery request, CancellationToken cancellationToken)
        {
            return await _db.HistoriqueStock
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
