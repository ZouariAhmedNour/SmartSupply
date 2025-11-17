using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SmartSupply.Application.Common;
using SmartSupply.Application.Queries.Commandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Commandes
{
    public class GetCommandeByIdHandler : IRequestHandler<GetCommandeByIdQuery, Result<Commande>>
    {
        private readonly SmartSupplyDbContext _db;
        public GetCommandeByIdHandler(SmartSupplyDbContext db) => _db = db;

        public async Task<Result<Commande>> Handle(GetCommandeByIdQuery request, CancellationToken cancellationToken)
        {
            var c = await _db.Commandes
                             .Include(x => x.Lignes)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (c == null) return new Result<Commande>(false, default, "Commande introuvable");
            return new Result<Commande>(true, c);
        }
    }
}
