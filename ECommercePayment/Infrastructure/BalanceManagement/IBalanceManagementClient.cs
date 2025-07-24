namespace ECommercePayment.Infrastructure.BalanceManagement
{
    using ECommercePayment.Application.DTOs;

    /// <summary>
    /// IBalanceManagementClient interface for Balance Management API interactions
    /// </summary>
    public interface IBalanceManagementClient
    {
        /// <summary>
        /// Get all products from Balance Management API
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProductDto>> GetProductsAsync();

        /// <summary>
        /// Preorder method - returns full Balance Management response
        /// </summary>
        /// <param name="preOrder"></param>
        /// <returns></returns>
        Task<BalanceManagementApiResponse<BalanceManagementPreorderData>> PreorderAsync(PreOrderDto preOrder);

        /// <summary>
        /// Complete an order in Balance Management
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<BalanceManagementOderData> CompleteOrderAsync(string orderId);
    }
} 