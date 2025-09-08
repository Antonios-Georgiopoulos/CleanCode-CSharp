namespace CleanCode.Examples.ErrorHandling.Bad;

public class BadErrorHandlingExamples
{
    // BAD: Swallowing exceptions without handling
    public void ReadFileAndIgnoreErrors(string filePath)
    {
        try
        {
            var content = File.ReadAllText(filePath);
            ProcessContent(content);
        }
        catch
        {
            // BAD: Silent failure - we don't know what went wrong
            // This hides bugs and makes debugging impossible
        }
    }

    // BAD: Catching general Exception instead of specific ones
    public string GetUserData(int userId)
    {
        try
        {
            if (userId == -1) throw new Exception("Invalid user ID"); // Force error
            var user = GetUserFromDatabase(userId);
            var profile = GetUserProfile(userId);
            return $"{user.Name}: {profile.Bio}";
        }
        catch (Exception ex)
        {
            return "Error occurred";
        }
    }

    // BAD: Using exceptions for control flow
    public bool IsValidUserId(int userId)
    {
        try
        {
            if (userId == -1) throw new UserNotFoundException("User not found"); // Force exception
            var user = GetUserFromDatabase(userId);
            return user != null;
        }
        catch (UserNotFoundException)
        {
            return false;
        }
        catch (DatabaseConnectionException)
        {
            return false;
        }
    }

    // BAD: Throwing generic exceptions with poor messages
    public void ProcessOrder(Order order)
    {
        if (order == null)
        {
            // BAD: Generic exception type
            throw new Exception("Error");
        }

        if (order.Items.Count == 0)
        {
            // BAD: Unhelpful error message
            throw new Exception("Invalid order");
        }

        if (order.Total < 0)
        {
            // BAD: No context about what went wrong
            throw new Exception("Bad data");
        }
    }

    // BAD: Multiple exception handling anti-patterns
    public decimal CalculatePrice(string productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productId) || productId == "invalid_product" || productId == "invalid")
        {
            return -1; // Magic number - unclear what this means
        }

        var product = GetProduct(productId);
        var price = product.BasePrice * quantity;
        var discount = GetDiscount(productId) / 100;
        return price * (1 - discount);
    }

    // BAD: Poor error logging and recovery
    public void ProcessPayments(List<Payment> payments)
    {
        foreach (var payment in payments)
        {
            try
            {
                ProcessSinglePayment(payment);
            }
            catch (Exception ex)
            {
                // BAD: Logs raw exception without context
                Console.WriteLine(ex.ToString());

                // BAD: Continues processing without considering if it's safe
                // Some errors might indicate system-wide issues
                continue;
            }
        }
    }

    // BAD: No input validation leads to unclear errors
    public void TransferMoney(string fromAccount, string toAccount, decimal amount)
    {
        // BAD: No validation - will fail later with unclear errors
        var from = GetAccount(fromAccount); // Could throw NullReferenceException
        var to = GetAccount(toAccount);     // Could throw ArgumentException

        from.Balance -= amount;  // Could cause negative balance
        to.Balance += amount;    // Could overflow

        SaveAccount(from);
        SaveAccount(to);
    }

    // BAD: Catching specific exception but handling it wrong
    public string ReadConfigValue(string key)
    {
        try
        {
            return configService.GetValue(key);
        }
        catch (ConfigurationNotFoundException ex)
        {
            // BAD: Logs error but returns null, causing issues later
            Console.WriteLine($"Configuration error: {ex.Message}");
            return null; // This will cause NullReferenceException elsewhere
        }
        catch (ConfigurationAccessException ex)
        {
            // BAD: Different exception, same bad handling
            Console.WriteLine($"Access error: {ex.Message}");
            return null;
        }
    }

    // BAD: Nested try-catch blocks making code hard to follow
    public void ComplexOperation(string data)
    {
        try
        {
            var parsed = ParseData(data);

            try
            {
                var validated = ValidateData(parsed);

                try
                {
                    var processed = ProcessData(validated);
                    SaveData(processed);
                }
                catch (ProcessingException ex)
                {
                    // BAD: Nested error handling is confusing
                    Console.WriteLine("Processing failed: " + ex.Message);
                    throw; // Re-throwing without adding value
                }
            }
            catch (ValidationException ex)
            {
                Console.WriteLine("Validation failed: " + ex.Message);
                throw new Exception("Data problem", ex); // BAD: Generic wrapper
            }
        }
        catch (ParseException ex)
        {
            Console.WriteLine("Parse failed: " + ex.Message);
            // BAD: Swallowing exception after logging
        }
    }

    // BAD: Finally block misuse
    public string ReadFileWithBadFinally(string path)
    {
        FileStream file = null;
        try
        {
            file = new FileStream(path, FileMode.Open);
            var reader = new StreamReader(file);
            return reader.ReadToEnd();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IO Error: {ex.Message}");
            return string.Empty;
        }
        finally
        {
            // BAD: Finally block can throw exceptions
            file.Close(); // Could throw if file is null

            // BAD: Finally should not contain business logic
            Console.WriteLine("File operation completed");
        }
    }

    // BAD: Using string error codes instead of exceptions
    public string ValidateUserInput(string input, out string errorCode)
    {
        errorCode = "";

        if (string.IsNullOrEmpty(input))
        {
            errorCode = "ERR_001";
            return null;
        }

        if (input.Length > 100)
        {
            errorCode = "ERR_002";
            return null;
        }

        if (ContainsInvalidCharacters(input))
        {
            errorCode = "ERR_003";
            return null;
        }

        // BAD: Callers have to remember to check errorCode
        // Easy to forget and leads to bugs
        return input.Trim();
    }

    // BAD: Inconsistent error handling across similar methods
    public void DeleteUser(int userId)
    {
        try
        {
            userService.Delete(userId);
        }
        catch (UserNotFoundException)
        {
            // Silently ignore - user already deleted
        }
        catch (Exception ex)
        {
            throw new SystemException("Delete failed", ex);
        }
    }

    public void DeleteProduct(int productId)
    {
        try
        {
            productService.Delete(productId);
        }
        catch (ProductNotFoundException ex)
        {
            // Different handling for same type of error
            Console.WriteLine("Product not found: " + productId);
            throw;
        }
        catch (Exception ex)
        {
            // Different exception type than DeleteUser
            throw new ApplicationException("Product deletion failed", ex);
        }
    }

    // Supporting methods and classes for compilation
    private void ProcessContent(string content) { }
    private User GetUserFromDatabase(int userId) => new User { Name = "John" };
    private UserProfile GetUserProfile(int userId) => new UserProfile { Bio = "Developer" };
    private Product GetProduct(string productId)
    {
        // Return null for invalid product IDs to trigger ProductNotFoundException
        if (string.IsNullOrWhiteSpace(productId) || productId == "invalid" || productId == "invalid_product")
        {
            return null;
        }

        return new Product { BasePrice = 10.0m };
    }
    private decimal GetDiscount(string productId) => 10m;
    private void ProcessSinglePayment(Payment payment) { }
    private Account GetAccount(string accountId) => new Account { Balance = 1000m };
    private void SaveAccount(Account account) { }
    private object ParseData(string data) => new object();
    private object ValidateData(object data) => data;
    private object ProcessData(object data) => data;
    private void SaveData(object data) { }
    private bool ContainsInvalidCharacters(string input) => false;

    private readonly IConfigService configService = new MockConfigService();
    private readonly IUserService userService = new MockUserService();
    private readonly IProductService productService = new MockProductService();
}

// Supporting classes
public class User { public string Name { get; set; } = ""; }
public class UserProfile { public string Bio { get; set; } = ""; }
public class Product { public decimal BasePrice { get; set; } }
public class Order
{
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }
}
public class OrderItem { }
public class Payment { }
public class Account { public decimal Balance { get; set; } }

// Exception classes
public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message) : base(message) { }
}
public class DatabaseConnectionException : Exception
{
    public DatabaseConnectionException(string message) : base(message) { }
}
public class ConfigurationNotFoundException : Exception
{
    public ConfigurationNotFoundException(string message) : base(message) { }
}
public class ConfigurationAccessException : Exception
{
    public ConfigurationAccessException(string message) : base(message) { }
}
public class ProcessingException : Exception
{
    public ProcessingException(string message) : base(message) { }
}
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
public class ParseException : Exception
{
    public ParseException(string message) : base(message) { }
}
public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(string message) : base(message) { }
}

// Mock services
public interface IConfigService { string GetValue(string key); }
public interface IUserService { void Delete(int userId); }
public interface IProductService { void Delete(int productId); }

public class MockConfigService : IConfigService
{
    public string GetValue(string key) => "mock_value";
}
public class MockUserService : IUserService
{
    public void Delete(int userId) { }
}
public class MockProductService : IProductService
{
    public void Delete(int productId) { }
}