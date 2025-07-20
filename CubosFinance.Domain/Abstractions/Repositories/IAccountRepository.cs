using CubosFinance.Domain.Entities;

namespace CubosFinance.Domain.Abstractions.Repositories;

public interface IAccountRepository
{
    Task<bool> ExistsByNumberAsync(string account);
    Task AddAsync(Account account);
    Task<IEnumerable<Account>> GetByPersonIdAsync(Guid personId);
    Task<bool> ExistsAsync(Guid accountId);
}

