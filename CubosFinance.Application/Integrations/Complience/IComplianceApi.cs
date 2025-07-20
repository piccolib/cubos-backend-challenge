using CubosFinance.Application.Integrations.Complience.Models;
using Refit;

namespace CubosFinance.Application.Integrations.Complience;

public interface IComplianceApi
{
    [Post("/auth/code")]
    Task<ComplianceApiResponse<AuthCodeResponse>> GetAuthCodeAsync([Body] AuthCodeRequest request);

    [Post("/auth/token")]
    Task<ComplianceApiResponse<TokenResponse>> GetTokenAsync([Body] TokenRequest request);

    [Post("/auth/refresh")]
    Task<ComplianceApiResponse<TokenResponse>> RefreshTokenAsync([Body] RefreshTokenRequest request);

    [Post("/cpf/validate")]
    Task<ComplianceApiResponse<DocumentValidationResponse>> ValidateCpfAsync(
        [Body] DocumentValidationRequest request,
        [Header("Authorization")] string authorization);

    [Post("/cnpj/validate")]
    Task<ComplianceApiResponse<DocumentValidationResponse>> ValidateCnpjAsync(
        [Body] DocumentValidationRequest request,
        [Header("Authorization")] string authorization);
}