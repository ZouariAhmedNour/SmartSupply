
using MediatR;
using SmartSupply.Application.Commands.Utilisateurs;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Utilisateurs
{
    public class DeleteUtilisateurHandler : IRequestHandler<DeleteUtilisateurCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public DeleteUtilisateurHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteUtilisateurCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Utilisateurs.FindAsync(new object[] { request.Id }, cancellationToken);
            if (user == null)
                return false; // utilisateur introuvable

            _db.Utilisateurs.Remove(user);
            await _db.SaveChangesAsync(cancellationToken);

            return true; // suppression réussie
        }
    }
}
