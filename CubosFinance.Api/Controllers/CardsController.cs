using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("cards")]
public class AllCardsController : ControllerBase
{
    private readonly ICardService _cardService;

    public AllCardsController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int currentPage = 1,
        [FromQuery] int itemsPerPage = 10)
    {
        var personIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (personIdClaim == null || !Guid.TryParse(personIdClaim.Value, out var personId))
        {
            return Unauthorized(new { message = "Usuário não autenticado." });
        }

        var result = await _cardService.GetAllByPersonAsync(personId, currentPage, itemsPerPage);

        return Ok(new
        {
            cards = result.Data,
            pagination = result.Pagination
        });
    }
}