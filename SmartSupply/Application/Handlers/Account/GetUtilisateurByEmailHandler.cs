using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Utilisateurs;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;


namespace SmartSupply.Application.Handlers.Utilisateurs
{
    public class GetUtilisateurByEmailHandler : IRequestHandler<GetUtilisateurByEmailQuery, Utilisateur>
    {
        private readonly SmartSupplyDbContext _context;

        public GetUtilisateurByEmailHandler(SmartSupplyDbContext context)
        {
            _context = context;
        }

        public async Task<Utilisateur> Handle(GetUtilisateurByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        }
    }
}
