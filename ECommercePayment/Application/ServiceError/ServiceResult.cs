namespace ECommercePayment.Application.ServiceError
{
    /// <summary>
    /// Generic service result wrapper for handling success/failure scenarios without exceptions
    /// </summary>
    /// <typeparam name="T">Type of data returned on success</typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Data returned on successful operation
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Error information if operation failed
        /// </summary>
        public ServiceError? Error { get; set; }

        /// <summary>
        /// Additional message for context
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Execution time in milliseconds (for monitoring)
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// Creates a successful result with data
        /// </summary>
        /// <param name="data">Success data</param>
        /// <param name="message">Optional success message</param>
        /// <param name="executionTimeMs">Execution time</param>
        /// <returns>Success result</returns>
        public static ServiceResult<T> Success(T data, string? message = null, long executionTimeMs = 0)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                ExecutionTimeMs = executionTimeMs
            };
        }

        /// <summary>
        /// Creates a failed result with error information
        /// </summary>
        /// <param name="error">Error details</param>
        /// <param name="message">Optional error context message</param>
        /// <param name="executionTimeMs">Execution time</param>
        /// <returns>Failure result</returns>
        public static ServiceResult<T> Failure(ServiceError error, string? message = null, long executionTimeMs = 0)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Error = error,
                Message = message,
                ExecutionTimeMs = executionTimeMs
            };
        }

        /// <summary>
        /// Creates a failed result with error code and description
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="errorDescription">Error description</param>
        /// <param name="httpStatusCode">HTTP status code</param>
        /// <param name="message">Optional context message</param>
        /// <param name="executionTimeMs">Execution time</param>
        /// <returns>Failure result</returns>
        public static ServiceResult<T> Failure(string errorCode, string errorDescription, int httpStatusCode = 400, string? message = null, long executionTimeMs = 0)
        {
            var error = new ServiceError
            {
                Code = errorCode,
                Description = errorDescription,
                HttpStatusCode = httpStatusCode
            };

            return Failure(error, message, executionTimeMs);
        }
    }
} 