namespace AuthService.Models;

/// <summary>
/// Modèle de requête pour la connexion
/// </summary>
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

