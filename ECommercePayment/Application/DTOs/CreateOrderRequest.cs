namespace ECommercePayment.Application.DTOs
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new order
    /// </summary>
    public class CreateOrderRequest
    {
        /// <summary>
        /// ID of the product to order
        /// </summary>
        /// <example>prod-001</example>
        [Required]
        public string ProductId { get; set; } = string.Empty;
        
        /// <summary>
        /// Quantity of the product to order
        /// </summary>
        /// <example>2</example>
        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }
    }
} 