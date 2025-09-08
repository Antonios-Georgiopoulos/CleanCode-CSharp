using System.Text.RegularExpressions;

namespace CleanCode.Examples.Comments.Good;

public partial class GoodCommentExamples
{
    // GOOD: Explaining business logic or complex algorithms
    public decimal CalculateCompoundInterest(decimal principal, decimal rate, int years)
    {
        // Using the compound interest formula: A = P(1 + r)^t
        // Where P = principal, r = annual rate, t = time in years
        return principal * (decimal)Math.Pow((double)(1 + rate), years);
    }

    // GOOD: Warning about important constraints or side effects
    public void ProcessLargeDataset(List<DataItem> items)
    {
        // WARNING: This method loads all items into memory.
        // For datasets > 10,000 items, consider using ProcessLargeDatasetStreaming() instead.

        var processedItems = items.Select(ProcessItem).ToList();
        SaveToDatabase(processedItems);
    }

    // GOOD: Explaining non-obvious implementation decisions
    public string GenerateUniqueId()
    {
        // Using Guid.NewGuid() instead of timestamp to avoid collisions
        // in high-concurrency scenarios where multiple IDs might be generated
        // within the same millisecond
        return Guid.NewGuid().ToString("N")[..8];
    }

    // GOOD: Legal or compliance requirements
    public void LogUserActivity(string userId, string activity)
    {
        // Required by GDPR: Personal data must be anonymized in logs
        var anonymizedUserId = HashUserId(userId);

        // Retention policy: Activity logs are automatically purged after 90 days
        logger.LogActivity(anonymizedUserId, activity, DateTime.UtcNow);
    }

    // GOOD: API documentation using XML comments
    /// <summary>
    /// Validates an email address according to RFC 5322 standards.
    /// </summary>
    /// <param name="email">The email address to validate</param>
    /// <returns>True if the email is valid; otherwise, false</returns>
    /// <exception cref="ArgumentNullException">Thrown when email is null</exception>
    /// <example>
    /// <code>
    /// bool isValid = ValidateEmail("user@example.com"); // Returns true
    /// bool isInvalid = ValidateEmail("invalid-email");  // Returns false
    /// </code>
    /// </example>
    public bool ValidateEmail(string email)
    {
        if (email == null)
            throw new ArgumentNullException(nameof(email));

        return EmailRegex().IsMatch(email);
    }

    // GOOD: Explaining complex regular expressions
    [GeneratedRegex(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    // GOOD: Clarifying units or expected ranges
    public void SetCacheExpiration(int seconds)
    {
        // Cache expiration must be between 60 seconds (1 minute) and 86400 seconds (24 hours)
        if (seconds < 60 || seconds > 86400)
            throw new ArgumentOutOfRangeException(nameof(seconds),
                "Cache expiration must be between 60 and 86400 seconds");

        cacheExpirationSeconds = seconds;
    }

    // GOOD: Explaining why certain approach was chosen
    public async Task<ApiResponse> CallExternalApiAsync(string endpoint)
    {
        // Using exponential backoff for retries because the external API
        // has rate limiting and temporary failures are common

        var maxRetries = 3;
        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                return await httpClient.GetAsync(endpoint);
            }
            catch (HttpRequestException) when (attempt < maxRetries - 1)
            {
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
            }
        }

        throw new HttpRequestException("Failed after all retry attempts");
    }

    // GOOD: Documenting known limitations or future improvements
    public List<SearchResult> SearchProducts(string query)
    {
        // Current implementation uses simple string matching.
        // TODO: Replace with Elasticsearch for better performance and fuzzy matching.
        // See ticket PROD-1234 for implementation details.

        return products
            .Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Take(50) // Limit to 50 results to prevent performance issues
            .Select(p => new SearchResult()) // Map Product to SearchResult
            .ToList();
    }

    // GOOD: Self-documenting code that minimizes need for comments
    public class Order
    {
        public bool IsEligibleForFreeShipping => Total >= FreeShippingThreshold;
        public bool RequiresSignature => ContainsHighValueItems || ContainsRestrictedItems;
        public decimal TotalWithTax => Total + CalculateTax();

        private const decimal FreeShippingThreshold = 50m;

        private bool ContainsHighValueItems => Items.Any(item => item.Value > 500m);
        private bool ContainsRestrictedItems => Items.Any(item => item.IsRestricted);

        public decimal Total { get; set; }
        public List<OrderItem> Items { get; private set; } = new();

        private decimal CalculateTax()
        {
            // Tax calculation varies by shipping address
            return ShippingAddress.State switch
            {
                "CA" => Total * 0.0875m,  // California sales tax
                "NY" => Total * 0.08m,    // New York sales tax
                "TX" => Total * 0.0625m,  // Texas sales tax
                _ => 0m                   // No tax for other states
            };
        }

        public Address ShippingAddress { get; set; } = new();
    }

    // GOOD: Interface documentation that explains contracts
    /// <summary>
    /// Represents a service for sending notifications to users.
    /// Implementations must be thread-safe and handle failures gracefully.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Sends a notification to the specified recipient.
        /// </summary>
        /// <param name="recipient">The notification recipient (email, phone, etc.)</param>
        /// <param name="message">The message content</param>
        /// <param name="priority">Priority level affecting delivery speed and retry logic</param>
        /// <returns>True if notification was queued successfully; false otherwise</returns>
        /// <remarks>
        /// This method is non-blocking and queues notifications for background processing.
        /// High priority notifications are delivered within 1 minute.
        /// Normal priority notifications are delivered within 5 minutes.
        /// </remarks>
        Task<bool> SendNotificationAsync(string recipient, string message, NotificationPriority priority = NotificationPriority.Normal);
    }

    // GOOD: Comments that add value by explaining intent
    public void ProcessPayment(PaymentRequest request)
    {
        // Validate payment before processing to fail fast and reduce processing costs
        ValidatePaymentRequest(request);

        // Lock account temporarily to prevent double-charging during payment processing
        using var accountLock = AcquireAccountLock(request.AccountId);

        try
        {
            var result = paymentProcessor.ProcessPayment(request);

            // Update account balance immediately for real-time balance checks
            UpdateAccountBalance(request.AccountId, result.Amount);

            // Audit log required for financial compliance (SOX, PCI-DSS)
            auditLogger.LogPayment(request.AccountId, result.TransactionId, result.Amount);
        }
        catch (PaymentException ex) when (ex.IsRetryable)
        {
            // Queue for retry - payment processor may be temporarily unavailable
            paymentRetryQueue.Enqueue(request);
        }
    }

    // GOOD: Explaining performance considerations
    public async Task<List<Product>> GetRecommendedProductsAsync(int userId)
    {
        // Cache user recommendations for 1 hour to reduce database load
        // Recommendation calculation is expensive (ML model inference)
        var cacheKey = $"recommendations:user:{userId}";

        if (cache.TryGetValue(cacheKey, out var cachedProducts))
        {
            return (List<Product>)cachedProducts;
        }

        var recommendations = await recommendationEngine.GetRecommendationsAsync(userId);
        cache[cacheKey] = recommendations;
        return recommendations;
    }

    // GOOD: Minimal comments for complex but well-named code
    public ValidationResult ValidateCustomerData(Customer customer)
    {
        var validators = new IValidator<Customer>[]
        {
            new RequiredFieldsValidator(),
            new EmailFormatValidator(),
            new PhoneNumberValidator(),
            new CreditScoreValidator(),
            new GeographicRestrictionValidator()
        };

        return validators
            .Select(validator => validator.Validate(customer))
            .Aggregate(ValidationResult.Success(), (current, result) => current.Combine(result));
    }

    // Supporting types and members
    private readonly ILogger logger = new ConsoleLogger();
    private readonly IHttpClient httpClient = new HttpClientWrapper();
    private readonly List<Product> products = new();
    private readonly IPaymentProcessor paymentProcessor = new MockPaymentProcessor();
    private readonly IAuditLogger auditLogger = new MockAuditLogger();
    private readonly IPaymentRetryQueue paymentRetryQueue = new MockRetryQueue();
    private readonly IRecommendationEngine recommendationEngine = new MockRecommendationEngine();
    private readonly Dictionary<string, object> cache = new();
    private int cacheExpirationSeconds = 3600;

    private string HashUserId(string userId) => userId.GetHashCode().ToString();
    private DataItem ProcessItem(DataItem item) => item;
    private void SaveToDatabase(List<DataItem> items) { }
    private IDisposable AcquireAccountLock(int accountId) => new MockLock();
    private void ValidatePaymentRequest(PaymentRequest request) { }
    private void UpdateAccountBalance(int accountId, decimal amount) { }
}

// Supporting types
public enum NotificationPriority { Normal, High }
public class DataItem { }
public class ApiResponse { }
public class PaymentRequest 
{ 
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
}
public class SearchResult { }
public class OrderItem
{
    public decimal Value { get; set; }
    public bool IsRestricted { get; set; }
}
public class Address { public string State { get; set; } = ""; }
public class PaymentException : Exception
{
    public bool IsRetryable { get; set; }
    public PaymentException(string message) : base(message) { }
}
public class Customer { }
public class ValidationResult
{
    public static ValidationResult Success() => new();
    public ValidationResult Combine(ValidationResult other) => this;
}
public class Product
{
    public string Name { get; set; } = "";
}

// Mock interfaces and implementations
public interface ILogger { void LogActivity(string userId, string activity, DateTime timestamp); }
public interface IHttpClient { Task<ApiResponse> GetAsync(string endpoint); }
public interface IValidator<T> { ValidationResult Validate(T item); }
public interface IPaymentProcessor { PaymentResult ProcessPayment(PaymentRequest request); }
public interface IAuditLogger { void LogPayment(int accountId, string transactionId, decimal amount); }
public interface IPaymentRetryQueue { void Enqueue(PaymentRequest request); }
public interface IRecommendationEngine { Task<List<Product>> GetRecommendationsAsync(int userId); }

public class PaymentResult
{
    public string TransactionId { get; set; } = "";
    public decimal Amount { get; set; }
}

public class ConsoleLogger : ILogger
{
    public void LogActivity(string userId, string activity, DateTime timestamp) =>
        Console.WriteLine($"{timestamp}: {userId} - {activity}");
}

public class HttpClientWrapper : IHttpClient
{
    public async Task<ApiResponse> GetAsync(string endpoint)
    {
        await Task.CompletedTask;
        return new ApiResponse();
    }
}

public class RequiredFieldsValidator : IValidator<Customer>
{
    public ValidationResult Validate(Customer item) => ValidationResult.Success();
}

public class EmailFormatValidator : IValidator<Customer>
{
    public ValidationResult Validate(Customer item) => ValidationResult.Success();
}

public class PhoneNumberValidator : IValidator<Customer>
{
    public ValidationResult Validate(Customer item) => ValidationResult.Success();
}

public class CreditScoreValidator : IValidator<Customer>
{
    public ValidationResult Validate(Customer item) => ValidationResult.Success();
}

public class GeographicRestrictionValidator : IValidator<Customer>
{
    public ValidationResult Validate(Customer item) => ValidationResult.Success();
}

public class MockPaymentProcessor : IPaymentProcessor
{
    public PaymentResult ProcessPayment(PaymentRequest request) =>
        new() { TransactionId = "TXN123", Amount = request.Amount };
}

public class MockAuditLogger : IAuditLogger
{
    public void LogPayment(int accountId, string transactionId, decimal amount) { }
}

public class MockRetryQueue : IPaymentRetryQueue
{
    public void Enqueue(PaymentRequest request) { }
}

public class MockRecommendationEngine : IRecommendationEngine
{
    public async Task<List<Product>> GetRecommendationsAsync(int userId)
    {
        await Task.CompletedTask;
        return new List<Product>();
    }
}

public class MockLock : IDisposable
{
    public void Dispose() { }
}