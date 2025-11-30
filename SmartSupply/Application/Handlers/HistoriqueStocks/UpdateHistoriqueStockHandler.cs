using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.HistoriqueStocks;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.HistoriqueStocks
{
    public class UpdateHistoriqueStockHandler : IRequestHandler<UpdateHistoriqueStockCommand, HistoriqueStock?>
    {
        private readonly SmartSupplyDbContext _db;

        public UpdateHistoriqueStockHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<HistoriqueStock?> Handle(UpdateHistoriqueStockCommand request, CancellationToken cancellationToken)
        {
            var existing = await _db.HistoriqueStock.FindAsync(new object[] { request.Id }, cancellationToken);
            if (existing == null)
                return null;

            if (request.ProduitId <= 0 || request.EntrepotId <= 0)
                return null;

            if (string.IsNullOrWhiteSpace(request.TypeMouvement))
                return null;

            // vérifier existence produit & entrepot
            var produitExists = await _db.Produits.AnyAsync(p => p.Id == request.ProduitId, cancellationToken);
            var entrepotExists = await _db.Entrepots.AnyAsync(e => e.Id == request.EntrepotId, cancellationToken);
            if (!produitExists || !entrepotExists)
                return null;

            existing.ProduitId = request.ProduitId;
            existing.EntrepotId = request.EntrepotId;
            existing.Quantite = request.Quantite;
            existing.DateMouvement = request.DateMouvement;
            existing.TypeMouvement = request.TypeMouvement;
            existing.Commentaire = request.Commentaire;

            _db.HistoriqueStock.Update(existing);
            await _db.SaveChangesAsync(cancellationToken);

            return existing;
        }
    }
}
