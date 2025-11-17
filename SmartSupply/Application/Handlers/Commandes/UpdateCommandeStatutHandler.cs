using MediatR;
using SmartSupply.Application.Commands.Commandes;
using SmartSupply.Application.Common;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Commandes
{
    public class UpdateCommandeStatutHandler : IRequestHandler<UpdateCommandeStatutCommand, Result>
    {
        private readonly SmartSupplyDbContext _context;
        public UpdateCommandeStatutHandler(SmartSupplyDbContext context) => _context = context;
        public async Task<Result> Handle(UpdateCommandeStatutCommand request, CancellationToken cancellationToken)
        {
            var cmd = await _context.Commandes.FindAsync(new object?[] { request.CommandeId }, cancellationToken);
            if (cmd == null) return new Result(false, null, "Commande introuvable");

            cmd.Statut = request.Statut;
            await _context.SaveChangesAsync(cancellationToken);
            return new Result(true);
        }
    }
    
}
