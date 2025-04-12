using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MovieHub.Application.Common.Exceptions;

namespace MovieHub.WebApi.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilter()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(DuplicateException), HandleDuplicateException }
        };
    }
    
    public void OnException(ExceptionContext context)
    {
        HandleException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }
        HandleUnknownException(context);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        context.Result = new BadRequestObjectResult(new
        {
            error = "Validation error",
            details = exception.Message
        });

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        context.Result = new NotFoundObjectResult(new
        {
            error = "Not found",
            details = exception.Message
        });

        context.ExceptionHandled = true;
    }

    private void HandleDuplicateException(ExceptionContext context)
    {
        var exception = (DuplicateException)context.Exception;

        context.Result = new ConflictObjectResult(new
        {
            error = "Duplicate",
            details = exception.Message
        });

        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        context.Result = new ObjectResult(new
        {
            error = "Internal Server Error",
            details = context.Exception.Message
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
    
}