using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Stocks;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Stocks
{
    public class UpdateStockHandler : IRequestHandler<UpdateStockCommand, Stock?>
    {
        private readonly SmartSupplyDbContext _db;

        public UpdateStockHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<Stock?> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _db.Stocks.FindAsync(new object[] { request.Id }, cancellationToken);
            if (stock == null)
                return null;

            if (request.ProduitId <= 0 || request.EntrepotId <= 0)
                return null;

            if (request.QuantiteDisponible < 0 || request.SeuilAlerte < 0)
                return null;

            // Vérifier existence produit & entrepot
            var produitExists = await _db.Produits.AnyAsync(p => p.Id == request.ProduitId, cancellationToken);
            var entrepotExists = await _db.Entrepots.AnyAsync(e => e.Id == request.EntrepotId, cancellationToken);
            if (!produitExists || !entrepotExists)
                return null;

            // Si on change le couple Produit+Entrepot, vérifier doublon
            if (stock.ProduitId != request.ProduitId || stock.EntrepotId != request.EntrepotId)
            {
                var exists = await _db.Stocks.AnyAsync(s => s.ProduitId == request.ProduitId && s.EntrepotId == request.EntrepotId && s.Id != request.Id, cancellationToken);
                if (exists)
                    return null;
            }

            stock.ProduitId = request.ProduitId;
            stock.EntrepotId = request.EntrepotId;
            stock.QuantiteDisponible = request.QuantiteDisponible;
            stock.SeuilAlerte = request.SeuilAlerte;

            _db.Stocks.Update(stock);
            await _db.SaveChangesAsync(cancellationToken);

            return stock;
        }
    }
}
