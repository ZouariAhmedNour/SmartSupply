using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Stocks;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Stocks
{
    public class CreateStockHandler : IRequestHandler<CreateStockCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public CreateStockHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            // validations de base
            if (request.ProduitId <= 0 || request.EntrepotId <= 0)
                return false;

            if (request.QuantiteDisponible < 0 || request.SeuilAlerte < 0)
                return false;

            // Vérifier que produit & entrepot existent
            var produitExists = await _db.Produits.AnyAsync(p => p.Id == request.ProduitId, cancellationToken);
            var entrepotExists = await _db.Entrepots.AnyAsync(e => e.Id == request.EntrepotId, cancellationToken);
            if (!produitExists || !entrepotExists)
                return false;

            // Option recommandée : empêcher doublon (même produit dans même entrepôt)
            var already = await _db.Stocks.AnyAsync(s => s.ProduitId == request.ProduitId && s.EntrepotId == request.EntrepotId, cancellationToken);
            if (already)
                return false;

            var stock = new Stock
            {
                ProduitId = request.ProduitId,
                EntrepotId = request.EntrepotId,
                QuantiteDisponible = request.QuantiteDisponible,
                SeuilAlerte = request.SeuilAlerte
            };

            _db.Stocks.Add(stock);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
