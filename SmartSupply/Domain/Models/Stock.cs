namespace SmartSupply.Domain.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProduitId { get; set; }
        public int EntrepotId { get; set; }
        public int QuantiteDisponible { get; set; }
        public int SeuilAlerte { get; set; } = 0;
        public Entrepot? Entrepot { get; set; }
        public Produit? Produit { get; set; }


    }
}
