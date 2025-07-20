using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Common;
using CubosFinance.Application.DTOs.Login;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

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
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            dto.Document = Helper.GetOnlyDigits(dto.Document);

            var response = await _loginService.LoginAsync(dto);

            return Ok(new LoginResponseDto { Token = response.Token });
        }
        catch (InvalidCredentialException)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}
