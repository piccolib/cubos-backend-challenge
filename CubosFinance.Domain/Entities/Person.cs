
namespace CubosFinance.Domain.Entities;

public class Person : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Document { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
}
