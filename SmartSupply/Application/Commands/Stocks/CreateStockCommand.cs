using MediatR;

namespace SmartSupply.Application.Commands.Stocks
{
    public record CreateStockCommand
   (
        int ProduitId,
        int EntrepotId,
        int QuantiteDisponible,
        int SeuilAlerte
    ) : IRequest<bool>;
}
