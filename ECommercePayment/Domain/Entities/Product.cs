namespace ECommercePayment.Domain.Entities
{
    public class Product
    {
        /// <summary>
        /// Unique identifier for the product
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; } 

        /// <summary>
        /// Detailed description of the product
        /// </summary>
        public string Description { get; set; } 

        /// <summary>
        /// Price of the product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Currency code (e.g., USD, EUR, TRY)
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Product category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Number of units available in stock
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Product creation timestamp
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
} 