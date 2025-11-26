using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Utilisateurs;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SmartSupply.Application.Handlers.Utilisateurs
{
    public class GetUtilisateurByIdHandler : IRequestHandler<GetUtilisateurByIdQuery, Utilisateur?>
    {
        private readonly SmartSupplyDbContext _db;

        public GetUtilisateurByIdHandler(SmartSupplyDbContext db) => _db = db;

        public async Task<Utilisateur?> Handle(GetUtilisateurByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Utilisateurs
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        }
    }
}
