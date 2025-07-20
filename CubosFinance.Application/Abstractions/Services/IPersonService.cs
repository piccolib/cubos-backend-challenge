using CubosFinance.Application.DTOs.People;

namespace CubosFinance.Application.Abstractions.Services;

public interface IPersonService
{
    Task<PersonResponseDto> CreateAsync(CreatePersonDto dto);
}