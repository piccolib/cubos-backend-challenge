namespace CubosFinance.Application.DTOs.Transactions;

public class CreateInternalTransferDto
{
    public Guid ReceiverAccountId { get; set; }
    public decimal Value { get; set; }
    public string Description { get; set; } = string.Empty;
}
