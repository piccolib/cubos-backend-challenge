using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Common;
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
            throw new AccountNotFoundException("Account not found.");
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
            AccountId = accountId,
            Value = dto.Value,
            Description = dto.Description,
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

    public async Task<TransactionResponseDto> CreateInternalAsync(Guid senderAccountId, CreateInternalTransferDto dto)
    {
        if (senderAccountId == dto.ReceiverAccountId)
            throw new InvalidOperationException("Cannot transfer to the same account.");

        var senderExists = await _accountRepository.ExistsAsync(senderAccountId);
        var receiverExists = await _accountRepository.ExistsAsync(dto.ReceiverAccountId);

        if (!senderExists || !receiverExists)
            throw new AccountNotFoundException("One or both accounts not found.");

        var senderBalance = await _transactionRepository.GetCurrentBalanceAsync(senderAccountId);
        if (senderBalance < dto.Value)
            throw new InsufficientBalanceException();

        var debitTransaction = Transaction.CreateDebit(senderAccountId, dto.Value, dto.Description);

        var creditTransaction = Transaction.CreateCredit(dto.ReceiverAccountId, dto.Value, dto.Description);

        await _transactionRepository.CreateAsync(debitTransaction);
        await _transactionRepository.CreateAsync(creditTransaction);

        return new TransactionResponseDto
        {
            Id = debitTransaction.Id,
            Value = debitTransaction.Value,
            Description = debitTransaction.Description,
            CreatedAt = debitTransaction.CreatedAt,
            UpdatedAt = debitTransaction.UpdatedAt
        };
    }

    public async Task<PagedResponseDto<TransactionResponseDto>> GetAllByAccountAsync(Guid accountId, TransactionResquestDto requestDto)
    {
        var transactions = await _transactionRepository
            .GetByAccountIdAsync(accountId, requestDto.CurrentPage, requestDto.ItemsPerPage, requestDto.Type);

        var dtos = transactions.Select(t => new TransactionResponseDto
        {
            Id = t.Id,
            Value = t.Value,
            Description = t.Description,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();

        return new PagedResponseDto<TransactionResponseDto>
        {
            Data = dtos,
            Pagination = new PaginationDto
            {
                ItemsPerPage = requestDto.ItemsPerPage,
                CurrentPage = requestDto.CurrentPage,
            }
        };
    }

    public async Task<BalanceResponseDto> GetBalanceByAccountAsync(Guid accountId)
    {
        var accountExists = await _accountRepository.ExistsAsync(accountId);
        if (!accountExists)
            throw new AccountNotFoundException("Account not found.");

        var balance = await _transactionRepository.GetCurrentBalanceAsync(accountId);

        return new BalanceResponseDto
        {
            Balance = balance
        };
    }
}

