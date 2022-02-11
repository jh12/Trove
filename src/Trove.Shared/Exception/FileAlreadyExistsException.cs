using System.Net;

namespace Trove.Shared.Exception;

public class FileAlreadyExistsException : EntryBaseException
{
    public FileAlreadyExistsException(Guid key) : base(key, "A file already exists with that id", HttpStatusCode.Conflict)
    {
    }

    public FileAlreadyExistsException(Guid key, string message) : base(key, message, HttpStatusCode.Conflict)
    {
    }

    public FileAlreadyExistsException(string message) : base(HttpStatusCode.Conflict, message)
    {
    }

    public FileAlreadyExistsException() : base(HttpStatusCode.Conflict, "A file already exists with that id")
    {
    }
}