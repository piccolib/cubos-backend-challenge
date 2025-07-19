
using CubosFinance.Application.DTOs.People;

namespace CubosFinance.Application.Interfaces;

public interface IPersonService
{
    Task<PersonResponseDto> CreateAsync(CreatePersonDto dto);
}