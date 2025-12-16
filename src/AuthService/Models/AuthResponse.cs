namespace AuthService.Models;

/// <summary>
/// Réponse d'authentification avec le token JWT
/// </summary>
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

