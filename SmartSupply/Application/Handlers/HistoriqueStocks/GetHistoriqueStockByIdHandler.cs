using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.HistoriqueStocks;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.HistoriqueStocks
{
    public class GetHistoriqueStockByIdHandler : IRequestHandler<GetHistoriqueStockByIdQuery, HistoriqueStock?>
    {
        private readonly SmartSupplyDbContext _db;

        public GetHistoriqueStockByIdHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<HistoriqueStock?> Handle(GetHistoriqueStockByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.HistoriqueStock
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
        }
        }
    }

