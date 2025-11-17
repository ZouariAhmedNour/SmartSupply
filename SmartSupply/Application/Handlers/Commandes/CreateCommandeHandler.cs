using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Commandes;
using SmartSupply.Application.Common;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.Application.Handlers.Commandes
{
    public class CreateCommandeHandler : IRequestHandler<CreateCommandeCommand, Result<int>>
    {
        private readonly SmartSupplyDbContext _context;
        public CreateCommandeHandler(SmartSupplyDbContext context) => _context = context;

        public async Task<Result<int>> Handle(CreateCommandeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ClientNom) || string.IsNullOrWhiteSpace(request.ClientEmail))
                return new Result<int>(false, default, "Nom et email requis.");

            var commande = new Commande
            {
                ClientNom = request.ClientNom,
                ClientEmail = request.ClientEmail,
                DateCreation = DateTime.UtcNow,
                Statut = "EnAttente",
                MontantTotal = 0m,
                Lignes = new List<LigneCommande>()
            };

            decimal total = 0m;
            if (request.Lignes != null && request.Lignes.Any())
            {
                foreach (var l in request.Lignes)
                {
                    // récupère le produit
                    var produit = await _context.Produits.FirstOrDefaultAsync(p => p.Id == l.ProduitId, cancellationToken);
                    if (produit == null)
                        return new Result<int>(false, default, $"Produit {l.ProduitId} introuvable.");

                    // si l.PrixUnitaire vaut 0 (valeur par défaut), on prend le prix du produit en base
                    decimal prix = l.PrixUnitaire != default(decimal) ? l.PrixUnitaire : produit.PrixUnitaire;

                    var ligne = new LigneCommande
                    {
                        ProduitId = l.ProduitId,
                        Quantite = l.Quantite,
                        PrixUnitaire = prix
                    };
                    commande.Lignes.Add(ligne);
                    total += ligne.PrixUnitaire * ligne.Quantite;
                }
                commande.MontantTotal = total;
            }

            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync(cancellationToken);

            return new Result<int>(true, commande.Id);
        }

    }
}
