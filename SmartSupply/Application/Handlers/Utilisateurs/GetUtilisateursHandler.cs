using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Utilisateurs;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartSupply.Application.Handlers.Utilisateurs
{
    public class GetUtilisateursHandler : IRequestHandler<GetUtilisateursQuery, List<Utilisateur>>
    {
        private readonly SmartSupplyDbContext _db;

        public GetUtilisateursHandler(SmartSupplyDbContext db) => _db = db;

        public async Task<List<Utilisateur>> Handle(GetUtilisateursQuery request, CancellationToken cancellationToken)
        {
            return await _db.Utilisateurs
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
