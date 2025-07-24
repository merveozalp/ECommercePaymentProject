namespace ECommercePayment.Middleware
{
    using System.Net;
    using System.Text.Json;

    using ECommercePayment.Domain.Exceptions;

    /// <summary>
    /// ExceptionHandlingMiddleware
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        ///  Invokes the middleware to handle exceptions globally
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (OrderNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (InsufficientBalanceException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.PaymentRequired);
            }
            catch (ExternalServiceException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.ServiceUnavailable);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Handles exceptions and writes a standardized error response to the HTTP context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            var traceId = context.TraceIdentifier;
            
            // Set log level based on exception type
            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(ex, "Server error occurred. Path: {Path}, TraceId: {TraceId}", context.Request.Path, traceId);
            }
            else
            {
                _logger.LogWarning(ex, "Business logic error occurred. Path: {Path}, TraceId: {TraceId}, StatusCode: {StatusCode}", 
                    context.Request.Path, traceId, (int)statusCode);
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            
            var errorResponse = new
            {
                status = context.Response.StatusCode,
                error = ex.Message,
                traceId,
                timestamp = DateTime.UtcNow
            };
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
} 