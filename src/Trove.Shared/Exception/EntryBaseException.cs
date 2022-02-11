using System.Net;

namespace Trove.Shared.Exception;

public abstract class EntryBaseException : HttpResponseException
{
    public Guid? Key { get; set; }

    protected EntryBaseException(Guid key, HttpStatusCode statusCode) : base(statusCode)
    {
        Key = key;
    }

    protected EntryBaseException(Guid key, string message, HttpStatusCode statusCode) : base(statusCode, message)
    {
        Key = key;
    }

    protected EntryBaseException(HttpStatusCode status) : base(status)
    {
    }

    protected EntryBaseException(HttpStatusCode status, string message) : base(status, message)
    {
    }
}