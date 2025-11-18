using MediatR;
using SmartSupply.Application.Common;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Utilisateurs
{
    public record GetUtilisateurByIdQuery(int Id) : IRequest<Result<Utilisateur>>
    {
    }
}
