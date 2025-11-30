using MediatR;

namespace SmartSupply.Application.Commands.Stocks
{
    public record DeleteStockCommand (int Id) : IRequest<bool>;
    
}
