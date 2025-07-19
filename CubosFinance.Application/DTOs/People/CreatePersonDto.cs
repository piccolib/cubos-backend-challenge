namespace CubosFinance.Application.DTOs.People;

public class CreatePersonDto
{
    public string Name { get; set; } = string.Empty;

    public string Document { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
