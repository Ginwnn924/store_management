using System.Net.Http.Json;
using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IPromotionService
{
    Task<List<PromotionResponse>?> GetPromotionsAsync(long minOrderAmount);
}

public class PromotionService : IPromotionService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PromotionService> _logger;

    public PromotionService(HttpClient httpClient, ILogger<PromotionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<PromotionResponse>?> GetPromotionsAsync(long minOrderAmount)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PromotionResponse>>>(
                $"api/Promotion?minOrderAmount={minOrderAmount}");

            if (response?.Data != null)
            {
                _logger.LogInformation("Successfully loaded {Count} promotions", response.Data.Count);
                return response.Data;
            }

            _logger.LogWarning("No promotions found for minOrderAmount: {Amount}", minOrderAmount);
            return new List<PromotionResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading promotions");
            return null;
        }
    }
}

public class PromotionResponse
{
    public int PromoId { get; set; }
    public string PromoCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DiscountType { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
}
