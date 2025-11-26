using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Utilisateurs;
using SmartSupply.Domain.Models;
using SmartSupply.Helpers;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Utilisateurs
{
    public class RegisterUtilisateurHandler : IRequestHandler<RegisterUtilisateurCommand, Utilisateur?>
    {
        private readonly SmartSupplyDbContext _context;

        public RegisterUtilisateurHandler(SmartSupplyDbContext context)
        {
            _context = context;
        }

        public async Task<Utilisateur?> Handle(RegisterUtilisateurCommand request, CancellationToken cancellationToken)
        {
            // Vérification mots de passe
            if (request.Password != request.ConfirmPassword)
                return null;

            // Vérifier si l’email existe déjà
            var emailExists = await _context.Utilisateurs
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (emailExists)
                return null;

            // Création de l’utilisateur
            var user = new Utilisateur
            {
                Nom = request.Nom,
                Prenom = request.Prenom,
                Email = request.Email,
                MdpHashed = PasswordHelper.HashPassword(request.Password),
                Role = request.Role
            };

            _context.Utilisateurs.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}
