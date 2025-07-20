namespace CubosFinance.Application.Integrations.Complience.Models;

public class AuthCodeResponse
{
    public string UserId { get; set; } = string.Empty;
    public string AuthCode { get; set; } = string.Empty;
}