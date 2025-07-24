namespace ECommercePayment.Infrastructure.BalanceManagement
{
    /// <summary>
    /// Balance Management order response data structure
    /// </summary>
    public class BalanceManagementOderData
    {
        /// <summary>
        /// PreOrder information
        /// </summary>
        public BalanceManagementPreorder PreOrder { get; set; } = new();
        
        /// <summary>
        /// Updated balance information
        /// </summary>
        public BalanceManagementBalance UpdatedBalance { get; set; } = new();
    }
} 