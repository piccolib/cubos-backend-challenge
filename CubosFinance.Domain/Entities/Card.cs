using CubosFinance.Domain.Entities;
using CubosFinance.Domain.Enums;

public class Card : BaseEntity
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public CardType Type { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
}
