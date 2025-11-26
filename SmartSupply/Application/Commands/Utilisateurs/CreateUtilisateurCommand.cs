using MediatR;

namespace SmartSupply.Application.Commands.Utilisateurs
{
    public record CreateUtilisateurCommand(
        string Nom,
        string Prenom,
        string Email,
        string MotDePasse,
        string Role
    ) : IRequest<bool>;

}
