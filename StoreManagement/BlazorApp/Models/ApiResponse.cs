namespace BlazorApp.Models;

/// <summary>
/// Generic API response wrapper matching backend Response<T>
/// </summary>
public class ApiResponse<T> where T : class
{
    public string? Message { get; set; }
    public T? Data { get; set; }
}

