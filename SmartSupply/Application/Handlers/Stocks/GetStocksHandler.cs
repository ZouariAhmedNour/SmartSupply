using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Stocks;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Stocks
{
    public class GetStocksHandler : IRequestHandler<GetStocksQuery, List<Stock>>
    {
        private readonly SmartSupplyDbContext _db;

        public GetStocksHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<List<Stock>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            return await _db.Stocks
                .Include(s => s.Entrepot)
                .Include(s => s.Produit)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
