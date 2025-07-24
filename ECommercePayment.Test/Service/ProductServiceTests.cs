namespace ECommercePayment.Tests.Service
{
    using Moq;
    using FluentAssertions;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ECommercePayment.Application.Services;
    using ECommercePayment.Application.DTOs;
    using ECommercePayment.Infrastructure.BalanceManagement;
    using ECommercePayment.Domain.Exceptions;

    /// <summary>
    /// Unit tests for ProductService
    /// </summary>
    public class ProductServiceTests
    {
        private Mock<IBalanceManagementClient> _mockBalanceClient;
        private ProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _mockBalanceClient = new Mock<IBalanceManagementClient>();
            _productService = new ProductService(_mockBalanceClient.Object);
        }

        [Test]
        public async Task GetProductsAsync_WhenBalanceManagementReturnsProducts_ShouldReturnProducts()
        {
            // Arrange
            var expectedProducts = new List<ProductDto>
            {
                new ProductDto
                {
                    Id = "prod-001",
                    Name = "Test Product 1",
                    Description = "Test Description 1",
                    Price = 19.99m,
                    Currency = "Currency.USD",
                    Category = "Electronics",
                    Stock = 10
                },
                new ProductDto
                {
                    Id = "prod-002",
                    Name = "Test Product 2",
                    Description = "Test Description 2",
                    Price = 29.99m,
                    Currency =" Currency.USD",
                    Category = "Books",
                    Stock = 5
                }
            };

            _mockBalanceClient
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedProducts);
            
            _mockBalanceClient.Verify(x => x.GetProductsAsync(), Times.Once);
        }

        [Test]
        public async Task GetProductsAsync_WhenBalanceManagementReturnsEmptyList_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyProducts = new List<ProductDto>();
            
            _mockBalanceClient
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(emptyProducts);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            
            _mockBalanceClient.Verify(x => x.GetProductsAsync(), Times.Once);
        }

        [Test]
        public async Task GetProductsAsync_WhenBalanceManagementThrowsExternalServiceException_ShouldPropagateException()
        {
            // Arrange
            var expectedException = new ExternalServiceException("Balance Management service is unavailable");
            
            _mockBalanceClient
                .Setup(x => x.GetProductsAsync())
                .ThrowsAsync(expectedException);

            // Act & Assert
            var act = async () => await _productService.GetProductsAsync();
            
            await act.Should().ThrowAsync<ExternalServiceException>()
               .WithMessage("Balance Management service is unavailable");
            
            _mockBalanceClient.Verify(x => x.GetProductsAsync(), Times.Once);
        }

        [Test]
        public async Task GetProductsAsync_WhenBalanceManagementThrowsGeneralException_ShouldPropagateException()
        {
            // Arrange
            var expectedException = new Exception("Unexpected error");
            
            _mockBalanceClient
                .Setup(x => x.GetProductsAsync())
                .ThrowsAsync(expectedException);

            // Act & Assert
            var act = async () => await _productService.GetProductsAsync();
            
            await act.Should().ThrowAsync<Exception>()
               .WithMessage("Unexpected error");
        }

        [Test]
        public async Task GetProductsAsync_ShouldCallBalanceManagementClientOnce()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Id = "test", Name = "Test Product", Price = 10.00m }
            };
            
            _mockBalanceClient
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(products);

            // Act
            await _productService.GetProductsAsync();

            // Assert
            _mockBalanceClient.Verify(x => x.GetProductsAsync(), Times.Once);
            _mockBalanceClient.VerifyNoOtherCalls();
        }

        [TearDown]
        public void TearDown()
        {
            _mockBalanceClient?.Reset();
        }
    }
} 