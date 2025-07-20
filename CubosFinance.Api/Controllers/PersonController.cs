using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Common;
using CubosFinance.Application.DTOs.People;
using CubosFinance.Application.Exceptions;
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
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
    {
        try
        {
            dto.Document = Helper.GetOnlyDigits(dto.Document);

            var createdPerson = await _personService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdPerson.Id }, createdPerson);
        }
        catch (DuplicatedDocumentException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult GetById(Guid id)
    {
        return NoContent(); // ou implementar real depois
    }
}
