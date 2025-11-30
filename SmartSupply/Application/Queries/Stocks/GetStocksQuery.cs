using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Stocks
{
    public record GetStocksQuery() : IRequest<List<Stock>>;
   
}
