namespace CubosFinance.Application.Exceptions;

public class PhysicalCardAlreadyExistsException : Exception
{
    public PhysicalCardAlreadyExistsException()
        : base("A physical card already exists for this account.") { }
}

public class InvalidCvvException : Exception
{
    public InvalidCvvException()
        : base("CVV must be exactly 3 digits.") { }
}
