using RMSHOP.DAL.DTO.Response.GlobalExceptionHandling;

namespace RMSHOP.PL.Middleware
{
    public class GlobalExceptionHandling
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch (Exception ex)
            {
                var errorDetails = new ErrorDetails()
                {
                     StatusCode= StatusCodes.Status500InternalServerError,
                     Message="server error!",
                    // StackTrace is shown only in development mode.
                    // In production, the exception details are written to log files for debugging and monitoring.
                    //StackTrace= ex.StackTrace
                    StackTrace = ex.InnerException.Message
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }
    }
}
