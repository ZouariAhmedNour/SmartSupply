using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediatR;
using SmartSupply.Application.Commands.Utilisateurs;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Utilisateurs
{
    public class CreateUtilisateurHandler : IRequestHandler<CreateUtilisateurCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;
        private readonly PasswordHasher<Utilisateur> _passwordHasher;

        public CreateUtilisateurHandler(SmartSupplyDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<Utilisateur>();
        }

        public async Task<bool> Handle(CreateUtilisateurCommand request, CancellationToken cancellationToken)
        {
            // Vérification des champs nécessaires
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.MotDePasse))
            {
                return false;
            }

            // Vérifier si email déjà utilisé
            var exists = await _db.Utilisateurs
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (exists)
                return false;

            // Création de l'utilisateur
            var user = new Utilisateur
            {
                Nom = request.Nom,
                Prenom = request.Prenom,
                Email = request.Email,
                Role = request.Role,
                DateCreation = DateTime.UtcNow
            };

            // Hash du mot de passe
            user.MdpHashed = _passwordHasher.HashPassword(user, request.MotDePasse);

            // Save en DB
            _db.Utilisateurs.Add(user);
            await _db.SaveChangesAsync(cancellationToken);

            return true; // création OK
        }
    }
}
