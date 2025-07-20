using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.People;
using CubosFinance.Application.Integrations.Complience.Models;

namespace CubosFinance.Application.Integrations.Complience.Services
{
    public class ComplianceValidationService : IComplianceValidationService
    {
        private readonly IComplianceApi _api;
        private readonly IComplianceAuthService _authService;

        private const int CpfLength = 11;
        private const int CnpjLength = 14;
        private const int ValidStatusCode = 1;

        public ComplianceValidationService(IComplianceApi api, IComplianceAuthService authService)
        {
            _api = api;
            _authService = authService;
        }

        public async Task<bool> ValidateAsync(DocumentValidationRequestDto dto)
        {
            var cleanDocument = new string(dto.Document.Where(char.IsDigit).ToArray());
            var request = new DocumentValidationRequest { Document = cleanDocument };
            var token = await _authService.GetAccessTokenAsync();
            var bearer = $"Bearer {token}";

            var result = cleanDocument.Length switch
            {
                CpfLength => await _api.ValidateCpfAsync(request, bearer),
                CnpjLength => await _api.ValidateCnpjAsync(request, bearer),
                _ => throw new ArgumentException("Documento inválido. Deve conter 11 (CPF) ou 14 (CNPJ) dígitos.")
            };

            if (!result.Success)
                throw new ApplicationException("Erro na validação do documento: " + result.ToString());

            var response = result.Data!;

            return response.Status == ValidStatusCode;
        }
    }
}
