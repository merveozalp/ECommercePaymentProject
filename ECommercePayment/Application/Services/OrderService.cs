namespace ECommercePayment.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ECommercePayment.Application.DTOs;
    using ECommercePayment.Application.Interfaces;
    using ECommercePayment.Domain.Entities;
    using ECommercePayment.Domain.Exceptions;
    using ECommercePayment.Infrastructure.BalanceManagement;
    using ECommercePayment.Infrastructure.Persistence;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Service for managing order operations
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IBalanceManagementClient _balanceManagementClient;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IBalanceManagementClient balanceManagementClient,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ILogger<OrderService> logger)
        {
            _balanceManagementClient = balanceManagementClient;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new order with stock control using repository transaction management
        /// </summary>
        public async Task<BalanceManagementApiResponse<BalanceManagementPreorderData>> CreateOrderAsync(CreateOrderRequest request)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            _logger.LogInformation($"Starting order creation process for ProductId: {request.ProductId}, Quantity: {request.Quantity}");

            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.ProductId) || request.Quantity <= 0)
                {
                    _logger.LogWarning($"Invalid order request received. ProductId: {request.ProductId}, Quantity: {request.Quantity}");

                    throw new ValidationException("Invalid order request. ProductId and valid Quantity are required.");
                }

                // Validate product exists and business rules
                var product = await _productRepository.GetByIdAsync(request.ProductId);
                if (product == null)
                {
                    _logger.LogWarning($"Product not found during order creation. ProductId: {request.ProductId}", request.ProductId);

                    throw new ValidationException($"Product not found: {request.ProductId}");
                }

                _logger.LogDebug($"Product found. Name: {product.Name}, Price: {product.Price}, Stock: {product.Stock}");

                // Business rule: Maximum order quantity
                if (request.Quantity > 100)
                {
                    _logger.LogWarning($"Order quantity exceeds maximum limit. ProductId: {request.ProductId}, Requested: {request.Quantity}, Limit: 100");

                    throw new ValidationException("Maximum order quantity is 100 items per order");
                }

                // Calculate total amount
                var totalAmount = product.Price * request.Quantity;

                // Create order object
                var order = new Order
                {
                    ProductId = request.ProductId,
                    Amount = totalAmount,
                    Status = "Blocked",
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductId = request.ProductId,
                            ProductName = product.Name,
                            Quantity = request.Quantity,
                            UnitPrice = product.Price,
                            TotalPrice = totalAmount
                        }
                    }
                };

                // Prepare stock reservations
                var stockReservations = new Dictionary<string, int>
            {
                { request.ProductId, request.Quantity }
            };

                var createdOrder = await _orderRepository.CreateOrderWithStockReservationAsync(order, stockReservations);
                if (createdOrder == null)
                {
                    _logger.LogWarning($"Failed to reserve stock during order creation. ProductId: { request.ProductId}, Available: {product.Stock}, Requested: {request.Quantity}");

                    throw new ValidationException($"Failed to reserve stock. Available: {product.Stock}, Requested: {request.Quantity}");
                }

                // Create pre-order request for Balance Management API
                var preOrderRequest = new PreOrderDto
                {
                    OrderId = request.ProductId,
                    Amount = totalAmount
                };

                var balanceResponse = await _balanceManagementClient.PreorderAsync(preOrderRequest);

                // Check if balance blocking was successful
                if (balanceResponse?.Data?.PreOrder.Status != "blocked")
                {
                    _logger.LogWarning($"Balance blocking failed. ProductId: {request.ProductId}, Status: {balanceResponse?.Data?.PreOrder.Status ?? "null"}");

                    // Cancel order and restore stock if balance blocking fails
                    await _orderRepository.CancelOrderWithStockRestorationAsync(request.ProductId);

                    throw new InsufficientBalanceException("Insufficient balance to complete the order");
                }

                stopwatch.Stop();

                _logger.LogInformation($"Order created successfully. OrderId: {createdOrder.Id}, TotalAmount: {totalAmount}, ExecutionTime: {stopwatch.ElapsedMilliseconds} ms");

                return balanceResponse;
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, $"External service error during order creation. ProductId: {request.ProductId}");

                // Note: We can't cancel here since we don't have the created order ID in catch scope
                // The repository transaction will automatically rollback
                throw;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex, $"Unexpected error during order creation. ProductId: {request.ProductId}, ExecutionTime: {stopwatch.ElapsedMilliseconds}ms");

                throw;
            }
        }

        /// <summary>
        /// Completes an order by processing the payment using repository transaction management
        /// </summary>
        public async Task<CompleteOrderResponse> CompleteOrderAsync(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new OrderNotFoundException("Product ID is required");
            }

            var order = await _orderRepository.GetBlockOrderByIdAsync(productId);
            if (order == null)
            {
                throw new OrderNotFoundException($"Order not found: {productId}");
            }

            try
            {
                var completionResponse = await _balanceManagementClient.CompleteOrderAsync(productId);

                // Verify payment completion
                if (completionResponse.PreOrder.Status != "completed")
                {
                    // If payment fails, cancel order and restore stock
                    await _orderRepository.CancelOrderWithStockRestorationAsync(productId);
                    throw new ExternalServiceException("Payment completion failed - stock has been restored");
                }

                // Complete order using repository transaction management
                var orderCompleted = await _orderRepository.CompleteOrderAsync(order.Id);

                if (orderCompleted == null)
                {
                    throw new ExternalServiceException("Failed to complete order in database");
                }

                // Create comprehensive response
                var response = new CompleteOrderResponse
                {
                    Success = true,
                    Message = "Order completed successfully",
                    Data = new CompleteOrderResponseData
                    {
                        Order = new CompletedOrderDto
                        {
                            ProductId = orderCompleted.ProductId,
                            Amount = orderCompleted.Amount,
                            Timestamp = orderCompleted.Timestamp,
                            Status = orderCompleted.Status,
                            CompletedAt = orderCompleted.CompletedAt
                        },
                        UpdatedBalance = new UpdatedBalanceDto
                        {
                            UserId = completionResponse.UpdatedBalance.UserId,
                            TotalBalance = completionResponse.UpdatedBalance.TotalBalance,
                            AvailableBalance = completionResponse.UpdatedBalance.AvailableBalance,
                            BlockedBalance = completionResponse.UpdatedBalance.BlockedBalance,
                            Currency = completionResponse.UpdatedBalance.Currency,
                            LastUpdated = DateTime.UtcNow
                        }
                    }
                };

                return response;
            }
            catch (ExternalServiceException)
            {
                // Cancel order and restore stock if external service fails
                await _orderRepository.CancelOrderWithStockRestorationAsync(productId);
                throw;
            }
        }

        /// <summary>
        /// Cancels an order and restores stock using repository transaction management
        /// </summary>
        public async Task<bool> CancelOrderAsync(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new OrderNotFoundException("Order ID is required");
            }

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new OrderNotFoundException($"Order not found: {orderId}");
            }

            // Only allow cancellation for blocked orders
            if (order.Status != "Blocked")
            {
                throw new ValidationException($"Order cannot be cancelled. Current status: {order.Status}");
            }

            // Cancel order and restore stock using repository transaction
            return await _orderRepository.CancelOrderWithStockRestorationAsync(orderId);
        }
    }
}