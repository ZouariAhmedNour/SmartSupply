using Microsoft.EntityFrameworkCore;
using MediatR;
using SmartSupply.Application.Commands.Utilisateurs;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;


namespace SmartSupply.Application.Handlers.Utilisateurs
{
    public class UpdateUtilisateurHandler : IRequestHandler<UpdateUtilisateurCommand, Utilisateur?>
    {
        private readonly SmartSupplyDbContext _db;

        public UpdateUtilisateurHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<Utilisateur?> Handle(UpdateUtilisateurCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Utilisateurs.FindAsync(new object[] { request.Id }, cancellationToken);
            if (user == null)
                return null; // utilisateur introuvable

            // Vérifier unicité de l'email si modifié
            if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _db.Utilisateurs.AnyAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);
                if (exists)
                    return null; // email déjà utilisé
            }

            user.Nom = request.Nom;
            user.Prenom = request.Prenom;
            user.Email = request.Email;
            user.Role = request.Role;

            _db.Utilisateurs.Update(user);
            await _db.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}
