namespace CubosFinance.Application.Integrations.Complience.Models;

public class ComplianceApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
}