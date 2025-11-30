using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.HistoriqueStocks
{
    public record GetHistoriqueStocksQuery()  : IRequest<List<HistoriqueStock>>;

}
