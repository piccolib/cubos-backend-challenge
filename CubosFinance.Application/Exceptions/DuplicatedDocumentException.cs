
namespace CubosFinance.Application.Exceptions;

public class DuplicatedDocumentException : ApplicationException
{
    public DuplicatedDocumentException(string document)
        : base($"Document '{document}' is already in use.") { }
}
