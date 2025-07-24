namespace ECommercePayment.Infrastructure.BalanceManagement
{
    /// <summary>
    /// Response wrapper for Balance Management API
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class BalanceManagementApiResponse<T>
    {
        /// <summary>
        /// Indicates if the request was successful
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Descriptive message about the operation
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// The actual data returned by the API
        /// </summary>
        public T? Data { get; set; }
    }
} 