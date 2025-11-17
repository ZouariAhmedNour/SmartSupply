using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Common;
using SmartSupply.Application.Queries.Commandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Commandes
{
    public class GetCommandesHandler : IRequestHandler<GetCommandesQuery, Result<List<Commande>>>
    {
        private readonly SmartSupplyDbContext context;
        public GetCommandesHandler(SmartSupplyDbContext dbContext) => context = dbContext;

        public async Task<Result<List<Commande>>> Handle(GetCommandesQuery request, CancellationToken cancellationToken)
        {
            var list = await context.Commandes
                .Include(c => c.Lignes)
                .OrderByDescending(c => c.DateCreation)
                .ToListAsync(cancellationToken);
            return new Result<List<Commande>>(true, list);
        }

    }
}
