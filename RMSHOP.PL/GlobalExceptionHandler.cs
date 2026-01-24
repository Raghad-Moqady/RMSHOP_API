using Microsoft.AspNetCore.Diagnostics;
using RMSHOP.DAL.DTO.Response.GlobalExceptionHandling;
using System;

namespace RMSHOP.PL
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            //catch code
            var errorDetails = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "server error!",
                // StackTrace is shown only in development mode.
                // In production, the exception details are written to log files for debugging and monitoring.
                //StackTrace= exception.StackTrace
                StackTrace = exception.InnerException.Message
            };
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(errorDetails);

            return true;
        }
    }
}
