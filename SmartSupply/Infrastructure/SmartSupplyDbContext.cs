using Microsoft.EntityFrameworkCore;
using SmartSupply.Domain.Models;

namespace SmartSupply.Infrastructure
{
    public class SmartSupplyDbContext : DbContext
    {
        public SmartSupplyDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Produit> Produits => Set<Produit>();
        public DbSet<Stock> Stocks => Set<Stock>();
        public DbSet<LigneCommande> LignesCommande => Set<LigneCommande>();
        public DbSet<Commande> Commandes => Set<Commande>();
        public DbSet<Entrepot> Entrepots => Set<Entrepot>();
        public DbSet<EventMetier> Events => Set<EventMetier>();
        public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produit>().HasIndex(p => p.CodeSKU).IsUnique();
            modelBuilder.Entity<Produit>().Property(p => p.PrixUnitaire).HasPrecision(18, 2);
            modelBuilder.Entity<LigneCommande>().Property(l => l.PrixUnitaire).HasPrecision(18, 2);
            modelBuilder.Entity<Commande>().Property(c => c.MontantTotal).HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<SmartSupply.Domain.Models.HistoriqueStock> HistoriqueStock { get; set; } = default!;


    }
}
