using MediatR;

namespace SmartSupply.Application.Commands.Utilisateurs
{
    public record DeleteUtilisateurCommand(int Id) : IRequest<bool>;
}
