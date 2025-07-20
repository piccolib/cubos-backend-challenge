
namespace CubosFinance.Application.Exceptions;

public class InvalidDocumentException : Exception
{
    public InvalidDocumentException(string document)
        : base($"Document '{document}' is not valid.") { }
}
