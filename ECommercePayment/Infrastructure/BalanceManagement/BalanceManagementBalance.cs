namespace ECommercePayment.Infrastructure.BalanceManagement
{
    /// <summary>
    /// Balance information from Balance Management
    /// </summary>
    public class BalanceManagementBalance
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        
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
        public string Currency { get; set; } = string.Empty;
        
        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
} 