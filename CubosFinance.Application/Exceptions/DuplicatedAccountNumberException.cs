
namespace CubosFinance.Application.Exceptions;

public class DuplicatedAccountNumberException : Exception
{
    public DuplicatedAccountNumberException()
        : base("Duplicated account number.") { }
}
