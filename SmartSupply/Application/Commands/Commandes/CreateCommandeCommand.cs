using MediatR;
using SmartSupply.Application.Common;
using SmartSupply.Domain.Models;


namespace SmartSupply.Application.Commands.Commandes
{
    public record CreateCommandeCommand(string ClientNom, string ClientEmail, List<LigneCommande> Lignes) : IRequest<Result<int>>;
    
}
