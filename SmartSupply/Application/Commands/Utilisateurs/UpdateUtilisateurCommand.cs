using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Commands.Utilisateurs
{
    public record UpdateUtilisateurCommand(
        int Id,
        string Nom,
        string Prenom,
        string Email,
        string Role
    ) : IRequest<Utilisateur?>;
}
