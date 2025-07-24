namespace ECommercePayment.Domain.Exceptions
{
    /// <summary>
    /// Order Exception
    /// </summary>
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(string message) : base(message) { }
    }
} 