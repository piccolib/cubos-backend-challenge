using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Exceptions;
using CubosFinance.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("accounts/{accountId}/cards")]
public class CardsController : ControllerBase
{
    private readonly ICardService _service;

    public CardsController(ICardService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid accountId, [FromBody] CreateCardDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(accountId, dto);
            return Ok(result);
        }
        catch (InvalidCvvException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (PhysicalCardAlreadyExistsException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCards(Guid accountId)
    {
        try
        {
            var result = await _service.GetAllByAccountAsync(accountId);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
}
