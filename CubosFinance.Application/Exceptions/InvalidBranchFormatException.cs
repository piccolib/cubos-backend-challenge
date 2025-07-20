
namespace CubosFinance.Application.Exceptions;

public class InvalidBranchFormatException : Exception
{
    public InvalidBranchFormatException()
        : base("Invalid Branch. Must contain 3 digits") { }
}
