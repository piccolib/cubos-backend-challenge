using CubosFinance.Domain.Enums;

public class CreateCardDto
{
    public CardType Type { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
}
