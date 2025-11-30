using MediatR;

namespace SmartSupply.Application.Commands.LigneCommandes
{
    public record DeleteLigneCommandeCommand(int Id) : IRequest<bool>;
}
