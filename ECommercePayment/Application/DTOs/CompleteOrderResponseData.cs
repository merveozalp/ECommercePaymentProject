namespace ECommercePayment.Application.DTOs
{
    /// <summary>
    /// Complete Order Response Data
    /// </summary>
    public class CompleteOrderResponseData
    {
        /// <summary>
        /// CompletedOrderDto
        /// </summary>
        public CompletedOrderDto Order { get; set; } = new();

        /// <summary>
        /// UpdatedBalanceDto
        /// </summary>
        public UpdatedBalanceDto UpdatedBalance { get; set; } = new();
    }
}
