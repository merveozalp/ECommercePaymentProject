namespace ECommercePayment.Infrastructure.BalanceManagement
{
    /// <summary>
    /// Represents a pre-order response from the Balance Management API
    /// </summary>
    public class BalanceManagementPreorder
    {
        /// <summary>
        /// The unique identifier of the order
        /// </summary>
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// The amount blocked for this pre-order
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The current status of the pre-order
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
} 