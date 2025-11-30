using MediatR;
using SmartSupply.Domain.Models;

namespace SmartSupply.Application.Commands.HistoriqueStocks
{
    public record UpdateHistoriqueStockCommand
    (
      int Id,
        int ProduitId,
        int EntrepotId,
        int Quantite,
        DateTime DateMouvement,
        string TypeMouvement,
        string? Commentaire
    ) : IRequest<HistoriqueStock?>;
}
