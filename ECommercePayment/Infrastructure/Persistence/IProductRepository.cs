namespace ECommercePayment.Infrastructure.Persistence
{
    using ECommercePayment.Domain.Entities;

    /// <summary>
    /// Repository interface for managing product data
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product?> GetByIdAsync(string id);

        /// <summary>
        /// Get all products from the repository
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// Add a new product to the repository
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Product> AddAsync(Product product);

        /// <summary>
        /// Update an existing product in the repository
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Product> UpdateAsync(Product product);

        /// <summary>
        /// Reserve stock for a product (reduces available stock)
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="quantity">Quantity to reserve</param>
        /// <returns>True if reservation successful, false if insufficient stock</returns>
        Task<bool> ReserveStockAsync(string productId, int quantity);

        /// <summary>
        /// Release reserved stock for a product (increases available stock)
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="quantity">Quantity to release</param>
        /// <returns>True if release successful</returns>
        Task<bool> ReleaseStockAsync(string productId, int quantity);

        /// <summary>
        /// Update stock quantity with validation and transaction safety
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="quantityChange">Positive to increase, negative to decrease</param>
        /// <returns>Updated product or null if operation failed</returns>
        Task<Product?> UpdateStockAsync(string productId, int quantityChange);
    }
} 