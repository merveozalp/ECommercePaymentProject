namespace ECommercePayment.Application.DTOs
{
    /// <summary>
    /// Updated balance information
    /// </summary>
    public class UpdatedBalanceDto
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Total balance
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// Available balance
        /// </summary>
        public decimal AvailableBalance { get; set; }

        /// <summary>
        /// Blocked balance
        /// </summary>
        public decimal BlockedBalance { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Last updated timestamp
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
} 