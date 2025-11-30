using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Produits;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Produits
{
    public class UpdateProduitHandler : IRequestHandler<UpdateProduitCommand, Produit?>
    {
        private readonly SmartSupplyDbContext _db;

        public UpdateProduitHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<Produit?> Handle(UpdateProduitCommand request, CancellationToken cancellationToken)
        {
            var produit = await _db.Produits.FindAsync(new object[] { request.Id }, cancellationToken);
            if (produit == null)
                return null;

            // Vérifier unicité du CodeSKU si modifié
            if (!string.Equals(produit.CodeSKU, request.CodeSKU, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _db.Produits.AnyAsync(p => p.CodeSKU == request.CodeSKU && p.Id != request.Id, cancellationToken);
                if (exists)
                    return null;
            }

            produit.Nom = request.Nom;
            produit.Description = request.Description;
            produit.CodeSKU = request.CodeSKU;
            produit.PrixUnitaire = request.PrixUnitaire;

            _db.Produits.Update(produit);
            await _db.SaveChangesAsync(cancellationToken);

            return produit;
        }
    }
}
