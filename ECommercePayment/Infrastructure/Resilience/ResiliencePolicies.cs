namespace ECommercePayment.Infrastructure.Resilience
{
    using Polly;
    using Polly.Extensions.Http;

    /// <summary>
    /// Resilience policies for external service calls
    /// </summary>
    public static class ResiliencePolicies
    {
        /// <summary>
        /// Retry policy with exponential backoff for Balance Management service
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() 
                .OrResult(msg => !msg.IsSuccessStatusCode) 
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => 
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // 2^1=2s, 2^2=4s, 2^3=8s
                        + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 1000)), 
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        // Retry logic executed - could add logging here if needed
                    });
        }

        /// <summary>
        /// Circuit breaker policy for Balance Management service
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5, // 5 consecutive failures
                    durationOfBreak: TimeSpan.FromSeconds(30), // 30 seconds break
                    onBreak: (exception, duration) =>
                    {
                        // Circuit breaker opened - could add logging here if needed
                    },
                    onReset: () =>
                    {
                        // Circuit breaker reset - could add logging here if needed
                    });
        }

        /// <summary>
        /// Timeout policy for Balance Management service
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(
                timeout: TimeSpan.FromSeconds(10) // 10 seconds per request
            );
        }

        /// <summary>
        /// Combined policy: Timeout + Retry + Circuit Breaker
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            var timeout = GetTimeoutPolicy();
            var retry = GetRetryPolicy();
            var circuitBreaker = GetCircuitBreakerPolicy();

            // Policy execution order: Timeout -> Retry -> Circuit Breaker
            return Policy.WrapAsync(retry, circuitBreaker, timeout);
        }
    }
} 