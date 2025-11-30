using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Entrepots;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Entrepots
{
    public class CreateEntrepotHandler : IRequestHandler<CreateEntrepotCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public CreateEntrepotHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(CreateEntrepotCommand request, CancellationToken cancellationToken)
        {
            // validations simples
            if (string.IsNullOrWhiteSpace(request.Nom) || string.IsNullOrWhiteSpace(request.Adresse))
                return false;

            if (request.CapaciteMax <= 0)
                return false;

            // Optional : unicité du nom
            var exists = await _db.Entrepots.AnyAsync(e => e.Nom == request.Nom, cancellationToken);
            if (exists)
                return false;

            var entrepot = new Entrepot
            {
                Nom = request.Nom,
                Adresse = request.Adresse,
                CapaciteMax = request.CapaciteMax,
                DateCreation = DateTime.UtcNow
            };

            _db.Entrepots.Add(entrepot);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
