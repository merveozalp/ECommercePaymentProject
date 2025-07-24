namespace ECommercePayment.Infrastructure.BalanceManagement
{
    /// <summary>
    /// Balance Management preorder response data structure
    /// </summary>
    public class BalanceManagementPreorderData
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