namespace CubosFinance.Application.DTOs.Transactions;

public class CreateTransactionDto
{
    public decimal Value { get; set; }
    public string Description { get; set; } = string.Empty;
}

