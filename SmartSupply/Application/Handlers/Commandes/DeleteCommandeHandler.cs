using MediatR;
using SmartSupply.Application.Commands.Commandes;
using SmartSupply.Application.Common;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Commandes
{
    public class DeleteCommandeHandler : IRequestHandler<DeleteCommandeCommand, Result>
    {
        private readonly SmartSupplyDbContext _context;
        public DeleteCommandeHandler(SmartSupplyDbContext context) => _context = context;

        public async Task<Result> Handle(DeleteCommandeCommand request, CancellationToken cancellationToken)
        {
            var commande = await _context.Commandes.FindAsync(new object?[] { request.Id }, cancellationToken);
            if (commande == null) return new Result(false, null, "Commande introuvable");

            _context.Commandes.Remove(commande);
            await _context.SaveChangesAsync(cancellationToken);
            return new Result(true);
        }

    }
}
