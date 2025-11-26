using MediatR;
using SmartSupply.Application.Commands.Commandes;
using SmartSupply.Infrastructure;


namespace SmartSupply.Application.Handlers.Commandes
{
    public class UpdateCommandeStatutHandler : IRequestHandler<UpdateCommandeStatutCommand, bool>
    {
        private readonly SmartSupplyDbContext _context;

        public UpdateCommandeStatutHandler(SmartSupplyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateCommandeStatutCommand request, CancellationToken cancellationToken)
        {
            var cmd = await _context.Commandes.FindAsync(new object?[] { request.CommandeId }, cancellationToken);
            if (cmd == null)
                return false; // commande introuvable

            cmd.Statut = request.Statut;
            await _context.SaveChangesAsync(cancellationToken);
            return true; // mise à jour réussie
        }
    }
}
