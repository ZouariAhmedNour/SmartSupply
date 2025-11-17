namespace SmartSupply.Domain.Models
{
    public class Produit
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string CodeSKU { get; set; }
        public decimal PrixUnitaire { get; set; }
        public DateTime DateCreation { get; set; }



    }
}
