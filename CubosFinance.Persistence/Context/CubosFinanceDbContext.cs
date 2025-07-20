using CubosFinance.Domain.Entities;
using CubosFinance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CubosFinance.Persistence.Context;

public class CubosFinanceDbContext : DbContext
{
    public CubosFinanceDbContext(DbContextOptions<CubosFinanceDbContext> options) : base(options) { }

    public DbSet<Person> People { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Card>()
            .Property(c => c.Type)
            .HasConversion(new EnumToStringConverter<CardType>());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = now;

            entry.Entity.UpdatedAt = now;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
