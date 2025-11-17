using MediatR;
using SmartSupply.Application.Common;

namespace SmartSupply.Application.Commands.Commandes
{
   public record UpdateCommandeStatutCommand(int CommandeId, string Statut) : IRequest<Result>;
}
