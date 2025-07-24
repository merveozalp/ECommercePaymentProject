namespace ECommercePayment.Infrastructure.Persistence
{
    using ECommercePayment.Domain.Entities;
    public interface IOrderRepository
    {
        /// <summary>
        /// Get by Id for order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<Order?> GetByIdAsync(string orderId);

        /// <summary>
        /// Get by Id for order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<Order?> GetBlockOrderByIdAsync(string orderId);

        /// <summary>
        /// Add a new order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<Order> AddAsync(Order order);

        /// <summary>
        /// Update an existing order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<Order> UpdateAsync(Order order);

        /// <summary>
        /// Create order with stock reservation in a single transaction
        /// </summary>
        /// <param name="order">Order to create</param>
        /// <param name="stockReservations">Dictionary of ProductId -> Quantity to reserve</param>
        /// <returns>Created order or null if stock reservation failed</returns>
        Task<Order?> CreateOrderWithStockReservationAsync(Order order, Dictionary<string, int> stockReservations);

        /// <summary>
        /// Complete order and finalize stock changes in a single transaction
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns>True if completion successful</returns>
        Task<Order> CompleteOrderAsync(int orderId);

        /// <summary>
        /// Cancel order and restore stock in a single transaction
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns>True if cancellation successful</returns>
        Task<bool> CancelOrderWithStockRestorationAsync(string orderId);
    }
} 