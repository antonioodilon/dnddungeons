using System.Net;

namespace DndDungeons.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        // Creating a global exception handler:
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this._logger = logger;
            this._next = next;
        }
        struct ErrorIdMessage
        {
            public Guid Id;
            public string ErrorMessage;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            } catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();

                // Log the exception:
                _logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return a custom error response:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                ErrorIdMessage error = new ErrorIdMessage
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong. The developers will look into this problem.",
                };

                await httpContext.Response.WriteAsJsonAsync(new { error.Id, error.ErrorMessage});
            }

        }
    }
}
