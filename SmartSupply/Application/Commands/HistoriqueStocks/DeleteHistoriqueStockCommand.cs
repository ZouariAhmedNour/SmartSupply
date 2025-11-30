using MediatR;

namespace SmartSupply.Application.Commands.HistoriqueStocks
{
    public record DeleteHistoriqueStockCommand(int Id): IRequest<bool>;
}
