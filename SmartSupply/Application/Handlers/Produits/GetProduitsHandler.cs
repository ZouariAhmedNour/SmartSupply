using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Common;
using SmartSupply.Application.DTOs;
using SmartSupply.Application.Queries.Produits;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Produits
{
    public class GetProduitsHandler : IRequestHandler<GetProduitsQuery, Result<List<ProduitDto>>>

    {
        private readonly SmartSupplyDbContext _context;
        public GetProduitsHandler(SmartSupplyDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<ProduitDto>>> Handle(GetProduitsQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.Produits
             .OrderBy(p => p.Nom)
             .Select(p => new ProduitDto(p.Id, p.Nom, p.Description, p.CodeSKU, p.PrixUnitaire, p.DateCreation))
             .ToListAsync(cancellationToken);

            return new Result<List<ProduitDto>>(true, items);
        }
    }
}
