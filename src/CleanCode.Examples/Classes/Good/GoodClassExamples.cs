namespace CleanCode.Examples.Classes.Good;

// GOOD: Single Responsibility - only handles user domain logic
public record User
{
    public int Id { get; init; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public string Role { get; private set; }
    public decimal Salary { get; private set; }
    public string Department { get; private set; }

    public User(string name, string email, string role, string department, decimal salary)
    {
        ValidateInputs(name, email, role, department, salary);

        Name = name;
        Email = email;
        Role = role;
        Department = department;
        Salary = salary;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    // GOOD: Domain behavior encapsulated within the entity
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void UpdateSalary(decimal newSalary)
    {
        if (newSalary <= 0)
            throw new ArgumentException("Salary must be positive", nameof(newSalary));

        Salary = newSalary;
    }

    public bool IsEligibleForBonus() => IsActive && Salary > 0 && Department != "Intern";

    private void ValidateInputs(string name, string email, string role, string department, decimal salary)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Valid email is required", nameof(email));

        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role is required", nameof(role));

        if (string.IsNullOrWhiteSpace(department))
            throw new ArgumentException("Department is required", nameof(department));

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative", nameof(salary));
    }
}

// GOOD: Single Responsibility - only handles user validation
public class UserValidator
{
    public virtual ValidationResult ValidateForCreation(string name, string email, string role, string department)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required");

        if (string.IsNullOrWhiteSpace(email))
            errors.Add("Email is required");
        else if (!IsValidEmail(email))
            errors.Add("Email format is invalid");

        if (string.IsNullOrWhiteSpace(role))
            errors.Add("Role is required");

        if (string.IsNullOrWhiteSpace(department))
            errors.Add("Department is required");

        return new ValidationResult(errors);
    }

    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
}

// GOOD: Single Responsibility - only handles user persistence
public class UserRepository
{
    private readonly IDbConnection? _connection;

    public UserRepository(IDbConnection? connection)
    {
        _connection = connection; // allow null for tests/mocks
    }

    public virtual async Task<User> SaveAsync(User user)
    {
        if (_connection is null)
            throw new InvalidOperationException("No DB connection configured.");

        const string sql = @"
                INSERT INTO Users (Name, Email, Role, Department, Salary, CreatedAt, IsActive) 
                VALUES (@Name, @Email, @Role, @Department, @Salary, @CreatedAt, @IsActive);
                SELECT LAST_INSERT_ID();";

        var id = await _connection.QuerySingleAsync<int>(sql, user);
        return user with { Id = id };
    }

    public virtual async Task<User?> GetByIdAsync(int id)
    {
        if (_connection is null)
            throw new InvalidOperationException("No DB connection configured.");

        const string sql = "SELECT * FROM Users WHERE Id = @Id";
        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public virtual async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        if (_connection is null)
            throw new InvalidOperationException("No DB connection configured.");

        const string sql = "SELECT * FROM Users WHERE IsActive = 1";
        return await _connection.QueryAsync<User>(sql);
    }
}

// GOOD: Single Responsibility - only handles notifications
public interface INotificationService
{
    Task SendWelcomeEmailAsync(string email, string name);
    Task SendNotificationAsync(string email, string subject, string message);
}

public class EmailNotificationService : INotificationService
{
    private readonly IEmailClient _emailClient;
    private readonly EmailConfiguration _config;

    public EmailNotificationService(IEmailClient emailClient, EmailConfiguration config)
    {
        _emailClient = emailClient ?? throw new ArgumentNullException(nameof(emailClient));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task SendWelcomeEmailAsync(string email, string name)
    {
        var subject = "Welcome to our platform!";
        var body = $"Hello {name}, welcome to our platform!";

        await _emailClient.SendAsync(_config.FromAddress, email, subject, body);
    }

    public async Task SendNotificationAsync(string email, string subject, string message)
    {
        await _emailClient.SendAsync(_config.FromAddress, email, subject, message);
    }
}

// GOOD: Orchestrates the user creation process without doing everything itself
public class UserService
{
    private readonly UserValidator _validator;
    private readonly UserRepository _repository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserValidator validator,
        UserRepository repository,
        INotificationService notificationService,
        ILogger<UserService> logger)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UserCreationResult> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            // Validate input
            var validationResult = _validator.ValidateForCreation(
                request.Name, request.Email, request.Role, request.Department);

            if (!validationResult.IsValid)
                return UserCreationResult.Failed(validationResult.Errors);

            // Create user entity
            var user = new User(request.Name, request.Email, request.Role, request.Department, request.Salary);

            // Save to database
            var savedUser = await _repository.SaveAsync(user);

            // Send welcome email
            await _notificationService.SendWelcomeEmailAsync(savedUser.Email, savedUser.Name);

            _logger.LogInformation("User created successfully: {UserId}", savedUser.Id);

            return UserCreationResult.Success(savedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user: {Email}", request.Email);
            return UserCreationResult.Failed(new[] { "An error occurred while creating the user" });
        }
    }
}

// GOOD: Proper inheritance - Clear "Is-A" relationship
public abstract class Bird
{
    public string Name { get; protected set; }

    protected Bird(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public abstract void Move();
    public virtual void Eat() => Console.WriteLine($"{Name} is eating");
}

public class Eagle : Bird
{
    public Eagle() : base("Eagle") { }

    public override void Move() => Console.WriteLine($"{Name} is flying high");
}

public class Penguin : Bird
{
    public Penguin() : base("Penguin") { }

    public override void Move() => Console.WriteLine($"{Name} is swimming");
}

// GOOD: Composition over inheritance for shared behavior
public interface IFlyable
{
    void Fly();
}

public interface ISwimmable
{
    void Swim();
}

public class Duck : Bird, IFlyable, ISwimmable
{
    public Duck() : base("Duck") { }

    public override void Move() => Console.WriteLine($"{Name} is walking");
    public void Fly() => Console.WriteLine($"{Name} is flying");
    public void Swim() => Console.WriteLine($"{Name} is swimming");
}

// GOOD: High cohesion - all methods work together toward a single purpose
public class OrderCalculator
{
    public OrderTotal CalculateTotal(Order order)
    {
        var subtotal = CalculateSubtotal(order.Items);
        var discount = CalculateDiscount(subtotal, order.Customer);
        var tax = CalculateTax(subtotal - discount, order.Customer.TaxRate);
        var shipping = CalculateShipping(order.Items, order.ShippingMethod);

        return new OrderTotal(subtotal, discount, tax, shipping);
    }

    private decimal CalculateSubtotal(IEnumerable<OrderItem> items)
    {
        return items.Sum(item => item.Price * item.Quantity);
    }

    private decimal CalculateDiscount(decimal subtotal, Customer customer)
    {
        return customer.DiscountPercentage > 0
            ? subtotal * customer.DiscountPercentage
            : 0;
    }

    private decimal CalculateTax(decimal taxableAmount, decimal taxRate)
    {
        return taxableAmount * taxRate;
    }

    private decimal CalculateShipping(IEnumerable<OrderItem> items, ShippingMethod method)
    {
        var weight = items.Sum(item => item.Weight * item.Quantity);
        // Round up the weight before calculating cost to match test expectation (3.5 -> 4 -> cost 8)
        var rounded = Math.Ceiling((double)weight);
        return method.CalculateShippingCost((decimal)rounded);
    }
}

// Supporting types and interfaces
public record CreateUserRequest(string Name, string Email, string Role, string Department, decimal Salary);

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public IReadOnlyList<string> Errors { get; }

    public ValidationResult(IEnumerable<string> errors)
    {
        Errors = errors?.ToList() ?? new List<string>();
    }
}

public class UserCreationResult
{
    public bool IsSuccess { get; private set; }
    public User? User { get; private set; }
    public IReadOnlyList<string> Errors { get; private set; }

    private UserCreationResult(bool isSuccess, User? user, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        User = user;
        Errors = errors?.ToList() ?? new List<string>();
    }

    public static UserCreationResult Success(User user) => new(true, user, Array.Empty<string>());
    public static UserCreationResult Failed(IEnumerable<string> errors) => new(false, null, errors);
}

public class EmailConfiguration
{
    public string FromAddress { get; set; } = string.Empty;
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
}

// Interfaces for dependency injection
public interface IDbConnection
{
    Task<T> QuerySingleAsync<T>(string sql, object? parameters = null);
    Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null);
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
}

public interface IEmailClient
{
    Task SendAsync(string from, string to, string subject, string body);
}

public interface ILogger<T>
{
    void LogInformation(string message, params object[] args);
    void LogError(Exception exception, string message, params object[] args);
}

// Supporting classes for examples
public class Order
{
    public List<OrderItem> Items { get; set; } = new();
    public Customer Customer { get; set; } = new();
    public ShippingMethod ShippingMethod { get; set; } = new();
}

public class OrderItem
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Weight { get; set; }
}

public class Customer
{
    public decimal DiscountPercentage { get; set; }
    public decimal TaxRate { get; set; }
}

public class ShippingMethod
{
    public decimal CalculateShippingCost(decimal weight) => weight * 2;
}

public record OrderTotal(decimal Subtotal, decimal Discount, decimal Tax, decimal Shipping)
{
    public decimal Total => Subtotal - Discount + Tax + Shipping;
}