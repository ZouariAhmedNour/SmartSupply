using MediatR;
using SmartSupply.Application.Commands.Produits;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Produits
{
    public class DeleteProduitHandler : IRequestHandler<DeleteProduitCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public DeleteProduitHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteProduitCommand request, CancellationToken cancellationToken)
        {
            var produit = await _db.Produits.FindAsync(new object[] { request.Id }, cancellationToken);
            if (produit == null)
                return false;

            _db.Produits.Remove(produit);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
