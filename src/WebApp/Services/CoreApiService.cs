using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CoreAPI.Application.DTOs;
using WebApp.Services;

namespace WebApp.Services;

/// <summary>
/// Service pour consommer la Core API via HttpClient
/// Implémentation de la consommation d'API depuis une application MVC
/// </summary>
public class CoreApiService : ICoreApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CoreApiService> _logger;

    public CoreApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CoreApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;

        var apiBaseUrl = _configuration["CoreApi:BaseUrl"] ?? "https://localhost:7001";
        _httpClient.BaseAddress = new Uri(apiBaseUrl);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private void AddAuthHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Enumerable.Empty<ProductDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des produits");
            throw;
        }
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"/api/products/{id}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération du produit {ProductId}", id);
            throw;
        }
    }

    public async Task<Guid> CreateProductAsync(CreateProductRequest request)
    {
        try
        {
            AddAuthHeader();
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/products", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Guid>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création du produit");
            throw;
        }
    }

    public async Task UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        try
        {
            AddAuthHeader();
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/products/{id}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour du produit {ProductId}", id);
            throw;
        }
    }

    public async Task DeleteProductAsync(Guid id)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.DeleteAsync($"/api/products/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la suppression du produit {ProductId}", id);
            throw;
        }
    }
}

