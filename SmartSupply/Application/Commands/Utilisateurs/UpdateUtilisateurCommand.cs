using MediatR;
using SmartSupply.Application.Common;

namespace SmartSupply.Application.Commands.Utilisateurs
{
    public record UpdateUtilisateurCommand(
        int Id,
        string Nom,
        string Prenom,
        string Email,
        string Role
    ) : IRequest<Result>;

}
