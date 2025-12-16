using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

/// <summary>
/// Controller pour l'authentification et l'autorisation
/// Endpoints pour login, register, et gestion des tokens JWT
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ITokenService tokenService,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// POST /api/auth/register
    /// Inscription d'un nouvel utilisateur
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return BadRequest(new { message = "Les mots de passe ne correspondent pas" });
        }

        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Échec de l'inscription: {Errors}", errors);
            return BadRequest(new { message = errors });
        }

        // Attribution du rôle par défaut
        await _userManager.AddToRoleAsync(user, "User");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);

        _logger.LogInformation("Utilisateur inscrit avec succès: {Email}", request.Email);

        return Ok(new AuthResponse
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1),
            UserId = user.Id,
            Email = user.Email!
        });
    }

    /// <summary>
    /// POST /api/auth/login
    /// Connexion d'un utilisateur existant
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            _logger.LogWarning("Tentative de connexion avec un email inexistant: {Email}", request.Email);
            return Unauthorized(new { message = "Email ou mot de passe incorrect" });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Échec de la connexion pour: {Email}", request.Email);
            return Unauthorized(new { message = "Email ou mot de passe incorrect" });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);

        _logger.LogInformation("Connexion réussie: {Email}", request.Email);

        return Ok(new AuthResponse
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1),
            UserId = user.Id,
            Email = user.Email!
        });
    }

    /// <summary>
    /// POST /api/auth/validate
    /// Validation d'un token JWT
    /// </summary>
    [HttpPost("validate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult ValidateToken([FromBody] string token)
    {
        var principal = _tokenService.ValidateToken(token);

        if (principal == null)
        {
            return Unauthorized(new { message = "Token invalide" });
        }

        return Ok(new { valid = true, claims = principal.Claims.Select(c => new { c.Type, c.Value }) });
    }
}

