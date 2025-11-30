using MediatR;
using SmartSupply.Application.Commands.Entrepots;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Entrepots
{
    public class DeleteEntrepotHandler : IRequestHandler<DeleteEntrepotCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public DeleteEntrepotHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteEntrepotCommand request, CancellationToken cancellationToken)
        {
            var entrepot = await _db.Entrepots.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entrepot == null)
                return false;

            _db.Entrepots.Remove(entrepot);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
