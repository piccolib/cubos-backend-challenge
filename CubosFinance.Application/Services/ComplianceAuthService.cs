using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Integrations.Complience;
using CubosFinance.Application.Integrations.Complience.Models;
using CubosFinance.Application.Integrations.Complience.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace CubosFinance.Application.Services
{
    public class ComplianceAuthService : IComplianceAuthService
    {
        private readonly IComplianceApi _api;
        private readonly ComplianceAuthSettings _settings;
        private TokenResponse? _token;

        public ComplianceAuthService(IComplianceApi api, IOptions<ComplianceAuthSettings> options)
        {
            _api = api;
            _settings = options.Value;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (_token == null)
            {
                var code = await _api.GetAuthCodeAsync(new AuthCodeRequest
                {
                    Email = _settings.Email,
                    Password = _settings.Password
                });

                if (!code.Success || code.Data is null)
                    throw new ApplicationException($"Erro ao obter código: {code}");

                var tokenResponse = await _api.GetTokenAsync(new TokenRequest
                {
                    AuthCode = code.Data.AuthCode
                });

                if (!tokenResponse.Success || tokenResponse.Data is null)
                    throw new ApplicationException($"Erro ao obter token: {tokenResponse}");

                _token = tokenResponse.Data;
            }

            return _token.AccessToken;
        }
    }
}
