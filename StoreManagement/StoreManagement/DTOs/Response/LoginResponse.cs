namespace StoreManagement.DTOs.Response;

public class LoginResponse
{   public int UserId { get; set; } = 1;
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
