namespace ECommercePayment.Controllers
{
    using ECommercePayment.Application.DTOs;
    using ECommercePayment.Application.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all available products
        /// </summary>
        /// <returns>List of products with pricing information</returns>
        /// <response code="200">Returns the list of products</response>
        /// <response code="503">If Balance Management service is unavailable</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        [ProducesResponseType(typeof(object), 503)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }
    }
} 