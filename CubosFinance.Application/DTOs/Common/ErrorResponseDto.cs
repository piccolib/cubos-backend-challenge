namespace CubosFinance.Application.DTOs.Common;

public class ErrorResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string? Detail { get; set; }
}
