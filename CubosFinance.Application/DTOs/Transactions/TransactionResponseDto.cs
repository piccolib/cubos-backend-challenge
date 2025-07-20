namespace CubosFinance.Application.DTOs.Transactions;
public class TransactionResponseDto
{
    public Guid Id { get; set; }
    public decimal Value { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
