
namespace CubosFinance.Application.Exceptions;

public class DuplicatedDocumentException : Exception
{
    public DuplicatedDocumentException(string document)
        : base($"Document '{document}' is already in use.") { }
}
