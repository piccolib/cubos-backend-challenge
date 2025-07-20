using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using CubosFinance.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CubosFinance.Persistence.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly CubosFinanceDbContext _context;
    public PersonRepository(CubosFinanceDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Person person)
    {
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByDocumentAsync(string document)
    {
        return await _context.People.AnyAsync(p => p.Document == document);
    }

    public async Task<Person?> GetByDocumentAsync(string document)
    {
        return await _context.People
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Document == document);
    }
}

