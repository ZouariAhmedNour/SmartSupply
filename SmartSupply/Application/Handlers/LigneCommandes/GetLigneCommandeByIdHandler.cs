using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.LigneCommandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.LigneCommandes
{
    public class GetLigneCommandeByIdHandler : IRequestHandler<GetLigneCommandeByIdQuery, LigneCommande?>
    {
        private readonly SmartSupplyDbContext _db;

        public GetLigneCommandeByIdHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<LigneCommande?> Handle(GetLigneCommandeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.LignesCommande
                .Include(l => l.Commande)
                .Include(l => l.Produit)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);
        }
    }
}
