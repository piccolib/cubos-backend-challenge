using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using CubosFinance.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CubosFinance.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly CubosFinanceDbContext _context;

    public AccountRepository(CubosFinanceDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByNumberAsync(string account)
    {
        return await _context.Accounts.AnyAsync(a => a.Number == account);
    }

    public async Task<IEnumerable<Account>> GetByPersonIdAsync(Guid personId)
    {
        return await _context.Accounts
            .Where(a => a.PersonId == personId)
            .ToListAsync();
    }
}

