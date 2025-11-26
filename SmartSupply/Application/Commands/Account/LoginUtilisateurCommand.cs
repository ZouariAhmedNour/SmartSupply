using MediatR;
using SmartSupply.Domain.Models;

public record LoginUtilisateurCommand(string Email, string Password)
    : IRequest<Utilisateur?>;
