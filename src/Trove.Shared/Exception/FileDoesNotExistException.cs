using System.Net;

namespace Trove.Shared.Exception;

public class FileDoesNotExistException : EntryBaseException
{
    public FileDoesNotExistException(Guid key) : base(key, "A file does not exist with that id", HttpStatusCode.NotFound)
    {
    }

    public FileDoesNotExistException(Guid key, string message) : base(key, message, HttpStatusCode.NotFound)
    {
    }

    public FileDoesNotExistException(string message) : base(HttpStatusCode.NotFound, message)
    {
    }

    public FileDoesNotExistException() : base(HttpStatusCode.NotFound, "A file does not exist with that id")
    {
    }
}