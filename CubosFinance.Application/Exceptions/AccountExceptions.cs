
namespace CubosFinance.Application.Exceptions;

public class DuplicatedAccountNumberException : Exception
{
    public DuplicatedAccountNumberException()
        : base("Duplicated account number") { }
}

public class DuplicatedDocumentException : Exception
{
    public DuplicatedDocumentException(string document)
        : base($"Document '{document}' is already in use.") { }
}

public class InvalidAccountFormatException : Exception
{
    public InvalidAccountFormatException()
        : base("Invalid account format. Must follow this format XXXXXXX-X") { }
}

public class InvalidBranchFormatException : Exception
{
    public InvalidBranchFormatException()
        : base("Invalid Branch. Must contain 3 digits") { }
}

