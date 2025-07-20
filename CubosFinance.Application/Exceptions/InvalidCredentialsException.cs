
namespace CubosFinance.Application.Exceptions;

public class InvalidCredentialsException : ApplicationException
{
    public InvalidCredentialsException()
        : base("Incorrect password.") { }
}
