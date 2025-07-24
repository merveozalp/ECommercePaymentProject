namespace ECommercePayment.Tests.Client
{
    using Moq.Protected;
    using Moq;
    using FluentAssertions;

    using System.Net;
    using System.Text.Json;

    using ECommercePayment.Infrastructure.BalanceManagement;
    using ECommercePayment.Application.DTOs;
    using ECommercePayment.Domain.Exceptions;

    /// <summary>
    /// Balance Managemnet Service Client Test
    /// </summary>
    public class BalanceManagementClientTest
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private BalanceManagementClient _balanceManagementClient;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test-api.com")
            };
            _balanceManagementClient = new BalanceManagementClient(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetProductsAsync_WhenApiReturnsSuccessfulResponse_ShouldReturnProducts()
        {
            // Arrange
            var expectedProducts = new List<ProductDto>
            {
                new ProductDto
                {
                    Id = "prod-001",
                    Name = "Test Product 1",
                    Description = "Test Description",
                    Price = 19.99m,
                    Currency = "Currency.USD",
                    Category = "ProductCategory.Electronics",
                    Stock = 10
                }
            };

            var apiResponse = new BalanceManagementApiResponse<IEnumerable<ProductDto>>
            {
                Success = true,
                Data = expectedProducts
            };

            var responseJson = JsonSerializer.Serialize(apiResponse);
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, System.Text.Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _balanceManagementClient.GetProductsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be("prod-001");
            result.First().Name.Should().Be("Test Product 1");
            result.First().Price.Should().Be(19.99m);
        }

        [Test]
        public async Task GetProductsAsync_WhenApiReturnsUnsuccessfulResponse_ShouldThrowExternalServiceException()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act & Assert
            var act = async () => await _balanceManagementClient.GetProductsAsync();

            await act.Should().ThrowAsync<ExternalServiceException>()
                .WithMessage("Failed to fetch products: Response status code does not indicate success: 500 (Internal Server Error).");
        }

        [Test]
        public async Task GetProductsAsync_WhenHttpRequestExceptionThrown_ShouldThrowExternalServiceException()
        {
            // Arrange
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            var act = async () => await _balanceManagementClient.GetProductsAsync();

            await act.Should().ThrowAsync<ExternalServiceException>()
                .WithMessage("Failed to fetch products: Network error");
        }

        [Test]
        public async Task PreorderAsync_WhenApiReturnsSuccessfulResponse_ShouldReturnPreorderData()
        {
            // Arrange
            var preOrderRequest = new PreOrderDto
            {
                OrderId = "order-123",
                Amount = 50.00m
            };

            var expectedResponse = new BalanceManagementApiResponse<BalanceManagementPreorderData>
            {
                Success = true,
                Data = new BalanceManagementPreorderData
                {
                    PreOrder = new BalanceManagementPreorder
                    {
                        OrderId = "order-123",
                        Amount = 50.00m,
                        Status = "Blocked"
                    }
                }
            };

            var responseJson = JsonSerializer.Serialize(expectedResponse);
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, System.Text.Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _balanceManagementClient.PreorderAsync(preOrderRequest);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.PreOrder.OrderId.Should().Be("order-123");
            result.Data.PreOrder.Amount.Should().Be(50.00m);
            result.Data.PreOrder.Status.Should().Be("Blocked");
        }

        [Test]
        public async Task PreorderAsync_WhenApiReturnsError_ShouldThrowExternalServiceException()
        {
            // Arrange
            var preOrderRequest = new PreOrderDto
            {
                OrderId = "order-123",
                Amount = 50.00m
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act & Assert
            var act = async () => await _balanceManagementClient.PreorderAsync(preOrderRequest);

            await act.Should().ThrowAsync<ExternalServiceException>()
                .WithMessage("Failed to create preorder: Response status code does not indicate success: 400 (Bad Request).");
        }

        [Test]
        public async Task CompleteOrderAsync_WhenApiReturnsError_ShouldThrowExternalServiceException()
        {
            //Arrange
           var orderId = "order-123";
            var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            //Act & Assert
            var act = async () => await _balanceManagementClient.CompleteOrderAsync(orderId);

            await act.Should().ThrowAsync<ExternalServiceException>()
                .WithMessage("Failed to complete order: Response status code does not indicate success: 404 (Not Found).");
        }
    }
}