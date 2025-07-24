namespace ECommercePayment.Application.DTOs
{
    /// <summary>
    /// Product information with pricing and stock details
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Unique identifier for the product
        /// </summary>
        /// <example>prod-001</example>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name of the product
        /// </summary>
        /// <example>Gaming Laptop</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the product
        /// </summary>
        /// <example>High-performance gaming laptop with RTX graphics</example>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Price of the product
        /// </summary>
        /// <example>1299.99</example>
        public decimal Price { get; set; }

        /// <summary>
        /// Currency of the product price
        /// </summary>
        /// <example>USD</example>
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Product category
        /// </summary>
        /// <example>Electronics</example>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Number of units available in stock
        /// </summary>
        /// <example>50</example>
        public int Stock { get; set; }
    }
} 