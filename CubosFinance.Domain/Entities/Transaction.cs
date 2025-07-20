namespace CubosFinance.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public decimal Value { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Account Account { get; set; } = null!;

    public static Transaction CreateDebit(Guid accountId, decimal value, string description)
    {
        return new Transaction
        {
            AccountId = accountId,
            Value = -Math.Abs(value),
            Description = description,
        };
    }

    public static Transaction CreateCredit(Guid accountId, decimal value, string description)
    {
        return new Transaction
        {
            AccountId = accountId,
            Value = Math.Abs(value),
            Description = description,
        };
    }
}
