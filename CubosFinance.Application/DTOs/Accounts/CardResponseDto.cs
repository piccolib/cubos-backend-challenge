using CubosFinance.Domain.Enums;

public class CardResponseDto
{
    public Guid Id { get; set; }
    public CardType Type { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
