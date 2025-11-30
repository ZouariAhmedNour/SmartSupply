using MediatR;
using SmartSupply.Application.Commands.LigneCommandes;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.LigneCommandes
{
    public class DeleteLigneCommandeHandler : IRequestHandler<DeleteLigneCommandeCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public DeleteLigneCommandeHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteLigneCommandeCommand request, CancellationToken cancellationToken)
        {
            var ligne = await _db.LignesCommande.FindAsync(new object[] { request.Id }, cancellationToken);
            if (ligne == null)
                return false;

            _db.LignesCommande.Remove(ligne);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
