using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.HistoriqueStocks
{
    public record GetHistoriqueStockByIdQuery(int Id)  : IRequest<HistoriqueStock?>;
   
}

