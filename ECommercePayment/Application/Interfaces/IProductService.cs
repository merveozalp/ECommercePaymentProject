using ECommercePayment.Application.DTOs;

namespace ECommercePayment.Application.Interfaces
{
    /// <summary>
    /// Product Service
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get products
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
} 