namespace CubosFinance.Application.Integrations.Complience.Models;

public class DocumentValidationResponse
{
    public string Document { get; set; } = null!;
    public int Status { get; set; }
    public string Reason { get; set; } = null!;
}
