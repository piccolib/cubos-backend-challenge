using CubosFinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CubosFinance.Persistence.Context;

public class CubosFinanceDbContext : DbContext
{
    public CubosFinanceDbContext(DbContextOptions<CubosFinanceDbContext> options) : base(options) { }

    public DbSet<Person> People { get; set; }

}
