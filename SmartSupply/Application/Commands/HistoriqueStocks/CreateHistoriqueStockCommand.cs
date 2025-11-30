using MediatR;

namespace SmartSupply.Application.Commands.HistoriqueStocks
{
    public record CreateHistoriqueStockCommand
   (
      int ProduitId,
        int EntrepotId,
        int Quantite,
        string TypeMouvement,
        string? Commentaire
    ) : IRequest<bool>;
}
