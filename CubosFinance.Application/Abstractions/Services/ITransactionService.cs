using CubosFinance.Application.DTOs.Transactions;

namespace CubosFinance.Application.Abstractions.Services;

public interface ITransactionService
{
    Task<TransactionResponseDto> CreateAsync(Guid accountId, CreateTransactionDto dto);
}