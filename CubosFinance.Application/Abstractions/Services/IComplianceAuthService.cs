namespace CubosFinance.Application.Abstractions.Services;

public interface IComplianceAuthService
{
    Task<string> GetAccessTokenAsync();
}
