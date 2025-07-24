namespace ECommercePayment.Tests.Service
{
    using Moq;
    using FluentAssertions;

    using ECommercePayment.Application.Services;
    using ECommercePayment.Application.DTOs;
    using ECommercePayment.Domain.Entities;
    using ECommercePayment.Infrastructure.BalanceManagement;
    using ECommercePayment.Infrastructure.Persistence;
    using ECommercePayment.Domain.Exceptions;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Unit tests for OrderService
    /// </summary>
    public class OrderServiceTests
    {
        private Mock<IBalanceManagementClient> _mockBalanceClient;
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<ILogger<OrderService>> _mockLoggerService;
        private OrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            _mockBalanceClient = new Mock<IBalanceManagementClient>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockLoggerService = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(
                _mockBalanceClient.Object,
                _mockOrderRepository.Object,
                _mockProductRepository.Object,
                _mockLoggerService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockBalanceClient?.Reset();
            _mockOrderRepository?.Reset();
            _mockProductRepository?.Reset();
        }

        [Test]
        public async Task CreateOrderAsync_WhenValidRequest_ShouldCreateOrderSuccessfully()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                ProductId = "prod-001",
                Quantity = 2 
            };

            var product = new Product { Id = "prod-001", Name = "Test Product", Price = 10.00m };
            var expectedTotalAmount = 20.00m;

            var balanceResponse = new BalanceManagementApiResponse<BalanceManagementPreorderData>
            {
                Success = true,
                Data = new BalanceManagementPreorderData
                {
                    PreOrder = new BalanceManagementPreorder
                    {
                        OrderId = "order-123",
                        Amount = expectedTotalAmount,
                        Status = "blocked"
                    }
                }
            };

            _mockProductRepository
                .Setup(x => x.GetByIdAsync("prod-001"))
                .ReturnsAsync(product);

            _mockBalanceClient
                .Setup(x => x.PreorderAsync(It.IsAny<PreOrderDto>()))
                .ReturnsAsync(balanceResponse);

            _mockOrderRepository
                .Setup(x => x.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order order) => order);

            _mockOrderRepository
                .Setup(x => x.CreateOrderWithStockReservationAsync(It.IsAny<Order>(), It.IsAny<Dictionary<string, int>>()))
                .ReturnsAsync(new Order { Id = 1, Amount = expectedTotalAmount, Status = "blocked" });

            // Act
            var result = await _orderService.CreateOrderAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.PreOrder.OrderId.Should().NotBeEmpty();
            result.Data.PreOrder.Amount.Should().Be(expectedTotalAmount);
            result.Data.PreOrder.Status.Should().Be("blocked");

            _mockProductRepository.Verify(x => x.GetByIdAsync("prod-001"), Times.Once);
            _mockBalanceClient.Verify(x => x.PreorderAsync(It.IsAny<PreOrderDto>()), Times.Once);
        }

        [Test]
        public async Task CreateOrderAsync_WhenProductNotFound_ShouldThrowValidationException()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                ProductId = "nonexistent",
                Quantity = 1
            };

            _mockProductRepository
                .Setup(x => x.GetByIdAsync("nonexistent"))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            var act = async () => await _orderService.CreateOrderAsync(request);

            await act.Should().ThrowAsync<ValidationException>()
               .WithMessage("Product not found: nonexistent");

            _mockProductRepository.Verify(x => x.GetByIdAsync("nonexistent"), Times.Once);
            _mockBalanceClient.Verify(x => x.PreorderAsync(It.IsAny<PreOrderDto>()), Times.Never);
        }

        [Test]
        public async Task CompleteOrderAsync_WhenValidOrderId_ShouldCompleteOrderSuccessfully()
        {
            // Arrange
            var orderId = "order-123";
            var existingOrder = new Order
            {
                Id = 1,
                ProductId = orderId,
                Amount = 50.00m,
                Status = "Blocked",
                Items = new List<OrderItem>()
            };

            var completionResponse = new BalanceManagementOderData
            {
                PreOrder = new BalanceManagementPreorder
                {
                    OrderId = orderId,
                    Amount = 50.00m,
                    Status = "completed"
                },
                UpdatedBalance = new BalanceManagementBalance
                {
                    BlockedBalance = 19.00m,
                    TotalBalance = 100.00m,
                    AvailableBalance = 81.00m
                }
            };

            _mockOrderRepository
                .Setup(x => x.GetBlockOrderByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mockBalanceClient
                .Setup(x => x.CompleteOrderAsync(orderId))
                .ReturnsAsync(completionResponse);

            _mockOrderRepository
                .Setup(x => x.CompleteOrderAsync(It.IsAny<int>()))
                .ReturnsAsync(existingOrder);

            _mockOrderRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order order) => order);

            // Act
            var result = await _orderService.CompleteOrderAsync(orderId);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task CompleteOrderAsync_WhenOrderNotFound_ShouldThrowOrderNotFoundException()
        {
            // Arrange
            var orderId = "nonexistent-order";

            _mockOrderRepository
                .Setup(x => x.GetByIdAsync(orderId))
                .ReturnsAsync((Order?)null);

            // Act & Assert
            var act = async () => await _orderService.CompleteOrderAsync(orderId);

            await act.Should().ThrowAsync<OrderNotFoundException>()
               .WithMessage($"Order not found: {orderId}");

            _mockBalanceClient.Verify(x => x.CompleteOrderAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task CompleteOrderAsync_WhenPaymentFails_ShouldThrowExternalServiceException()
        {
            // Arrange
            var orderId = "prod-001";
            var existingOrder = new Order
            {
                ProductId = orderId,
                Amount = 50.00m,
                Status = "Blocked",
                Items = new List<OrderItem>()
            };

            var completionResponse = new BalanceManagementOderData
            {
                PreOrder = new BalanceManagementPreorder
                {
                    OrderId = orderId,
                    Amount = 50.00m,
                    Status = "Cancelled"
                },
                UpdatedBalance = new BalanceManagementBalance
                {
                    BlockedBalance = 19.00m,
                    TotalBalance = 100.00m,
                    AvailableBalance = 81.00m
                }
            };

            _mockOrderRepository
                .Setup(x => x.GetBlockOrderByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mockBalanceClient
                .Setup(x => x.CompleteOrderAsync(orderId))
                .ReturnsAsync(completionResponse);

            // Act & Assert
            var act = async () => await _orderService.CompleteOrderAsync(orderId);

            await act.Should().ThrowAsync<ExternalServiceException>()
               .WithMessage("Payment completion failed - stock has been restored");

            _mockOrderRepository.Verify(x => x.UpdateAsync(It.IsAny<Order>()), Times.Never);
        }
    }
}