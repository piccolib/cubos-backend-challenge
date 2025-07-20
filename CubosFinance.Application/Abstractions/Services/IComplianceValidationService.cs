using CubosFinance.Application.DTOs;

namespace CubosFinance.Application.Abstractions.Services;

public interface IComplianceValidationService
{
    Task<bool> ValidateAsync(DocumentValidationRequestDto dto);
}
