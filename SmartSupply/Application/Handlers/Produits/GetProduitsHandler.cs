using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Produits;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Produits
{
    public class GetProduitsHandler : IRequestHandler<GetProduitsQuery, List<Produit>>
    {
        private readonly SmartSupplyDbContext _db;

        public GetProduitsHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<List<Produit>> Handle(GetProduitsQuery request, CancellationToken cancellationToken)
        {
            return await _db.Produits.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
