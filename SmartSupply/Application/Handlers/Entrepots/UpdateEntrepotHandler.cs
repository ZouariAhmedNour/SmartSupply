using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Entrepots;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Entrepots
{
    public class UpdateEntrepotHandler : IRequestHandler<UpdateEntrepotCommand, Entrepot?>

    {
        private readonly SmartSupplyDbContext _db;

        public UpdateEntrepotHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<Entrepot?> Handle(UpdateEntrepotCommand request, CancellationToken cancellationToken)
        {
            var entrepot = await _db.Entrepots.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entrepot == null)
                return null;

            // Vérifier unicité du nom si modifié
            if (!string.Equals(entrepot.Nom, request.Nom, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _db.Entrepots.AnyAsync(e => e.Nom == request.Nom && e.Id != request.Id, cancellationToken);
                if (exists)
                    return null;
            }

            entrepot.Nom = request.Nom;
            entrepot.Adresse = request.Adresse;
            entrepot.CapaciteMax = request.CapaciteMax;

            _db.Entrepots.Update(entrepot);
            await _db.SaveChangesAsync(cancellationToken);

            return entrepot;
        }
    }
}
