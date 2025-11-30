using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.LigneCommandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.LigneCommandes
{
    public class CreateLigneCommandeHandler : IRequestHandler<CreateLigneCommandeCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public CreateLigneCommandeHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(CreateLigneCommandeCommand request, CancellationToken cancellationToken)
        {
            // validations simples
            if (request.CommandeId <= 0 || request.ProduitId <= 0)
                return false;

            if (request.Quantite <= 0)
                return false;

            if (request.PrixUnitaire < 0)
                return false;

            // vérifier existence commande & produit
            var commandeExists = await _db.Commandes.AnyAsync(c => c.Id == request.CommandeId, cancellationToken);
            var produitExists = await _db.Produits.AnyAsync(p => p.Id == request.ProduitId, cancellationToken);
            if (!commandeExists || !produitExists)
                return false;

            var ligne = new LigneCommande
            {
                CommandeId = request.CommandeId,
                ProduitId = request.ProduitId,
                Quantite = request.Quantite,
                PrixUnitaire = request.PrixUnitaire
            };

            _db.LignesCommande.Add(ligne);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
