namespace SmartSupply.Domain.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MdpHashed { get; set; }
        public string Role { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    }
}
