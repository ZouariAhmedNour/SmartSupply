using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Common;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Commandes
{
    public class UpdateCommandeHandler : IRequestHandler<UpdateCommandeCommand, Result>
    {
        private readonly SmartSupplyDbContext _db;
        public UpdateCommandeHandler(SmartSupplyDbContext db) => _db = db;

        public async Task<Result> Handle(UpdateCommandeCommand request, CancellationToken cancellationToken)
        {
            var cmd = await _db.Commandes.FindAsync(new object?[] { request.Id }, cancellationToken);
            if (cmd == null) return new Result(false, "Commande introuvable");

            // Mappage simple (tu peux mettre de la validation / règles métiers ici)
            cmd.ClientNom = request.ClientNom;
            cmd.ClientEmail = request.ClientEmail;
            cmd.Statut = request.Statut;

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
                return new Result(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Optionnel : gérer concurrence
                return new Result(false, "Conflit de mise à jour, réessaye.");
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
