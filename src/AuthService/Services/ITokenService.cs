using System.Security.Claims;

namespace AuthService.Services;

/// <summary>
/// Interface pour la génération de tokens JWT
/// </summary>
public interface ITokenService
{
    string GenerateToken(string userId, string email, IEnumerable<string> roles);
    ClaimsPrincipal? ValidateToken(string token);
}

