
namespace CubosFinance.Application.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base("Incorrect password.") { }
}
