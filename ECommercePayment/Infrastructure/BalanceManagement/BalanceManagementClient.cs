using ECommercePayment.Application.DTOs;
using ECommercePayment.Domain.Exceptions;

namespace ECommercePayment.Infrastructure.BalanceManagement
{
    /// <summary>
    /// Balance Management client
    /// </summary>
    public class BalanceManagementClient : IBalanceManagementClient
    {
        private readonly HttpClient _httpClient;

        public BalanceManagementClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get all products from Balance Management API
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/products");
                response.EnsureSuccessStatusCode();
                
                var apiResponse = await response.Content.ReadFromJsonAsync<BalanceManagementApiResponse<IEnumerable<ProductDto>>>();
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
                
                return new List<ProductDto>();
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to fetch products: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ExternalServiceException($"Unexpected error while fetching products: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Preorder method - returns full Balance Management response
        /// </summary>
        /// <param name="preOrder"></param>
        /// <returns>Full Balance Management response with preOrder and balance info</returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<BalanceManagementApiResponse<BalanceManagementPreorderData>> PreorderAsync(PreOrderDto preOrder)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/balance/preorder", preOrder);
                response.EnsureSuccessStatusCode();
                
                var apiResponse = await response.Content.ReadFromJsonAsync<BalanceManagementApiResponse<BalanceManagementPreorderData>>();
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse;
                }
                
                throw new ExternalServiceException("PreOrder failed - no valid response from Balance Management service");
            }
            catch (HttpRequestException ex)
            {
                // log 
                throw new ExternalServiceException($"Failed to create preorder: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // log 
                throw new ExternalServiceException($"Unexpected error during preorder: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Complete an order by ID
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<BalanceManagementOderData> CompleteOrderAsync(string orderId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"/api/balance/complete", orderId);
                response.EnsureSuccessStatusCode();
                
                var apiResponse = await response.Content.ReadFromJsonAsync<BalanceManagementApiResponse<BalanceManagementOderData>>();
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
                
                throw new ExternalServiceException("Order completion failed - no valid response from Balance Management service");
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to complete order: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ExternalServiceException($"Unexpected error during order completion: {ex.Message}", ex);
            }
        }
    }
} 