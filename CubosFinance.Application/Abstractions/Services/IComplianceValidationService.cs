using CubosFinance.Application.DTOs.People;

namespace CubosFinance.Application.Abstractions.Services;

public interface IComplianceValidationService
{
    Task<bool> ValidateAsync(DocumentValidationRequestDto dto);
}
