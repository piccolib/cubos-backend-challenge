using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Account;
using CubosFinance.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CubosFinance.Api.Controllers;

[ApiController]
[Route("accounts")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountsController(IAccountService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
        catch (InvalidBranchFormatException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidAccountFormatException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DuplicatedAccountNumberException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var personId = User.FindFirstValue("id");
            var result = await _service.GetAllByUserAsync(Guid.Parse(personId));
            return Ok(result);
        }
        catch (FormatException)
        {
            return BadRequest(new { message = "Invalid user identifier format." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
}
