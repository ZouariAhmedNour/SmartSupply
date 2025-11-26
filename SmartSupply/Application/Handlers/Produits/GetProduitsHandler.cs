using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Queries.Produits;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Produits
{
    public class GetProduitByIdHandler : IRequestHandler<GetProduitByIdQuery, Produit?>
    {
        private readonly SmartSupplyDbContext _context;

        public GetProduitByIdHandler(SmartSupplyDbContext context)
        {
            _context = context;
        }

        public async Task<Produit?> Handle(GetProduitByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Produits
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        }
    }
}
