namespace SmartSupply.Domain.Models
{
    public class EventMetier
    {
        public int Id { get; set; }
        public string TypeEvent { get; set; } = null!;
        public string Donnees { get; set; } = null!;
        public DateTime DateEvent { get; set; } = DateTime.UtcNow;
    }
}
