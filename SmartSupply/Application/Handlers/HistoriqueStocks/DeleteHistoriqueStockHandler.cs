using MediatR;
using SmartSupply.Application.Commands.HistoriqueStocks;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.HistoriqueStocks
{
    public class DeleteHistoriqueStockHandler : IRequestHandler<DeleteHistoriqueStockCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public DeleteHistoriqueStockHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteHistoriqueStockCommand request, CancellationToken cancellationToken)
        {
            var ent = await _db.HistoriqueStock.FindAsync(new object[] { request.Id }, cancellationToken);
            if (ent == null) return false;

            _db.HistoriqueStock.Remove(ent);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
