using MediatR;
using SmartSupply.Domain.Models;
using System.Collections.Generic;

namespace SmartSupply.Application.Queries.Utilisateurs
{
    public record GetUtilisateursQuery() : IRequest<List<Utilisateur>>;
}
