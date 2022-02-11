using System.Net;

namespace Trove.Shared.Exception;

public class EntryAlreadyExistsException : EntryBaseException
{
    public EntryAlreadyExistsException(Guid key) : base(key, "An entry already exists with the provided id", HttpStatusCode.Conflict)
    {
    }

    public EntryAlreadyExistsException(Guid key, string message) : base(key, message, HttpStatusCode.Conflict)
    {
    }

    public EntryAlreadyExistsException(string message) : base(HttpStatusCode.Conflict, message)
    {
    }

    public EntryAlreadyExistsException() : base(HttpStatusCode.Conflict, "An entry already exists with the provided id")
    {
    }
}