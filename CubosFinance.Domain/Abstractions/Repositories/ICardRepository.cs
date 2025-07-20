namespace CubosFinance.Domain.Abstractions.Repositories;

public interface ICardRepository
{
    Task AddAsync(Card card);
    Task<bool> PhysicalCardExistsAsync(Guid accountId);
    Task<IEnumerable<Card>> GetAllByAccountIdAsync(Guid accountId);
}

