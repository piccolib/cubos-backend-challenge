namespace CubosFinance.Application.Integrations.Complience.Models;

public class TokenResponse
{
    public string IdToken { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
