using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Queries.Commandes
{
    // Retourne la commande ou null si non trouvée
    public record GetCommandeByIdQuery(int Id) : IRequest<Commande?>;
}
