using System.Collections.Generic;
using System.Threading.Tasks;
using ECommercePayment.Application.DTOs;
using ECommercePayment.Application.Interfaces;
using ECommercePayment.Infrastructure.BalanceManagement;

namespace ECommercePayment.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IBalanceManagementClient _balanceClient;
        public ProductService(IBalanceManagementClient balanceClient)
        {
            _balanceClient = balanceClient;
        }
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return await _balanceClient.GetProductsAsync();
        }
    }
} 