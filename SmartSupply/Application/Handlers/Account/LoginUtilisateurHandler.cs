using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Utilisateurs;
using SmartSupply.Domain.Models;
using SmartSupply.Helpers;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Utilisateurs
{
    public class LoginUtilisateurHandler : IRequestHandler<LoginUtilisateurCommand, Utilisateur?>
    {
        private readonly SmartSupplyDbContext _context;

        public LoginUtilisateurHandler(SmartSupplyDbContext context)
        {
            _context = context;
        }

        public async Task<Utilisateur?> Handle(LoginUtilisateurCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null)
                return null;

            if (!PasswordHelper.VerifyPassword(request.Password, user.MdpHashed))
                return null;

            return user;
        }
    }
}
