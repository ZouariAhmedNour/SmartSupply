using MediatR;
using SmartSupply.Application.Common;

namespace SmartSupply.Application.Handlers.Commandes
{
    public record UpdateCommandeCommand(int Id , string ClientNom, string ClientEmail, string Statut) : IRequest<Result>;
}
