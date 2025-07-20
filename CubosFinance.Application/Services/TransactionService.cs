using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Transactions;
using CubosFinance.Application.Exceptions;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;

namespace CubosFinance.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task<TransactionResponseDto> CreateAsync(Guid accountId, CreateTransactionDto dto)
    {
        var accountExists = await _accountRepository.ExistsAsync(accountId);
        if (!accountExists)
        {
            throw new AccountNotFoundException();
        }

        if (dto.Value < 0)
        {
            var currentBalance = await _transactionRepository.GetCurrentBalanceAsync(accountId);
            if (currentBalance + dto.Value < 0)
            {
                throw new InsufficientBalanceException();
            }
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            Value = dto.Value,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _transactionRepository.CreateAsync(transaction);

        return new TransactionResponseDto
        {
            Id = transaction.Id,
            Value = transaction.Value,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt,
            UpdatedAt = transaction.UpdatedAt
        };
    }
}

