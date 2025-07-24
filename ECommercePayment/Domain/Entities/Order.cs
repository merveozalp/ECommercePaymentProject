namespace ECommercePayment.Domain.Entities
{
    /// <summary>
    /// Represents an order in the e-commerce system
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Unique identifier for the order (auto-increment)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product identifier for this order
        /// </summary>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Total amount of the order
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Current status of the order
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Order creation timestamp
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Order completion timestamp
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// List of items in the order
        /// </summary>
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
} 