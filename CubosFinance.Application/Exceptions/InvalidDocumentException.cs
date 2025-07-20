
namespace CubosFinance.Application.Exceptions;

public class InvalidDocumentException : ApplicationException
{
    public InvalidDocumentException(string document)
        : base($"Document '{document}' is not valid.") { }
}
