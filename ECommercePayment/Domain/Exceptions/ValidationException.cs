namespace ECommercePayment.Domain.Exceptions
{
    /// <summary>
    /// Validation Exception
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
} 