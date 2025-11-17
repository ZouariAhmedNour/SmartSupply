namespace SmartSupply.Domain.Models
{
    public class Entrepot
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public int CapaciteMax { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    }
}
