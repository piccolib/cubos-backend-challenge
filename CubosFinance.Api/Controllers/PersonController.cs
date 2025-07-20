using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Common;
using CubosFinance.Application.DTOs.People;
using CubosFinance.Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace CubosFinance.Api.Controllers;

[ApiController]
[Route("people")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
    {
        dto.Document = Helper.GetOnlyDigits(dto.Document);

        var result = await _personService.CreateAsync(dto);

        return Ok(result);
    }
}
