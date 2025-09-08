namespace CleanCode.Examples.CodeFormatting.Good;

// GOOD: Consistent indentation, spacing, and formatting
public class GoodFormattingExamples
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool IsActive { get; set; }

    // GOOD: Proper spacing around operators and consistent brace placement
    public void ProcessData(List<string> data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        for (int i = 0; i < data.Count; i++)
        {
            var item = data[i];
            if (item != null && item.Length > 0)
            {
                Console.WriteLine($"Processing: {item}");
            }
        }
    }

    // GOOD: Breaking long lines for readability
    public void ProcessUserOrder(
        string userName,
        string userEmail,
        List<OrderItem> orderItems,
        PaymentMethod paymentMethod,
        ShippingAddress shippingAddress,
        BillingAddress billingAddress,
        DiscountCoupon discountCoupon,
        TaxCalculationRules taxRules)
    {
        if (IsValidOrderData(userName, userEmail, orderItems, paymentMethod, shippingAddress, billingAddress))
        {
            var subtotal = CalculateSubtotal(orderItems);
            var discountAmount = CalculateDiscount(discountCoupon, subtotal);
            var taxAmount = CalculateTax(taxRules, subtotal - discountAmount, shippingAddress.State);
            var total = subtotal - discountAmount + taxAmount;

            ProcessPayment(total, paymentMethod);
        }
    }

    // GOOD: Consistent method spacing with logical grouping
    public void Method1()
    {
        Console.WriteLine("Method 1");
    }

    public void Method2()
    {
        Console.WriteLine("Method 2");
    }

    public void Method3()
    {
        Console.WriteLine("Method 3");
    }

    // GOOD: Well-indented nested code with clear structure
    public void ProcessNestedData()
    {
        var users = GetUsers();

        foreach (var user in users)
        {
            var orders = GetUserOrders(user.Id);

            foreach (var order in orders)
            {
                var items = GetOrderItems(order.Id);

                foreach (var item in items)
                {
                    ProcessOrderItem(item, user);
                }
            }
        }
    }

    // GOOD: Consistent commenting style
    public void GoodCommentFormatting()
    {
        // This is a properly formatted single-line comment
        // Multiple single-line comments are aligned consistently

        /*
         * This is a properly formatted
         * multi-line block comment
         * with consistent indentation
         */

        var x = 5; // Inline comment with proper spacing
        var y = 10; // Another inline comment with consistent spacing
    }

    // GOOD: Consistent string formatting using interpolation
    public void GoodStringFormatting()
    {
        var name = "John";
        var age = 30;
        var email = "john@example.com";

        // Consistent use of string interpolation
        var message1 = $"Hello {name}, you are {age} years old";
        var message2 = $"Hello {name}";
        var message3 = $"Your email is {email}";
        var message4 = $"Welcome, {name}! Your email: {email}";
    }

    // GOOD: Clean LINQ formatting
    public void GoodLinqFormatting()
    {
        var users = GetUsers();

        // Simple LINQ on single line
        var activeUsers = users.Where(u => u.IsActive).ToList();

        // Complex LINQ with proper line breaks and indentation
        var result = users
            .Where(u => u.IsActive)
            .Where(u => u.Age > 21)
            .Select(u => new { u.Name, u.Email })
            .OrderBy(u => u.Name)
            .ToList();

        // Multi-condition LINQ with clear formatting
        var filteredUsers = users
            .Where(u => u.IsActive &&
                       u.Age > 18 &&
                       !string.IsNullOrEmpty(u.Email))
            .ToList();
    }

    // GOOD: Consistent exception handling formatting
    public void GoodExceptionFormatting()
    {
        try
        {
            var data = GetData();
            ProcessData(data);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Null argument: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Invalid operation: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }
        finally
        {
            Cleanup();
        }
    }

    // GOOD: Modern switch expressions
    public string GoodSwitchFormatting(int value)
    {
        return value switch
        {
            1 => "One",
            2 => "Two",
            3 => "Three",
            _ => "Unknown"
        };
    }

    // GOOD: Alternative traditional switch formatting
    public string GoodTraditionalSwitchFormatting(int value)
    {
        switch (value)
        {
            case 1:
                return "One";
            case 2:
                return "Two";
            case 3:
                return "Three";
            default:
                return "Unknown";
        }
    }

    // GOOD: Method chaining with proper formatting
    public void GoodMethodChaining()
    {
        var result = GetUsers()
            .Where(u => u.IsActive)
            .OrderBy(u => u.Name)
            .ThenBy(u => u.Age)
            .Select(u => new UserDto
            {
                Name = u.Name,
                Email = u.Email,
                IsActive = u.IsActive
            })
            .ToList();
    }

    // GOOD: Async/await formatting
    public async Task<List<User>> GoodAsyncFormatting()
    {
        try
        {
            var users = await GetUsersAsync();

            var processedUsers = users
                .Where(u => u.IsActive)
                .ToList();

            foreach (var user in processedUsers)
            {
                await ProcessUserAsync(user);
            }

            return processedUsers;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing users: {ex.Message}");
            return new List<User>();
        }
    }

    // Helper methods with good formatting
    private bool IsValidOrderData(
        string userName,
        string userEmail,
        List<OrderItem> orderItems,
        PaymentMethod paymentMethod,
        ShippingAddress shippingAddress,
        BillingAddress billingAddress)
    {
        return !string.IsNullOrWhiteSpace(userName) &&
               !string.IsNullOrWhiteSpace(userEmail) &&
               orderItems?.Count > 0 &&
               paymentMethod != null &&
               shippingAddress != null &&
               billingAddress != null;
    }

    private decimal CalculateSubtotal(List<OrderItem> orderItems)
    {
        return orderItems.Sum(item => item.Price * item.Quantity);
    }

    private decimal CalculateDiscount(DiscountCoupon coupon, decimal subtotal)
    {
        return coupon?.IsValid == true ? subtotal * coupon.DiscountPercentage : 0;
    }

    private decimal CalculateTax(TaxCalculationRules rules, decimal amount, string state)
    {
        return rules.CalculateTax(amount, state);
    }

    private void ProcessPayment(decimal total, PaymentMethod paymentMethod)
    {
        Console.WriteLine($"Processing payment of {total:C} using {paymentMethod.Type}");
    }

    private void ProcessOrderItem(OrderItem item, User user)
    {
        if (item.IsValid)
        {
            var price = CalculatePrice(item);
            if (price > 0)
            {
                var discount = CalculateDiscount(item, user);
                var finalPrice = price - discount;
                SavePrice(item.Id, finalPrice);
            }
        }
    }

    // Supporting methods for compilation
    private List<User> GetUsers() => new();
    private List<Order> GetUserOrders(int userId) => new();
    private List<OrderItem> GetOrderItems(int orderId) => new();
    private decimal CalculatePrice(OrderItem item) => 10.0m;
    private decimal CalculateDiscount(OrderItem item, User user) => 1.0m;
    private void SavePrice(int itemId, decimal price) { }
    private object GetData() => new();
    private void ProcessData(object data) { }
    private void Cleanup() { }
    private async Task<List<User>> GetUsersAsync() => await Task.FromResult(new List<User>());
    private async Task ProcessUserAsync(User user) => await Task.CompletedTask;
}

// GOOD: Consistent naming conventions
public class ConsistentNamingConventions
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool IsActive { get; set; }

    public void ProcessData() { }
    public void ProcessUser() { }
    public void CalculateTotal() { }
}

// GOOD: Logical grouping of related members
public class WellOrganizedMembers
{
    // Properties grouped together
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    // Private fields grouped together
    private readonly string _secretKey = string.Empty;
    private readonly ILogger _logger;

    // Constructor
    public WellOrganizedMembers(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // Public methods grouped together
    public void ProcessOrder() { }
    public void CalculateTotal() { }
    public void ValidateUser() { }

    // Private methods grouped together
    private void InternalMethod() { }
    private void AnotherInternalMethod() { }
}

// GOOD: Property initialization patterns
public class PropertyInitializationExamples
{
    // Simple property initialization
    public string Name { get; set; } = string.Empty;
    public List<string> Items { get; set; } = new();

    // Required properties with clear formatting
    public required string RequiredProperty { get; set; }
    public required int RequiredNumber { get; set; }

    // Complex object initialization
    public UserPreferences Preferences { get; set; } = new()
    {
        Theme = "Dark",
        Language = "English",
        EnableNotifications = true
    };
}

// Supporting classes with good formatting
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsValid { get; set; }
}

public class PaymentMethod
{
    public string Type { get; set; } = string.Empty;
}

public class ShippingAddress
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}

public class BillingAddress
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}

public class DiscountCoupon
{
    public string Code { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime ExpirationDate { get; set; }
}

public class TaxCalculationRules
{
    private readonly Dictionary<string, decimal> _stateTaxRates = new()
    {
        { "CA", 0.0875m },
        { "NY", 0.08m },
        { "TX", 0.0625m }
    };

    public decimal CalculateTax(decimal amount, string state)
    {
        return _stateTaxRates.TryGetValue(state, out var rate) ? amount * rate : 0;
    }
}

public class UserPreferences
{
    public string Theme { get; set; } = "Light";
    public string Language { get; set; } = "English";
    public bool EnableNotifications { get; set; } = true;
}

public class UserDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

// Supporting interface
public interface ILogger
{
    void Log(string message);
}