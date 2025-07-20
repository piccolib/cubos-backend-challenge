using CubosFinance.Domain.Entities;
using CubosFinance.Domain.Enums;

namespace CubosFinance.Domain.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<decimal> GetCurrentBalanceAsync(Guid accountId);
    Task<List<Transaction>> GetByAccountIdAsync(Guid accountId, int currentPage, int itemsPerPage, TransactionType? type);
}