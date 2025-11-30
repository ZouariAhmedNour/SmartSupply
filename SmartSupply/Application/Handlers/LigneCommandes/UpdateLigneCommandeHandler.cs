using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.LigneCommandeCommand;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.LigneCommandes
{
    public class UpdateLigneCommandeHandler : IRequestHandler<UpdateLigneCommandeCommand, LigneCommande?>
    {
        private readonly SmartSupplyDbContext _db;

        public UpdateLigneCommandeHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<LigneCommande?> Handle(UpdateLigneCommandeCommand request, CancellationToken cancellationToken)
        {
            var ligne = await _db.LignesCommande.FindAsync(new object[] { request.Id }, cancellationToken);
            if (ligne == null)
                return null;

            if (request.CommandeId <= 0 || request.ProduitId <= 0)
                return null;

            if (request.Quantite <= 0)
                return null;

            if (request.PrixUnitaire < 0)
                return null;

            // vérifier existence commande & produit
            var commandeExists = await _db.Commandes.AnyAsync(c => c.Id == request.CommandeId, cancellationToken);
            var produitExists = await _db.Produits.AnyAsync(p => p.Id == request.ProduitId, cancellationToken);
            if (!commandeExists || !produitExists)
                return null;

            ligne.CommandeId = request.CommandeId;
            ligne.ProduitId = request.ProduitId;
            ligne.Quantite = request.Quantite;
            ligne.PrixUnitaire = request.PrixUnitaire;

            _db.LignesCommande.Update(ligne);
            await _db.SaveChangesAsync(cancellationToken);

            return ligne;
        }
    }
}
