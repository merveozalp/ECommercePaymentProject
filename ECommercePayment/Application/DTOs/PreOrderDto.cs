namespace ECommercePayment.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for pre-order operations with Balance Management API
    /// </summary>
    public class PreOrderDto
    {
        /// <summary>
        /// The unique identifier of the order
        /// </summary>
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// The amount to be blocked for the order
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Current status of the pre-order
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
} 