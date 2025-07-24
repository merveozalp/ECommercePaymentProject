namespace ECommercePayment.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using ECommercePayment.Domain.Entities;

    /// <summary>
    /// SQL implementation of the product repository interface for managing product data
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        /// <summary>
        /// Get all products from the repository
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Add a new product to the repository
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Update an existing product in the repository
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Reserve stock for a product (reduces available stock) with transaction safety
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="quantity">Quantity to reserve</param>
        /// <returns>True if reservation successful, false if insufficient stock</returns>
        public async Task<bool> ReserveStockAsync(string productId, int quantity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return false;
                }

                if (product.Stock < quantity)
                {
                    return false; // Insufficient stock
                }

                product.Stock -= quantity;
                _context.Products.Update(product);
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

        /// <summary>
        /// Release reserved stock for a product (increases available stock) with transaction safety
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="quantity">Quantity to release</param>
        /// <returns>True if release successful</returns>
        public async Task<bool> ReleaseStockAsync(string productId, int quantity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return false;
                }

                product.Stock += quantity;
                _context.Products.Update(product);
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

        /// <summary>
        /// Update stock quantity with validation and transaction safety
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="quantityChange">Positive to increase, negative to decrease</param>
        /// <returns>Updated product or null if operation failed</returns>
        public async Task<Product?> UpdateStockAsync(string productId, int quantityChange)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return null;
                }

                var newStock = product.Stock + quantityChange;
                if (newStock < 0)
                {
                    return null; // Would result in negative stock
                }

                product.Stock = newStock;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return product;
            }
            catch
            {
                await transaction.RollbackAsync();
                return null;
            }
        }
    }
} 