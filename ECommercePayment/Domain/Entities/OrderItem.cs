namespace ECommercePayment.Domain.Entities
{
    /// <summary>
    /// Represents an item within an order
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Unique identifier for the order item (auto-increment)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product identifier
        /// </summary>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Product name (snapshot for historical reference)
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Quantity of the product ordered
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price at the time of order (snapshot)
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Total price for this line item (UnitPrice * Quantity)
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Order creation timestamp
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
} 