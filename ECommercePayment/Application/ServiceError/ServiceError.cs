namespace ECommercePayment.Application.ServiceError
{
    /// <summary>
    /// Represents structured error information for service operations
    /// </summary>
    public class ServiceError
    {
        /// <summary>
        /// Unique error code for programmatic handling
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable error description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// HTTP status code associated with this error
        /// </summary>
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// Additional error details or context
        /// </summary>
        public Dictionary<string, object>? Details { get; set; }

        /// <summary>
        /// Error category for grouping related errors
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Creates a new service error
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="description">Error description</param>
        /// <param name="httpStatusCode">HTTP status code</param>
        /// <param name="category">Error category</param>
        public ServiceError(string code, string description, int httpStatusCode, string category = "")
        {
            Code = code;
            Description = description;
            HttpStatusCode = httpStatusCode;
            Category = category;
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        public ServiceError() { }

        /// <summary>
        /// Adds additional detail to the error
        /// </summary>
        /// <param name="key">Detail key</param>
        /// <param name="value">Detail value</param>
        /// <returns>Current ServiceError instance for chaining</returns>
        public ServiceError WithDetail(string key, object value)
        {
            Details ??= new Dictionary<string, object>();
            Details[key] = value;
            return this;
        }

        /// <summary>
        /// Sets the error category
        /// </summary>
        /// <param name="category">Category name</param>
        /// <returns>Current ServiceError instance for chaining</returns>
        public ServiceError WithCategory(string category)
        {
            Category = category;
            return this;
        }

        /// <summary>
        /// String representation of the error
        /// </summary>
        /// <returns>Formatted error string</returns>
        public override string ToString()
        {
            return $"[{Code}] {Description} (HTTP: {HttpStatusCode})";
        }
    }
} 