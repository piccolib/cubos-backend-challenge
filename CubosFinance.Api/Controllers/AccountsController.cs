using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Account;
using CubosFinance.Application.DTOs.Common;
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
    [ProducesResponseType(typeof(AccountResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
    {
        var result = await _accountService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AccountResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        var personIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (personIdClaim == null || !Guid.TryParse(personIdClaim.Value, out var personId))
        {
            return Unauthorized(new ErrorResponseDto { Message = "Usuário não autenticado." });
        }

        var result = await _accountService.GetAllByUserAsync(personId);
        return Ok(result);
    }

    [HttpPost("{accountId}/cards")]
    [ProducesResponseType(typeof(CardResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(Guid accountId, [FromBody] CreateCardDto dto)
    {
        var result = await _cardService.CreateAsync(accountId, dto);
        return Ok(result);
    }

    [HttpGet("{accountId}/cards")]
    [ProducesResponseType(typeof(IEnumerable<CardResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCards(Guid accountId)
    {
        var result = await _cardService.GetAllByAccountAsync(accountId);
        return Ok(result);
    }

    [HttpPost("{accountId}/transactions")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTransaction(Guid accountId, [FromBody] CreateTransactionDto dto)
    {
        var result = await _transactionService.CreateAsync(accountId, dto);
        return Ok(result);
    }

    [HttpPost("{accountId}/transactions/internal")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateInternalTransfer(Guid accountId, [FromBody] CreateInternalTransferDto dto)
    {
        var result = await _transactionService.CreateInternalAsync(accountId, dto);
        return Ok(result);
    }

    [HttpGet("{accountId}/transactions")]
    [ProducesResponseType(typeof(PagedResponseDto<TransactionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTransactions(Guid accountId, [FromQuery] TransactionResquestDto dto)
    {
        var result = await _transactionService.GetAllByAccountAsync(accountId, dto);

        return Ok(new
        {
            transactions = result.Data,
            pagination = result.Pagination
        });
    }

    [HttpGet("{accountId}/balance")]
    [ProducesResponseType(typeof(BalanceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBalance(Guid accountId)
    {
        var result = await _transactionService.GetBalanceByAccountAsync(accountId);
        return Ok(result);
    }

    [HttpPost("{accountId}/transactions/{transactionId}/revert")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RevertTransaction(Guid accountId, Guid transactionId)
    {
        var result = await _transactionService.RevertAsync(accountId, transactionId);
        return Ok(result);
    }
}
