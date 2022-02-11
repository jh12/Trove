using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Trove.Shared.Exception;
using ILogger = Serilog.ILogger;

namespace Trove.Middleware;

// ReSharper disable once ClassNeverInstantiated.Global
public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    private readonly ILogger _logger;
    public int Order => int.MaxValue - 10;

    public HttpResponseExceptionFilter(ILogger logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is NotImplementedException notImplemented)
        {
            _logger.Error(context.Exception, "Not implemented exception was thrown");
            
            context.Result = new ObjectResult(notImplemented.Message)
            {
                StatusCode = (int?)HttpStatusCode.NotImplemented
            };
            context.ExceptionHandled = true;
        }
        else if (context.Exception is EntryBaseException entryException)
        {
            context.Result = new ObjectResult(entryException.Value)
            {
                StatusCode = (int?)entryException.Status
            };
            context.ExceptionHandled = true;
        }
        else if (context.Exception is HttpResponseException httpResponse)
        {
            _logger.Error(context.Exception, "An unhandled exception was thrown");
            
            context.Result = new ObjectResult(httpResponse.Value)
            {
                StatusCode = (int?)httpResponse.Status
            };
            context.ExceptionHandled = true;
        }
        else if (context.Exception != null)
            _logger.Error(context.Exception, "An unhandled exception was thrown");
    }
}