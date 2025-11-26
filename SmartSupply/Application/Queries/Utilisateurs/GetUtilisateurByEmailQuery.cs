using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Utilisateurs
{
    public record GetUtilisateurByEmailQuery(string Email) : IRequest<Utilisateur>;
}
