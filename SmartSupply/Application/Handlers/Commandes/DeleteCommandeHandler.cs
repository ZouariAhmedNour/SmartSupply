using MediatR;
using SmartSupply.Application.Commands.Commandes;
using SmartSupply.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SmartSupply.Application.Handlers.Commandes
{
    public record DeleteCommandeHandler : IRequestHandler<DeleteCommandeCommand, bool>
    {
        private readonly SmartSupplyDbContext _context;

        public DeleteCommandeHandler(SmartSupplyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCommandeCommand request, CancellationToken cancellationToken)
        {
            var commande = await _context.Commandes.FindAsync(new object?[] { request.Id }, cancellationToken);
            if (commande == null)
                return false; // Commande introuvable

            _context.Commandes.Remove(commande);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
