namespace CleanCode.Examples.ErrorHandling.Good;

public class GoodErrorHandlingExamples
{
    private readonly ILogger<GoodErrorHandlingExamples> _logger;
    private readonly IUserService _userService;
    private readonly IConfigService _configService;

    public GoodErrorHandlingExamples(ILogger<GoodErrorHandlingExamples> logger, IUserService userService, IConfigService configService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _configService = configService ?? throw new ArgumentNullException(nameof(configService));
    }

    // GOOD: Specific exception handling with proper logging and recovery
    public async Task<string> ReadFileWithProperHandling(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        try
        {
            _logger.LogInformation("Reading file: {FilePath}", filePath);
            var content = await File.ReadAllTextAsync(filePath);
            return content;
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError(ex, "File not found: {FilePath}", filePath);
            throw new FileProcessingException($"The file '{filePath}' was not found", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied to file: {FilePath}", filePath);
            throw new FileProcessingException($"Access denied to file '{filePath}'", ex);
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "IO error reading file: {FilePath}", filePath);
            throw new FileProcessingException($"Error reading file '{filePath}': {ex.Message}", ex);
        }
    }

    // GOOD: Validation before operations and specific exception types
    public async Task<UserDataResult> GetUserDataSafely(int userId)
    {
        if (userId <= 0)
            return UserDataResult.Invalid("User ID must be greater than zero");

        try
        {
            var user = await _userService.GetUserAsync(userId);
            if (user == null)
                return UserDataResult.NotFound($"User with ID {userId} not found");

            var profile = await _userService.GetUserProfileAsync(userId);
            return UserDataResult.Success($"{user.Name}: {profile?.Bio ?? "No bio available"}");
        }
        catch (DatabaseConnectionException ex)
        {
            _logger.LogError(ex, "Database connection failed while getting user {UserId}", userId);
            return UserDataResult.Failure("Service temporarily unavailable. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error getting user data for {UserId}", userId);
            return UserDataResult.Failure("An unexpected error occurred");
        }
    }

    // GOOD: Using Result pattern instead of exceptions for business logic
    public ValidationResult ValidateUser(User user)
    {
        var errors = new List<string>();

        if (user == null)
            return ValidationResult.Failed("User cannot be null");

        if (string.IsNullOrWhiteSpace(user.Name))
            errors.Add("Name is required");

        if (string.IsNullOrWhiteSpace(user.Email))
            errors.Add("Email is required");
        else if (!IsValidEmail(user.Email))
            errors.Add("Email format is invalid");

        if (user.Age < 0 || user.Age > 150)
            errors.Add("Age must be between 0 and 150");

        return errors.Any()
            ? ValidationResult.Failed(errors)
            : ValidationResult.Success();
    }

    // GOOD: Meaningful exception types with helpful messages
    public void ProcessOrderSafely(Order? order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order), "Order cannot be null");

        if (order.Items == null || order.Items.Count == 0)
            throw new InvalidOrderException("Order must contain at least one item");

        if (order.Total < 0)
            throw new InvalidOrderException($"Order total cannot be negative. Current total: {order.Total:C}");

        if (order.CustomerId <= 0)
            throw new InvalidOrderException("Order must have a valid customer ID");

        _logger.LogInformation("Processing order for customer {CustomerId}", order.CustomerId);
    }

    // GOOD: Fail-fast with proper input validation
    public decimal CalculatePriceSafely(string productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product ID cannot be null or empty", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Quantity must be greater than zero");

        try
        {
            var product = GetProduct(productId);
            if (product == null)
                throw new ProductNotFoundException($"Product with ID '{productId}' not found");

            var basePrice = product.BasePrice * quantity;
            var discountPercentage = GetDiscountPercentage(productId);

            // Safe division
            var discountMultiplier = discountPercentage / 100m;
            if (discountMultiplier < 0 || discountMultiplier > 1)
            {
                _logger.LogWarning("Invalid discount percentage {Discount} for product {ProductId}. Using 0%", discountPercentage, productId);
                discountMultiplier = 0;
            }

            return basePrice * (1 - discountMultiplier);
        }
        catch (ProductNotFoundException)
        {
            throw; // Re-throw specific exceptions
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating price for product {ProductId}, quantity {Quantity}", productId, quantity);
            throw new PriceCalculationException($"Failed to calculate price for product '{productId}'", ex);
        }
    }

    // GOOD: Proper error handling in batch operations
    public async Task<BatchProcessingResult> ProcessPaymentsBatch(List<Payment> payments)
    {
        if (payments == null)
            throw new ArgumentNullException(nameof(payments));

        var results = new List<PaymentResult>();
        var criticalErrors = new List<Exception>();

        foreach (var payment in payments)
        {
            try
            {
                await ProcessSinglePaymentSafely(payment);
                results.Add(PaymentResult.Success(payment.Id));
                _logger.LogInformation("Payment {PaymentId} processed successfully", payment.Id);
            }
            catch (PaymentValidationException ex)
            {
                // Business rule violation - log and continue
                _logger.LogWarning("Payment validation failed for payment {PaymentId}: {Error}", payment.Id, ex.Message);
                results.Add(PaymentResult.Failed(payment.Id, ex.Message));
            }
            catch (InsufficientFundsException ex)
            {
                // Expected business exception - log and continue
                _logger.LogInformation("Insufficient funds for payment {PaymentId}: {Error}", payment.Id, ex.Message);
                results.Add(PaymentResult.Failed(payment.Id, "Insufficient funds"));
            }
            catch (PaymentGatewayException ex)
            {
                // External service error - might be temporary
                _logger.LogError(ex, "Payment gateway error for payment {PaymentId}", payment.Id);
                results.Add(PaymentResult.Failed(payment.Id, "Payment service temporarily unavailable"));
            }
            catch (Exception ex)
            {
                // Unexpected error - might indicate system issue
                _logger.LogError(ex, "Critical error processing payment {PaymentId}", payment.Id);
                criticalErrors.Add(ex);
                results.Add(PaymentResult.Failed(payment.Id, "System error"));

                // Stop processing if too many critical errors
                if (criticalErrors.Count >= 3)
                {
                    _logger.LogError(ex, "Error message: {Error}", ex.Message);
                    break;
                }
            }
        }

        return new BatchProcessingResult(results, criticalErrors);
    }

    // GOOD: Defensive programming with proper validation
    public void TransferMoneySafely(string fromAccountId, string toAccountId, decimal amount)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(fromAccountId))
            throw new ArgumentException("From account ID cannot be null or empty", nameof(fromAccountId));

        if (string.IsNullOrWhiteSpace(toAccountId))
            throw new ArgumentException("To account ID cannot be null or empty", nameof(toAccountId));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Transfer amount must be positive");

        if (fromAccountId.Equals(toAccountId, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Cannot transfer money to the same account");

        try
        {
            var fromAccount = GetAccount(fromAccountId);
            var toAccount = GetAccount(toAccountId);

            if (fromAccount.Balance < amount)
                throw new InsufficientFundsException($"Account {fromAccountId} has insufficient funds. Balance: {fromAccount.Balance:C}, Required: {amount:C}");

            // Check for overflow
            if (toAccount.Balance > decimal.MaxValue - amount)
                throw new InvalidOperationException($"Transfer would cause balance overflow in account {toAccountId}");

            // Perform transfer
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            SaveAccount(fromAccount);
            SaveAccount(toAccount);

            _logger.LogInformation("Transfer completed: {Amount:C} from {FromAccount} to {ToAccount}", amount, fromAccountId, toAccountId);
        }
        catch (AccountNotFoundException ex)
        {
            _logger.LogError(ex, "Account not found during transfer");
            throw;
        }
        catch (InsufficientFundsException ex)
        {
            _logger.LogWarning("Transfer failed due to insufficient funds: {Error}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transfer failed: {Amount:C} from {FromAccount} to {ToAccount}", amount, fromAccountId, toAccountId);
            throw new TransferException($"Failed to transfer {amount:C} from {fromAccountId} to {toAccountId}", ex);
        }
    }

    // GOOD: Proper resource management with using statements
    public async Task<string> ReadFileWithProperResourceManagement(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        try
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(fileStream);
            return await reader.ReadToEndAsync();
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            _logger.LogError(ex, "File or directory not found: {FilePath}", filePath);
            throw new FileProcessingException($"File not found: {filePath}", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied: {FilePath}", filePath);
            throw new FileProcessingException($"Access denied: {filePath}", ex);
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "IO error reading file: {FilePath}", filePath);
            throw new FileProcessingException($"Error reading file: {filePath}", ex);
        }
    }

    // GOOD: Configuration with proper fallbacks and error handling
    public ConfigurationResult<T> GetConfigurationValue<T>(string key, T defaultValue = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            return ConfigurationResult<T>.Invalid("Configuration key cannot be null or empty");

        try
        {
            var value = _configService.GetValue<T>(key);
            return ConfigurationResult<T>.Success(value);
        }
        catch (ConfigurationNotFoundException ex)
        {
            _logger.LogWarning("Configuration key not found: {Key}. Using default value: {DefaultValue}", key, defaultValue);
            return ConfigurationResult<T>.Success(defaultValue);
        }
        catch (ConfigurationParseException ex)
        {
            _logger.LogError(ex, "Failed to parse configuration value for key: {Key}", key);
            return ConfigurationResult<T>.Invalid($"Invalid configuration format for key '{key}': {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving configuration for key: {Key}", key);
            return ConfigurationResult<T>.Failure("Configuration service temporarily unavailable");
        }
    }

    // GOOD: Consistent error handling pattern
    public async Task<OperationResult> DeleteUserSafely(int userId)
    {
        if (userId <= 0)
            return OperationResult.Invalid("User ID must be greater than zero");

        try
        {
            var exists = await _userService.UserExistsAsync(userId);
            if (!exists)
                return OperationResult.NotFound($"User with ID {userId} not found");

            await _userService.DeleteAsync(userId);
            _logger.LogInformation("User {UserId} deleted successfully", userId);
            return OperationResult.Success();
        }
        catch (UserDeletionException ex)
        {
            _logger.LogError(ex, "Failed to delete user {UserId}", userId);
            return OperationResult.Failed($"Failed to delete user: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting user {UserId}", userId);
            return OperationResult.Failed("An unexpected error occurred");
        }
    }

    // Supporting methods
    private bool IsValidEmail(string email) => email.Contains("@") && email.Contains(".");
    private Product GetProduct(string productId)
    {
        if (string.IsNullOrWhiteSpace(productId) || productId.Equals("invalid", StringComparison.OrdinalIgnoreCase))
            return null;
        return new Product { BasePrice = 10.0m };
    }
    private decimal GetDiscountPercentage(string productId) => 10m;
    private Account GetAccount(string accountId) => new Account { Balance = 1000m };
    private void SaveAccount(Account account) { }
    private async Task ProcessSinglePaymentSafely(Payment payment) => await Task.CompletedTask;
}

// GOOD: Result patterns for error handling without exceptions
public class UserDataResult
{
    public bool IsSuccess { get; private set; }
    public string Data { get; private set; }
    public string ErrorMessage { get; private set; }
    public UserDataResultType Type { get; private set; }

    private UserDataResult(bool isSuccess, string data, string errorMessage, UserDataResultType type)
    {
        IsSuccess = isSuccess;
        Data = data ?? string.Empty;
        ErrorMessage = errorMessage ?? string.Empty;
        Type = type;
    }

    public static UserDataResult Success(string data) => new(true, data, string.Empty, UserDataResultType.Success);
    public static UserDataResult NotFound(string message) => new(false, string.Empty, message, UserDataResultType.NotFound);
    public static UserDataResult Invalid(string message) => new(false, string.Empty, message, UserDataResultType.Invalid);
    public static UserDataResult Failure(string message) => new(false, string.Empty, message, UserDataResultType.Failure);
}

public enum UserDataResultType { Success, NotFound, Invalid, Failure }

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<string> Errors { get; }

    private ValidationResult(List<string> errors)
    {
        Errors = errors ?? new List<string>();
    }

    public static ValidationResult Success() => new(new List<string>());
    public static ValidationResult Failed(string error) => new(new List<string> { error });
    public static ValidationResult Failed(List<string> errors) => new(errors);
}

public class ConfigurationResult<T>
{
    public bool IsSuccess { get; private set; }
    public T Value { get; private set; }
    public string ErrorMessage { get; private set; }

    private ConfigurationResult(bool isSuccess, T value, string errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage ?? string.Empty;
    }

    public static ConfigurationResult<T> Success(T value) => new(true, value, string.Empty);
    public static ConfigurationResult<T> Invalid(string message) => new(false, default, message);
    public static ConfigurationResult<T> Failure(string message) => new(false, default, message);
}

public class OperationResult
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public OperationResultType Type { get; private set; }

    private OperationResult(bool isSuccess, string message, OperationResultType type)
    {
        IsSuccess = isSuccess;
        Message = message ?? string.Empty;
        Type = type;
    }

    public static OperationResult Success() => new(true, string.Empty, OperationResultType.Success);
    public static OperationResult NotFound(string message) => new(false, message, OperationResultType.NotFound);
    public static OperationResult Invalid(string message) => new(false, message, OperationResultType.Invalid);
    public static OperationResult Failed(string message) => new(false, message, OperationResultType.Failed);
}

public enum OperationResultType { Success, NotFound, Invalid, Failed }

public class BatchProcessingResult
{
    public List<PaymentResult> Results { get; }
    public List<Exception> CriticalErrors { get; }
    public bool HasCriticalErrors => CriticalErrors.Any();

    public BatchProcessingResult(List<PaymentResult> results, List<Exception> criticalErrors)
    {
        Results = results ?? new List<PaymentResult>();
        CriticalErrors = criticalErrors ?? new List<Exception>();
    }
}

public class PaymentResult
{
    public string PaymentId { get; private set; }
    public bool IsSuccess { get; private set; }
    public string ErrorMessage { get; private set; }

    private PaymentResult(string paymentId, bool isSuccess, string errorMessage)
    {
        PaymentId = paymentId ?? string.Empty;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage ?? string.Empty;
    }

    public static PaymentResult Success(string paymentId) => new(paymentId, true, string.Empty);
    public static PaymentResult Failed(string paymentId, string errorMessage) => new(paymentId, false, errorMessage);
}

// Supporting classes and exceptions
public class User
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class UserProfile
{
    public string Bio { get; set; } = string.Empty;
}

public class Product
{
    public decimal BasePrice { get; set; }
}

public class Order
{
    public string Id { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }
}

public class OrderItem { }

public class Payment
{
    public string Id { get; set; } = string.Empty;
}

public class Account
{
    public decimal Balance { get; set; }
}

// Custom exceptions with meaningful names and messages
public class FileProcessingException : Exception
{
    public FileProcessingException(string message) : base(message) { }
    public FileProcessingException(string message, Exception innerException) : base(message, innerException) { }
}

public class InvalidOrderException : Exception
{
    public InvalidOrderException(string message) : base(message) { }
}

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(string message) : base(message) { }
}

public class PriceCalculationException : Exception
{
    public PriceCalculationException(string message, Exception innerException) : base(message, innerException) { }
}

public class PaymentValidationException : Exception
{
    public PaymentValidationException(string message) : base(message) { }
}

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string message) : base(message) { }
}

public class PaymentGatewayException : Exception
{
    public PaymentGatewayException(string message) : base(message) { }
}

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(string message) : base(message) { }
}

public class TransferException : Exception
{
    public TransferException(string message, Exception innerException) : base(message, innerException) { }
}

public class DatabaseConnectionException : Exception
{
    public DatabaseConnectionException(string message) : base(message) { }
}

public class ConfigurationNotFoundException : Exception
{
    public ConfigurationNotFoundException(string message) : base(message) { }
}

public class ConfigurationParseException : Exception
{
    public ConfigurationParseException(string message, Exception innerException) : base(message, innerException) { }
}

public class UserDeletionException : Exception
{
    public UserDeletionException(string message) : base(message) { }
}

// Service interfaces
public interface ILogger<T>
{
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(Exception exception, string message, params object[] args);
}

public interface IUserService
{
    Task<User> GetUserAsync(int userId);
    Task<UserProfile> GetUserProfileAsync(int userId);
    Task<bool> UserExistsAsync(int userId);
    Task DeleteAsync(int userId);
}

public interface IConfigService
{
    T GetValue<T>(string key);
}

// Simple mock implementations for compilation
public class MockLogger<T> : ILogger<T>
{
    public void LogInformation(string message, params object[] args)
    {
        try
        {
            Console.WriteLine($"INFO: {string.Format(message, args)}");
        }
        catch
        {
            Console.WriteLine($"INFO: {message}");
        }
    }

    public void LogWarning(string message, params object[] args)
    {
        try
        {
            Console.WriteLine($"WARN: {string.Format(message, args)}");
        }
        catch
        {
            Console.WriteLine($"WARN: {message}");
        }
    }

    public void LogError(Exception exception, string message, params object[] args)
    {
        try
        {
            Console.WriteLine($"ERROR: {string.Format(message, args)} - {exception?.Message}");
        }
        catch
        {
            Console.WriteLine($"ERROR: {message} - {exception?.Message}");
        }
    }
}

public class MockUserService : IUserService
{
    public async Task<User> GetUserAsync(int userId)
    {
        await Task.CompletedTask;
        return userId > 0 ? new User { Name = "John Doe", Email = "john@example.com" } : null;
    }

    public async Task<UserProfile> GetUserProfileAsync(int userId)
    {
        await Task.CompletedTask;
        return new UserProfile { Bio = "Software Developer" };
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        await Task.CompletedTask;
        return userId > 0;
    }

    public async Task DeleteAsync(int userId) =>
        await Task.CompletedTask;
}

public class MockConfigService : IConfigService
{
    public T GetValue<T>(string key)
    {
        if (typeof(T) == typeof(int))
            return (T)(object)30;
        return (T)(object)"mock_value";
    }
}