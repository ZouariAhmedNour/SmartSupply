using MediatR;
using SmartSupply.Domain.Models;

public record RegisterUtilisateurCommand(
    string Nom,
    string Prenom,
    string Email,
    string Password,
    string ConfirmPassword,
    string Role
) : IRequest<Utilisateur?>;
