namespace CubosFinance.Application.Exceptions;

public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException() : base("Insufficient balance for this operation.") { }
}

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(string message) : base(message) { }
}
