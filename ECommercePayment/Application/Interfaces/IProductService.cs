using ECommercePayment.Application.DTOs;

namespace ECommercePayment.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
} 