
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using CubosFinance.Domain.Enums;
using CubosFinance.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace CubosFinance.Persistence.Repositories;

public class CardRepository : ICardRepository
{
    private readonly CubosFinanceDbContext _context;

    public CardRepository(CubosFinanceDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Card card)
    {
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByDocumentAsync(string document)
    {
        return await _context.People.AnyAsync(p => p.Document == document);
    }

    public async Task<bool> PhysicalCardExistsAsync(Guid accountId)
    {
        return await _context.Cards.AnyAsync(c => c.AccountId == accountId && c.Type == CardType.Physical);
    }

    public async Task<IEnumerable<Card>> GetAllByAccountIdAsync(Guid accountId)
    {
        return await _context.Cards
            .Where(c => c.AccountId == accountId)
            .ToListAsync();
    }
}