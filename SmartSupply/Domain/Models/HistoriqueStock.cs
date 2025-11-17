namespace SmartSupply.Domain.Models
{
    public class HistoriqueStock
    {
        public int Id { get; set; }
        public int ProduitId { get; set; }  
        public int EntrepotId { get; set; }
        public int Quantite { get; set; }
        public DateTime DateMouvement { get; set; } = DateTime.UtcNow;
        public string TypeMouvement { get; set; } 
        public string? Commentaire { get; set; }
    }
}
