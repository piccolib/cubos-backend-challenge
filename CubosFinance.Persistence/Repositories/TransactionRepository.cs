using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using CubosFinance.Domain.Enums;
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

    public async Task<List<Transaction>> GetByAccountIdAsync(Guid accountId, int currentPage, int itemsPerPage, TransactionType? type)
    {
        var query = _context.Transactions
            .Where(t => t.AccountId == accountId);

        if (type != null)
        {
            if (type == TransactionType.credit)
                query = query.Where(t => t.Value > 0);
            else if (type == TransactionType.debit)
                query = query.Where(t => t.Value < 0);
        }

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((currentPage - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid transactionId)
    {
        return await _context.Transactions.FindAsync(transactionId);
    }

    public async Task<bool> ExistsReversalFor(Guid originalTransactionId)
    {
        return await _context.Transactions
            .AnyAsync(t => t.ReversedFromTransactionId == originalTransactionId);
    }
}
