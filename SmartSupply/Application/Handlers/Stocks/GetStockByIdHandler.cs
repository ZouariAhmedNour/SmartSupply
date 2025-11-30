using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Stocks;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Stocks
{
    public class GetStockByIdHandler : IRequestHandler<GetStockByIdQuery, Stock?>
    {
        private readonly SmartSupplyDbContext _db;

        public GetStockByIdHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<Stock?> Handle(GetStockByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Stocks
                .Include(s => s.Entrepot)
                .Include(s => s.Produit)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        }
    }
}
