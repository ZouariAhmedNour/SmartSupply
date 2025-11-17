namespace SmartSupply.Domain.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public string ClientNom { get; set; } = null!;
        public string ClientEmail { get; set; } = null!;
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public string Statut { get; set; } = "EnAttente";
        public decimal MontantTotal { get; set; }
        public List<LigneCommande> Lignes { get; set; } = new();

    }
}
