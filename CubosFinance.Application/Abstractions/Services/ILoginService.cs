using CubosFinance.Application.DTOs.Login;

namespace CubosFinance.Application.Abstractions.Services;

public interface ILoginService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
}