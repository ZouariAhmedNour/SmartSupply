using MediatR;
using SmartSupply.Application.Common;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Utilisateurs
{
    public record GetUtilisateursQuery() : IRequest<Result<List<Utilisateur>>>
    {
    }
}
