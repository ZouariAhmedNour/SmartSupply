using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Produits;
using SmartSupply.Application.Common;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Produits
{
    public class CreateProduitHandler : IRequestHandler<CreateProduitCommand, Result<int>>
    {
        private readonly SmartSupplyDbContext _context;

        public async Task<Result<int>> Handle(CreateProduitCommand request, CancellationToken cancellationToken)
        {
            // vérifie duplicat SKU
            if (await _context.Produits.AnyAsync(p => p.CodeSKU == request.CodeSKU, cancellationToken))
                return new Result<int>(false, default, "Code SKU déjà existant");

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

            return new Result<int>(true, produit.Id);
        }
    }
}
