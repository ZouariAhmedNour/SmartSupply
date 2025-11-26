using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Commandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SmartSupply.Application.Handlers.Commandes
{
    public class GetCommandeByIdHandler : IRequestHandler<GetCommandeByIdQuery, Commande?>
    {
        private readonly SmartSupplyDbContext _db;

        public GetCommandeByIdHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<Commande?> Handle(GetCommandeByIdQuery request, CancellationToken cancellationToken)
        {
            var commande = await _db.Commandes
                .Include(x => x.Lignes)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return commande; // retourne null si non trouvé
        }
    }
}
