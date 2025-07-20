using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Common;
using CubosFinance.Application.DTOs.Login;
using CubosFinance.Application.DTOs.Common;
using CubosFinance.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CubosFinance.API.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        dto.Document = Helper.GetOnlyDigits(dto.Document);

        var response = await _loginService.LoginAsync(dto);

        return Ok(new LoginResponseDto { Token = response.Token });
    }
}
