using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Stocks
{
    public record GetStockByIdQuery(int Id) : IRequest<Stock?>;
   
}
