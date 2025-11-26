using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Commandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;


namespace SmartSupply.Application.Handlers.Commandes
{
    public record CreateCommandeHandler : IRequestHandler<CreateCommandeCommand, int?>
    {
        private readonly SmartSupplyDbContext _context;

        public CreateCommandeHandler(SmartSupplyDbContext context)
        {
            _context = context;
        }

        public async Task<int?> Handle(CreateCommandeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ClientNom) || string.IsNullOrWhiteSpace(request.ClientEmail))
                return null; // Nom et email requis

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
                    var produit = await _context.Produits.FirstOrDefaultAsync(p => p.Id == l.ProduitId, cancellationToken);
                    if (produit == null)
                        return null; // Produit introuvable

                    decimal prix = l.PrixUnitaire != default ? l.PrixUnitaire : produit.PrixUnitaire;

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

            return commande.Id;
        }
    }
}
