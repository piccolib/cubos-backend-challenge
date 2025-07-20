using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using CubosFinance.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CubosFinance.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly CubosFinanceDbContext _context;

    public TransactionRepository(CubosFinanceDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<decimal> GetCurrentBalanceAsync(Guid accountId)
    {
        return await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .SumAsync(t => t.Value);
    }
}
