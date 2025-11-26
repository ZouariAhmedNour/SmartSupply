using MediatR;

namespace SmartSupply.Application.Commands.Commandes
{
    public record DeleteCommandeCommand(int Id) : IRequest<bool>;
    
}
