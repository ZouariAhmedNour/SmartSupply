using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Commands.Entrepots
{
    public record UpdateEntrepotCommand
    (
        int Id,
        string Nom,
        string Adresse,
        int CapaciteMax

        ) : IRequest<Entrepot?>;
}
