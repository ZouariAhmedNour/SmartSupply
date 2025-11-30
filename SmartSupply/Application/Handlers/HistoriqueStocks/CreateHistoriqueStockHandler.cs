using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.HistoriqueStocks;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.HistoriqueStocks
{
    public class CreateHistoriqueStockHandler : IRequestHandler<CreateHistoriqueStockCommand, bool>

    {
        private readonly SmartSupplyDbContext _db;

        public CreateHistoriqueStockHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(CreateHistoriqueStockCommand request, CancellationToken cancellationToken)
        {
            // validations basiques
            if (request.ProduitId <= 0 || request.EntrepotId <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(request.TypeMouvement))
                return false;

            // vérifier que produit & entrepot existent
            var produitExists = await _db.Produits.AnyAsync(p => p.Id == request.ProduitId, cancellationToken);
            var entrepotExists = await _db.Entrepots.AnyAsync(e => e.Id == request.EntrepotId, cancellationToken);
            if (!produitExists || !entrepotExists)
                return false;

            var h = new HistoriqueStock
            {
                ProduitId = request.ProduitId,
                EntrepotId = request.EntrepotId,
                Quantite = request.Quantite,
                TypeMouvement = request.TypeMouvement,
                Commentaire = request.Commentaire,
                DateMouvement = DateTime.UtcNow
            };

            _db.HistoriqueStock.Add(h);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
