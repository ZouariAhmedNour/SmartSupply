// fichier : SmartSupply/Helpers/PasswordHelper.cs
using Microsoft.AspNetCore.Identity;
using SmartSupply.Domain.Models;


namespace SmartSupply.Helpers
{
    public static class PasswordHelper
    {
        // PasswordHasher est safe à réutiliser (stateless pour notre usage)
        private static readonly PasswordHasher<Utilisateur> _hasher = new PasswordHasher<Utilisateur>();

        // Hash le mot de passe
        public static string HashPassword(string plainPassword)
        {
            // on peut passer un Utilisateur "vide" parce que nous n'utilisons pas de properties pour le hash
            return _hasher.HashPassword(new Utilisateur(), plainPassword);
        }

        // Vérifie le mot de passe (true si correspond)
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(new Utilisateur(), hashedPassword, plainPassword);
            return result != PasswordVerificationResult.Failed;
        }
    }
}
