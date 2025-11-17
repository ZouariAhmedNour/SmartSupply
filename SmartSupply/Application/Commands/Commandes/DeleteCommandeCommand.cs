using MediatR;
using SmartSupply.Application.Common;

namespace SmartSupply.Application.Commands.Commandes
{
    public record DeleteCommandeCommand(int Id) : IRequest<Result>;
    
}
