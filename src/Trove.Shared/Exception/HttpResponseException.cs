using System.Net;

namespace Trove.Shared.Exception;

public class HttpResponseException : System.Exception
{
    public readonly HttpStatusCode Status;
    public readonly string? Value;

    public HttpResponseException(HttpStatusCode status)
    {
        Status = status;
    }

    public HttpResponseException(HttpStatusCode status, string value)
    {
        Status = status;
        Value = value;
    }
}