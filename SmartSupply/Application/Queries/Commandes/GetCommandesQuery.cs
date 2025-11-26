using MediatR;
using SmartSupply.Domain.Models;
using System.Collections.Generic;

namespace SmartSupply.Application.Queries.Commandes
{
    // Retourne la liste de toutes les commandes
    public record GetCommandesQuery() : IRequest<List<Commande>>;
}
