using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Account;
using CubosFinance.Application.DTOs.Transactions;
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
    private readonly IAccountService _accountService;
    private readonly ICardService _cardService;
    private readonly ITransactionService _transactionService;

    public AccountsController(IAccountService service, ICardService cardService, ITransactionService transactionService)
    {
        _accountService = service;
        _cardService = cardService;
        _transactionService = transactionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
    {
        try
        {
            var result = await _accountService.CreateAsync(dto);
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
            var personIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (personIdClaim == null || !Guid.TryParse(personIdClaim.Value, out var personId))
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var result = await _accountService.GetAllByUserAsync(personId);
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

    [HttpPost("{accountId}/cards")]
    public async Task<IActionResult> Create(Guid accountId, [FromBody] CreateCardDto dto)
    {
        try
        {
            var result = await _cardService.CreateAsync(accountId, dto);
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

    [HttpGet("{accountId}/cards")]
    public async Task<IActionResult> GetCards(Guid accountId)
    {
        try
        {
            var result = await _cardService.GetAllByAccountAsync(accountId);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPost("{accountId}/transactions")]
    public async Task<IActionResult> CreateTransaction(Guid accountId, [FromBody] CreateTransactionDto dto)
    {
        try
        {
            var result = await _transactionService.CreateAsync(accountId, dto);
            return Ok(result);
        }
        catch (InsufficientBalanceException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (AccountNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPost("{accountId}/transactions/internal")]
    public async Task<IActionResult> CreateInternalTransfer(Guid accountId, [FromBody] CreateInternalTransferDto dto)
    {
        try
        {
            var result = await _transactionService.CreateInternalAsync(accountId, dto);
            return Ok(result);
        }
        catch (InsufficientBalanceException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (AccountNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpGet("{accountId}/transactions")]
    public async Task<IActionResult> GetTransactions(Guid accountId, [FromQuery] TransactionResquestDto dto)
    {
        try
        {
            var result = await _transactionService.GetAllByAccountAsync(accountId, dto);

            return Ok(new
            {
                transactions = result.Data,
                pagination = result.Pagination
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpGet("{accountId}/balance")]
    public async Task<IActionResult> GetBalance(Guid accountId)
    {
        try
        {
            var result = await _transactionService.GetBalanceByAccountAsync(accountId);
            return Ok(result);
        }
        catch (AccountNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
}