namespace CubosFinance.Domain.Entities;

public class Account : BaseEntity
{
    public string Branch { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;
    public Guid PersonId { get; private set; }
    public Person Person { get; private set; }

    public Account(string branch, string number, Guid personId)
    {
        Branch = branch;
        Number = number;
        PersonId = personId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTimestamps()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
