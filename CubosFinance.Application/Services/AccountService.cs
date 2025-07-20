using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Account;
using CubosFinance.Application.Exceptions;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CubosFinance.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(IAccountRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AccountResponseDto> CreateAsync(CreateAccountDto dto)
    {
        if (!Regex.IsMatch(dto.Branch, @"^\d{3}$"))
            throw new InvalidBranchFormatException();

        if (!Regex.IsMatch(dto.Account, @"^\d{7}-\d{1}$"))
            throw new InvalidAccountFormatException();

        if (await _repository.ExistsByNumberAsync(dto.Account))
            throw new DuplicatedAccountNumberException();

        var personIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (personIdClaim == null || !Guid.TryParse(personIdClaim.Value, out var personId))
            throw new UnauthorizedAccessException("User not authenticated.");

        var account = new Account(dto.Branch, dto.Account, personId);
        await _repository.AddAsync(account);

        return new AccountResponseDto
        {
            Id = account.Id,
            Branch = account.Branch,
            Account = account.Number,
        };
    }

    public async Task<IEnumerable<AccountResponseDto>> GetAllByUserAsync(Guid personId)
    {
        var accounts = await _repository.GetByPersonIdAsync(personId);

        return accounts.Select(account => new AccountResponseDto
        {
            Id = account.Id,
            Branch = account.Branch,
            Account = account.Number,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        });
    }
}
