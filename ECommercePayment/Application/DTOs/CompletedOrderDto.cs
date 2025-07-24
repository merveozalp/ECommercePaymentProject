namespace ECommercePayment.Application.DTOs
{
    /// <summary>
    /// Completed order details
    /// </summary>
    public class CompletedOrderDto
    {
        /// <summary>
        /// Order identifier
        /// </summary>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Order amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Order creation timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Order status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Completion timestamp
        /// </summary>
        public DateTime? CompletedAt { get; set; }
    }
} 