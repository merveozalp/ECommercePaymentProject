namespace ECommercePayment.Application.Interfaces
{
    using ECommercePayment.Application.DTOs;
    using ECommercePayment.Infrastructure.BalanceManagement;

    /// <summary>
    /// IOrderService
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Create order in Balance Management API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BalanceManagementApiResponse<BalanceManagementPreorderData>> CreateOrderAsync(CreateOrderRequest request);

        /// <summary>
        /// Complete order in Balance Management API
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<CompleteOrderResponse> CompleteOrderAsync(string orderId);
    }
} 