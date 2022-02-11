using System.Net;

namespace Trove.Shared.Exception;

public class EntryNotFoundException : EntryBaseException
{
    public EntryNotFoundException(Guid key) : base(key, "No entry found with the provided id", HttpStatusCode.NotFound)
    {
    }

    public EntryNotFoundException(Guid key, string message) : base(key, message, HttpStatusCode.NotFound)
    {
    }
}