using CubosFinance.Domain.Entities;

namespace CubosFinance.Domain.Abstractions.Repositories;

public interface IPersonRepository
{
    Task AddAsync(Person person);
    Task<bool> ExistsByDocumentAsync(string document);
}
