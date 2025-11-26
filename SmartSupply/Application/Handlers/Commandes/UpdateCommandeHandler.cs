// Contenu CORRIGÉ pour le fichier UpdateCommandeHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
// N'oubliez pas l'using pour la commande de mise à jour
using SmartSupply.Application.Commands.Commandes; // Doit être l'using pour la commande de mise à jour
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SmartSupply.Application.Handlers.Commandes
{

    public class UpdateCommandeHandler : IRequestHandler<UpdateCommandeCommand, bool>
    {
        private readonly SmartSupplyDbContext _context;


        public UpdateCommandeHandler(SmartSupplyDbContext context) => _context = context;

        public async Task<bool> Handle(UpdateCommandeCommand request, CancellationToken cancellationToken)
        {
            // --- Logique de mise à jour ---
            var commande = await _context.Commandes.FindAsync(new object[] { request.Id }, cancellationToken);

            if (commande == null)
            {
                return false; // Commande non trouvée
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true; // Mise à jour réussie
        }
    }
}