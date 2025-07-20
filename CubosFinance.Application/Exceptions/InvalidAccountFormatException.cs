
namespace CubosFinance.Application.Exceptions;

public class InvalidAccountFormatException : Exception
{
    public InvalidAccountFormatException()
        : base("Invalid account format. Must follow this format XXXXXXX-X") { }
}
