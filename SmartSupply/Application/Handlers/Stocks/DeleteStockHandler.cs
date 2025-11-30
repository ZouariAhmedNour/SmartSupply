using MediatR;
using SmartSupply.Application.Commands.Stocks;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Stocks
{
    public class DeleteStockHandler : IRequestHandler<DeleteStockCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public DeleteStockHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _db.Stocks.FindAsync(new object[] { request.Id }, cancellationToken);
            if (stock == null)
                return false;

            _db.Stocks.Remove(stock);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
