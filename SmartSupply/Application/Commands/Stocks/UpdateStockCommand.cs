using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Commands.Stocks
{
    public record UpdateStockCommand
    (
        int Id,
        int ProduitId,
        int EntrepotId,
        int QuantiteDisponible,
        int SeuilAlerte
    ) : IRequest<Stock?>;
}
