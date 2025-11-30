using MediatR;
using SmartSupply.Application.Commands.EventMetiers;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.EventMetiers
{
    public class CreateEventMetierHandler : IRequestHandler<CreateEventMetierCommand, bool>
    {
        private readonly SmartSupplyDbContext _db;

        public CreateEventMetierHandler(SmartSupplyDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(CreateEventMetierCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.TypeEvent))
                return false;

            if (string.IsNullOrWhiteSpace(request.Donnees))
                return false;

            var ev = new EventMetier
            {
                TypeEvent = request.TypeEvent,
                Donnees = request.Donnees,
                DateEvent = DateTime.UtcNow
            };

            _db.Events.Add(ev);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
