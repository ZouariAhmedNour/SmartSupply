using MediatR;

namespace SmartSupply.Application.Commands.Entrepots
{
    public record DeleteEntrepotCommand(int Id) : IRequest<bool>;

}
