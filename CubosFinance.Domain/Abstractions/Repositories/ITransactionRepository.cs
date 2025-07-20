using CubosFinance.Domain.Entities;

namespace CubosFinance.Domain.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<decimal> GetCurrentBalanceAsync(Guid accountId);
}