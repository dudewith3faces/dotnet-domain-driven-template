using System.Net;
using System.Net.Mime;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Presentation.DTOs.Response.Base;

namespace Presentation.Extensions;
public static class ExceptionHandlerExtension
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(x =>
        {
            x.Run(async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                int status = (int)HttpStatusCode.BadRequest;
                object response;

                if (exception is BaseException baseException)
                {
                    status = baseException.HttpStatus;
                    response = new ErrorResponse(baseException.Message, baseException.Payload);
                }
                else if (exception is OperationCanceledException) response = new ErrorResponse(ErrorMessageConstant.REQUEST_TERMINATED_ERROR);
                else response = new ErrorResponse(ErrorMessageConstant.GENERAL_ERROR);

                context.Response.StatusCode = status;
                await context.Response.WriteAsync(response.Serialize());

                return;
            });
        });

    }
}
