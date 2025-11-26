using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Produits;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Produits
{
    public class CreateProduitHandler : IRequestHandler<CreateProduitCommand, int>
    {
        private readonly SmartSupplyDbContext _context;

        public CreateProduitHandler(SmartSupplyDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProduitCommand request, CancellationToken cancellationToken)
        {
            // Vérifie duplicat SKU
            if (await _context.Produits.AnyAsync(p => p.CodeSKU == request.CodeSKU, cancellationToken))
                throw new InvalidOperationException("Code SKU déjà existant");

            var produit = new Produit
            {
                Nom = request.Nom,
                Description = request.Description,
                CodeSKU = request.CodeSKU,
                PrixUnitaire = request.PrixUnitaire,
                DateCreation = DateTime.UtcNow
            };

            _context.Produits.Add(produit);
            await _context.SaveChangesAsync(cancellationToken);

            return produit.Id; // Retourne l'ID du produit créé
        }
    }
}
