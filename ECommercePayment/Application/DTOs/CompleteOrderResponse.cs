namespace ECommercePayment.Application.DTOs
{
    /// <summary>
    /// Response DTO for order completion with full details
    /// </summary>
    public class CompleteOrderResponse
    {
        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Response message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Response data containing order and balance information
        /// </summary>
        /// <summary>
        /// Completed order information
        /// </summary>
        public CompleteOrderResponseData Data { get; set; }
    }
} 