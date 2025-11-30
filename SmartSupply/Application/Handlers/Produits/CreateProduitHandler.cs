using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Produits;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Produits
{
    public class CreateProduitHandler : IRequestHandler<CreateProduitCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public CreateProduitHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(CreateProduitCommand request, CancellationToken cancellationToken)
        {
            // validations simples
            if (string.IsNullOrWhiteSpace(request.Nom) ||
                string.IsNullOrWhiteSpace(request.CodeSKU))
                return false;

            if (request.PrixUnitaire <= 0)
                return false;

            // unicité sur CodeSKU (optionnel mais recommandé)
            var exists = await _db.Produits.AnyAsync(p => p.CodeSKU == request.CodeSKU, cancellationToken);
            if (exists)
                return false;

            var produit = new Produit
            {
                Nom = request.Nom,
                Description = request.Description,
                CodeSKU = request.CodeSKU,
                PrixUnitaire = request.PrixUnitaire,
                DateCreation = DateTime.UtcNow
            };

            _db.Produits.Add(produit);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
