using CubosFinance.Application.DTOs.Common;
using CubosFinance.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CubosFinance.Api.Filters;

public class ExceptionHandlingFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = new ErrorResponseDto
        {
            Message = context.Exception.Message
        };

        context.Result = context.Exception switch
        {
            InvalidBranchFormatException => new BadRequestObjectResult(response),
            InvalidAccountFormatException => new BadRequestObjectResult(response),
            DuplicatedAccountNumberException => new ConflictObjectResult(response),
            AccountNotFoundException => new NotFoundObjectResult(response),
            InvalidCvvException => new BadRequestObjectResult(response),
            PhysicalCardAlreadyExistsException => new BadRequestObjectResult(response),
            InsufficientBalanceException => new BadRequestObjectResult(response),
            InvalidOperationException => new BadRequestObjectResult(response),

            _ => new ObjectResult(new ErrorResponseDto
            {
                Message = "An unexpected error occurred."
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            }
        };

        context.ExceptionHandled = true;
    }
}
