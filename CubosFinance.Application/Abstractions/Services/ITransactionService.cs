using CubosFinance.Application.DTOs.Common;
using CubosFinance.Application.DTOs.Transactions;

namespace CubosFinance.Application.Abstractions.Services;

public interface ITransactionService
{
    Task<TransactionResponseDto> CreateAsync(Guid accountId, CreateTransactionDto dto);
    Task<TransactionResponseDto> CreateInternalAsync(Guid senderAccountId, CreateInternalTransferDto dto);
    Task<PagedResponseDto<TransactionResponseDto>> GetAllByAccountAsync(Guid accountId, TransactionResquestDto requestDto);
    Task<BalanceResponseDto> GetBalanceByAccountAsync(Guid accountId);
    Task<TransactionResponseDto> RevertAsync(Guid accountId, Guid transactionId);
}