using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.LigneCommandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.LigneCommandes
{
    public class GetLigneCommandesHandler : IRequestHandler<GetLigneCommandesQuery, List<LigneCommande>>
    {
        private readonly SmartSupplyDbContext _db;

        public GetLigneCommandesHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<List<LigneCommande>> Handle(GetLigneCommandesQuery request, CancellationToken cancellationToken)
        {
            return await _db.LignesCommande
                .Include(l => l.Commande)
                .Include(l => l.Produit)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
