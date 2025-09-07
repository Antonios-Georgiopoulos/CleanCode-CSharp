namespace CleanCode.Examples.Functions.Good;

public class GoodFunctionExamples
{
    // GOOD: Each method has a single, clear responsibility
    public UserProcessingResult ProcessUser(CreateUserRequest request)
    {
        ValidateUserRequest(request);
        var user = CreateUserFromRequest(request);
        var bonus = CalculateUserBonus(user);
        var savedUser = SaveUser(user);

        if (user.IsActive)
        {
            SendWelcomeEmail(user.Email);
        }

        var report = GenerateUserReport(savedUser, bonus);
        LogUserProcessing(savedUser);

        return new UserProcessingResult(savedUser, report, bonus);
    }

    // GOOD: Single responsibility - only validates
    private void ValidateUserRequest(CreateUserRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
            throw new ArgumentException("Name is required", nameof(request.Name));

        if (string.IsNullOrEmpty(request.Email))
            throw new ArgumentException("Email is required", nameof(request.Email));

        if (request.Age < 0 || request.Age > 150)
            throw new ArgumentException("Invalid age", nameof(request.Age));

        if (string.IsNullOrEmpty(request.Address))
            throw new ArgumentException("Address is required", nameof(request.Address));
    }

    // GOOD: Pure function with clear input/output
    private User CreateUserFromRequest(CreateUserRequest request)
    {
        return new User
        {
            Name = request.Name,
            Email = request.Email,
            Age = request.Age,
            Address = request.Address,
            Phone = request.Phone,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            Department = request.Department,
            Salary = request.Salary
        };
    }

    // GOOD: Pure calculation function, no side effects
    private double CalculateUserBonus(User user)
    {
        const double SalesBonusRate = 0.1;
        const double EngineeringBonusRate = 0.15;
        const double SalesMinSalary = 50000;
        const double EngineeringMinSalary = 60000;

        return user.Department switch
        {
            "Sales" when user.Salary > SalesMinSalary => user.Salary * SalesBonusRate,
            "Engineering" when user.Salary > EngineeringMinSalary => user.Salary * EngineeringBonusRate,
            _ => 0.0
        };
    }

    // GOOD: Few parameters using a well-defined object
    public OrderResult CreateOrder(CreateOrderRequest request)
    {
        ValidateOrderRequest(request);
        var order = BuildOrder(request);
        var processedOrder = ProcessPayment(order, request.PaymentInfo);
        var savedOrder = SaveOrder(processedOrder);

        return new OrderResult(savedOrder);
    }

    // GOOD: Early returns eliminate deep nesting
    public string ValidateEmailAddress(string input)
    {
        if (input == null)
            return "Input cannot be null";

        if (string.IsNullOrEmpty(input))
            return "Email cannot be empty";

        if (input.Length <= 10)
            return "Email is too short";

        if (!input.Contains("@"))
            return "Email must contain @ symbol";

        var parts = input.Split('@');
        if (parts.Length != 2)
            return "Email format is invalid";

        if (!parts[1].Contains("."))
            return "Domain must contain a dot";

        return "Valid email";
    }

    // GOOD: Pure function with no side effects
    public int CalculateSum(IEnumerable<int> numbers)
    {
        return numbers?.Sum() ?? 0;
    }

    // GOOD: Separate methods instead of boolean parameters
    public void ProcessFileWithValidation(string filename)
    {
        ValidateFile(filename);
        ProcessFile(filename);
    }

    public void ProcessFileWithLogging(string filename)
    {
        ProcessFile(filename);
        LogFileProcessing(filename);
    }

    public void ProcessFileWithBackup(string filename)
    {
        BackupFile(filename);
        ProcessFile(filename);
    }

    // GOOD: Consistent return type
    public UserInfo GetUserInfo(int id)
    {
        if (id <= 0)
            return UserInfo.CreateGuestUser();

        var user = GetUserFromDatabase(id);
        return user != null
            ? UserInfo.FromUser(user)
            : UserInfo.CreateUnknownUser();
    }

    // GOOD: Method name matches exactly what it does
    public string GetUserNameById(int userId)
    {
        var user = GetUserFromDatabase(userId);
        return user?.Name ?? "Unknown";
    }

    // GOOD: Separate method for side effects
    public string GetUserNameByIdWithTracking(int userId)
    {
        var userName = GetUserNameById(userId);
        TrackUserAccess(userId);
        return userName;
    }

    // GOOD: Small, focused helper methods
    private void ValidateOrderRequest(CreateOrderRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        // Additional validation
    }

    private Order BuildOrder(CreateOrderRequest request)
    {
        return new Order
        {
            CustomerInfo = request.CustomerInfo,
            Products = request.Products,
            ShippingInfo = request.ShippingInfo,
            CreatedAt = DateTime.UtcNow
        };
    }

    private Order ProcessPayment(Order order, PaymentInfo paymentInfo)
    {
        // Payment processing logic
        order.PaymentStatus = "Processed";
        return order;
    }

    private Order SaveOrder(Order order)
    {
        // Database save logic
        order.Id = Guid.NewGuid();
        return order;
    }

    private void ValidateFile(string filename) { /* Validation logic */ }
    private void ProcessFile(string filename) { /* Processing logic */ }
    private void LogFileProcessing(string filename) { /* Logging logic */ }
    private void BackupFile(string filename) { /* Backup logic */ }
    private void SendWelcomeEmail(string email) { /* Email logic */ }
    private User SaveUser(User user) { user.Id = Guid.NewGuid(); return user; }
    private string GenerateUserReport(User user, double bonus) => $"User: {user.Name}, Bonus: ${bonus:F2}";
    private void LogUserProcessing(User user) { /* Logging logic */ }
    private User GetUserFromDatabase(int id) => new User { Name = "John", Id = Guid.NewGuid() };
    private void TrackUserAccess(int userId) { /* Analytics logic */ }
}

// GOOD: Well-defined data structures instead of many parameters
public record CreateUserRequest(
    string Name,
    string Email,
    int Age,
    string Address,
    string Phone,
    bool IsActive,
    string Department,
    double Salary
);

public record CreateOrderRequest(
    CustomerInfo CustomerInfo,
    List<Product> Products,
    ShippingInfo ShippingInfo,
    PaymentInfo PaymentInfo
);

public record UserProcessingResult(User User, string Report, double Bonus);
public record OrderResult(Order Order);

// Supporting classes
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Department { get; set; } = string.Empty;
    public double Salary { get; set; }
}

public class UserInfo
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public static UserInfo CreateGuestUser() => new() { Name = "Guest", Type = "Guest" };
    public static UserInfo CreateUnknownUser() => new() { Name = "Unknown", Type = "Unknown" };
    public static UserInfo FromUser(User user) => new() { Name = user.Name, Type = "Regular" };
}

public class Order
{
    public Guid Id { get; set; }
    public CustomerInfo CustomerInfo { get; set; } = new();
    public List<Product> Products { get; set; } = new();
    public ShippingInfo ShippingInfo { get; set; } = new();
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CustomerInfo
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class Product
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
}

public class ShippingInfo
{
    public string Address { get; set; } = string.Empty;
    public bool IsExpress { get; set; }
}

public class PaymentInfo
{
    public string Method { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
}