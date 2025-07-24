namespace ECommercePayment.Application.ServiceError
{
    /// <summary>
    /// Predefined service errors for consistent error handling across the application
    /// </summary>
    public static class ServiceErrors
    {
        /// <summary>
        /// Order-related service errors
        /// </summary>
        public static class Order
        {
            /// <summary>
            /// Insufficient balance to complete the order
            /// </summary>
            public static ServiceError InsufficientBalance => new(
                "ORDER_INSUFFICIENT_BALANCE",
                "Insufficient balance to complete the order",
                402,
                "Payment"
            );

            /// <summary>
            /// Product not found
            /// </summary>
            public static ServiceError ProductNotFound => new(
                "ORDER_PRODUCT_NOT_FOUND",
                "Product not found",
                404,
                "Validation"
            );

            /// <summary>
            /// Insufficient stock available
            /// </summary>
            public static ServiceError InsufficientStock => new(
                "ORDER_INSUFFICIENT_STOCK",
                "Insufficient stock available for the requested quantity",
                400,
                "Validation"
            );

            /// <summary>
            /// External service unavailable
            /// </summary>
            public static ServiceError ExternalServiceUnavailable => new(
                "ORDER_EXTERNAL_SERVICE_ERROR",
                "Balance management service is currently unavailable",
                503,
                "ExternalService"
            );

            /// <summary>
            /// Invalid order request
            /// </summary>
            public static ServiceError InvalidRequest => new(
                "ORDER_INVALID_REQUEST",
                "Invalid order request data",
                400,
                "Validation"
            );

            /// <summary>
            /// Order not found
            /// </summary>
            public static ServiceError OrderNotFound => new(
                "ORDER_NOT_FOUND",
                "Order not found",
                404,
                "Validation"
            );

            /// <summary>
            /// Order in invalid status for operation
            /// </summary>
            public static ServiceError InvalidOrderStatus => new(
                "ORDER_INVALID_STATUS",
                "Order is in invalid status for this operation",
                400,
                "Business"
            );

            /// <summary>
            /// Maximum order quantity exceeded
            /// </summary>
            public static ServiceError MaxQuantityExceeded => new(
                "ORDER_MAX_QUANTITY_EXCEEDED",
                "Order quantity exceeds maximum allowed limit",
                400,
                "Business"
            );

            /// <summary>
            /// Order completion failed
            /// </summary>
            public static ServiceError CompletionFailed => new(
                "ORDER_COMPLETION_FAILED",
                "Failed to complete the order",
                500,
                "Processing"
            );

            /// <summary>
            /// Order cancellation failed
            /// </summary>
            public static ServiceError CancellationFailed => new(
                "ORDER_CANCELLATION_FAILED",
                "Failed to cancel the order",
                500,
                "Processing"
            );
        }

        /// <summary>
        /// Product-related service errors
        /// </summary>
        public static class Product
        {
            /// <summary>
            /// Product not found
            /// </summary>
            public static ServiceError NotFound => new(
                "PRODUCT_NOT_FOUND",
                "Product not found",
                404,
                "Validation"
            );

            /// <summary>
            /// Product service unavailable
            /// </summary>
            public static ServiceError ServiceUnavailable => new(
                "PRODUCT_SERVICE_UNAVAILABLE",
                "Product service is currently unavailable",
                503,
                "ExternalService"
            );

            /// <summary>
            /// Failed to fetch products
            /// </summary>
            public static ServiceError FetchFailed => new(
                "PRODUCT_FETCH_FAILED",
                "Failed to fetch product information",
                500,
                "Processing"
            );
        }

        /// <summary>
        /// General system errors
        /// </summary>
        public static class System
        {
            /// <summary>
            /// Internal server error
            /// </summary>
            public static ServiceError InternalError => new(
                "SYSTEM_INTERNAL_ERROR",
                "An internal server error occurred",
                500,
                "System"
            );

            /// <summary>
            /// Database connection error
            /// </summary>
            public static ServiceError DatabaseError => new(
                "SYSTEM_DATABASE_ERROR",
                "Database connection error",
                500,
                "System"
            );

            /// <summary>
            /// Operation timeout
            /// </summary>
            public static ServiceError Timeout => new(
                "SYSTEM_TIMEOUT",
                "Operation timed out",
                408,
                "System"
            );

            /// <summary>
            /// Resource not available
            /// </summary>
            public static ServiceError ResourceUnavailable => new(
                "SYSTEM_RESOURCE_UNAVAILABLE",
                "Required resource is currently unavailable",
                503,
                "System"
            );
        }

        /// <summary>
        /// Validation-related errors
        /// </summary>
        public static class Validation
        {
            /// <summary>
            /// Required field missing
            /// </summary>
            public static ServiceError RequiredFieldMissing => new(
                "VALIDATION_REQUIRED_FIELD_MISSING",
                "Required field is missing",
                400,
                "Validation"
            );

            /// <summary>
            /// Invalid format
            /// </summary>
            public static ServiceError InvalidFormat => new(
                "VALIDATION_INVALID_FORMAT",
                "Invalid data format",
                400,
                "Validation"
            );

            /// <summary>
            /// Value out of range
            /// </summary>
            public static ServiceError ValueOutOfRange => new(
                "VALIDATION_VALUE_OUT_OF_RANGE",
                "Value is out of acceptable range",
                400,
                "Validation"
            );
        }
    }
} 