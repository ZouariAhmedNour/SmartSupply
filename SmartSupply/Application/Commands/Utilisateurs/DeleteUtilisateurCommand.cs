using MediatR;
using SmartSupply.Application.Common;

namespace SmartSupply.Application.Commands.Utilisateurs
{
    public record DeleteUtilisateurCommand(int Id) : IRequest<Result>;
    
}
