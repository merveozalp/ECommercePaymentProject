namespace ECommercePayment.Controllers
{
    using ECommercePayment.Application.DTOs;
    using ECommercePayment.Application.Interfaces;
    using ECommercePayment.Infrastructure.BalanceManagement;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        /// <summary>
        /// Create a new order and reserve funds through Balance Management
        /// </summary>
        /// <param name="request">Order creation request with product ID and quantity</param>
        /// <returns>Detailed order creation response with balance information</returns>
        /// <response code="200">Order created successfully with balance details</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="402">Insufficient balance for the order</response>
        /// <response code="404">Product not found</response>
        /// <response code="503">Balance Management service unavailable</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(BalanceManagementApiResponse<BalanceManagementPreorderData>), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 402)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 503)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var result = await _orderService.CreateOrderAsync(request);
            return Ok(result);
        }
        
        /// <summary>
        /// Complete an existing order by processing payment through Balance Management
        /// </summary>
        /// <param name="orderId">Unique order identifier</param>
        /// <returns>Completed order details</returns>
        /// <response code="200">Order completed successfully</response>
        /// <response code="404">Order not found</response>
        /// <response code="503">Balance Management service unavailable</response>
        [HttpPost("{orderId}/complete")]
        [ProducesResponseType(typeof(CompleteOrderResponse), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 503)]
        public async Task<IActionResult> Complete(string orderId)
        {
            var result = await _orderService.CompleteOrderAsync(orderId);
            return Ok(result);
        }
    }
} 