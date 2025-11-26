using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Commandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartSupply.Application.Handlers.Commandes
{
    public class GetCommandesHandler : IRequestHandler<GetCommandesQuery, List<Commande>>
    {
        private readonly SmartSupplyDbContext _db;

        public GetCommandesHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<List<Commande>> Handle(GetCommandesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Commandes
                .Include(c => c.Lignes)
                .OrderByDescending(c => c.DateCreation)
                .ToListAsync(cancellationToken);
        }
    }
}
