using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Produits;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Produits
{
    public class GetProduitByIdHandler : IRequestHandler<GetProduitByIdQuery, Produit?>
    {
        private readonly SmartSupplyDbContext _db;

        public GetProduitByIdHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<Produit?> Handle(GetProduitByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Produits.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        }
    }
}
