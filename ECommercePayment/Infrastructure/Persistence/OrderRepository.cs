namespace ECommercePayment.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using ECommercePayment.Domain.Entities;

    /// <summary>
    /// Order Repository
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceDbContext _context;

        public OrderRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get order by id with all items
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Order?> GetByIdAsync(string orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.ProductId == orderId);
        }

        /// <summary>
        /// Get order by id with all items
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Order?> GetBlockOrderByIdAsync(string orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.ProductId == orderId && o.Status == "blocked");
        }

        /// <summary>
        /// Add order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        /// <summary>
        /// Create order with stock reservation in a single transaction
        /// </summary>
        /// <param name="order">Order to create</param>
        /// <param name="stockReservations">Dictionary of ProductId -> Quantity to reserve</param>
        /// <returns>Created order or null if stock reservation failed</returns>
        public async Task<Order?> CreateOrderWithStockReservationAsync(Order order, Dictionary<string, int> stockReservations)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Reserve stock for all products
                foreach (var reservation in stockReservations)
                {
                    var product = await _context.Products.FindAsync(reservation.Key);
                    if (product == null)
                    {
                        return null; // Product not found
                    }

                    if (product.Stock < reservation.Value)
                    {
                        return null; // Insufficient stock
                    }

                    product.Stock -= reservation.Value;
                    _context.Products.Update(product);
                }

                // Create the order
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        /// <summary>
        /// Complete order and finalize stock changes in a single transaction
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns>True if completion successful</returns>
        public async Task<Order> CompleteOrderAsync(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null || order.Status != "Blocked")
                {
                    return null;
                }

                // Update order status
                order.Status = "completed";
                order.CompletedAt = DateTime.UtcNow;
                _context.Orders.Update(order);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        /// <summary>
        /// Cancel order and restore stock in a single transaction
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns>True if cancellation successful</returns>
        public async Task<bool> CancelOrderWithStockRestorationAsync(string orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.ProductId == orderId);

                if (order == null || order.Status != "Blocked")
                {
                    return false;
                }

                // Restore stock for all order items
                foreach (var item in order.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                        _context.Products.Update(product);
                    }
                }

                // Update order status
                order.Status = "Cancelled";
                order.CompletedAt = DateTime.UtcNow;
                _context.Orders.Update(order);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
} 