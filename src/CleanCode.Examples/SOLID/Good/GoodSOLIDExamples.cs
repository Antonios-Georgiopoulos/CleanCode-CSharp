namespace CleanCode.Examples.SOLID.Good;

// GOOD: Single Responsibility Principle
// Each class has one reason to change

public class Order
{
    public string CustomerName { get; }
    public string ProductName { get; }
    public decimal Price { get; }
    public int Quantity { get; }
    public decimal Total => Price * Quantity;

    public Order(string customerName, string productName, decimal price, int quantity)
    {
        CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        Price = price;
        Quantity = quantity;
    }
}

// GOOD: SRP - Only responsible for validation
public class OrderValidator
{
    public ValidationResult Validate(Order order)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(order.CustomerName))
            errors.Add("Customer name is required");

        if (order.Price <= 0)
            errors.Add("Price must be positive");

        if (order.Quantity <= 0)
            errors.Add("Quantity must be positive");

        return new ValidationResult(errors);
    }
}

// GOOD: SRP - Only responsible for business logic
public class DiscountCalculator
{
    public decimal CalculateDiscount(Order order)
    {
        if (order.Total > 100)
            return order.Total * 0.1m;

        return 0m;
    }
}

// GOOD: SRP - Only responsible for data persistence
public interface IOrderRepository
{
    Task SaveOrderAsync(Order order, decimal finalAmount);
}

public class OrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task SaveOrderAsync(Order order, decimal finalAmount)
    {
        // Database logic only
        await Task.CompletedTask; // Mock implementation
    }
}

// GOOD: SRP - Only responsible for notifications
public interface INotificationService
{
    Task SendOrderConfirmationAsync(string customerEmail, string orderDetails);
}

public class EmailNotificationService : INotificationService
{
    public async Task SendOrderConfirmationAsync(string customerEmail, string orderDetails)
    {
        // Email sending logic only
        await Task.CompletedTask; // Mock implementation
    }
}

// GOOD: SRP - Only responsible for logging
public interface ILogger
{
    void LogOrderProcessed(Order order, decimal finalAmount);
}

public class FileLogger : ILogger
{
    public void LogOrderProcessed(Order order, decimal finalAmount)
    {
        // Logging logic only
    }
}

// GOOD: SRP - Orchestrates the process but doesn't do everything itself
public class OrderProcessor
{
    private readonly OrderValidator _validator;
    private readonly DiscountCalculator _discountCalculator;
    private readonly IOrderRepository _orderRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger _logger;

    public OrderProcessor(
        OrderValidator validator,
        DiscountCalculator discountCalculator,
        IOrderRepository orderRepository,
        INotificationService notificationService,
        ILogger logger)
    {
        _validator = validator;
        _discountCalculator = discountCalculator;
        _orderRepository = orderRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<ProcessOrderResult> ProcessOrderAsync(Order order)
    {
        var validationResult = _validator.Validate(order);
        if (!validationResult.IsValid)
            return ProcessOrderResult.Failed(validationResult.Errors);

        var discount = _discountCalculator.CalculateDiscount(order);
        var finalAmount = order.Total - discount;

        await _orderRepository.SaveOrderAsync(order, finalAmount);
        await _notificationService.SendOrderConfirmationAsync("customer@email.com",
            $"Order for {order.ProductName}: ${finalAmount}");

        _logger.LogOrderProcessed(order, finalAmount);

        return ProcessOrderResult.Success(finalAmount);
    }
}

// GOOD: Open/Closed Principle
// Open for extension, closed for modification

public abstract class Shape
{
    public abstract double CalculateArea();
    public abstract string GetShapeInfo();
}

public class Circle : Shape
{
    public double Radius { get; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public override double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }

    public override string GetShapeInfo()
    {
        return $"Circle with radius {Radius}";
    }
}

public class Rectangle : Shape
{
    public double Width { get; }
    public double Height { get; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override double CalculateArea()
    {
        return Width * Height;
    }

    public override string GetShapeInfo()
    {
        return $"Rectangle {Width}x{Height}";
    }
}

public class Triangle : Shape
{
    public double Base { get; }
    public double Height { get; }

    public Triangle(double baseLength, double height)
    {
        Base = baseLength;
        Height = height;
    }

    public override double CalculateArea()
    {
        return 0.5 * Base * Height;
    }

    public override string GetShapeInfo()
    {
        return $"Triangle with base {Base} and height {Height}";
    }
}

// GOOD: We can add new shapes without modifying existing code
public class Pentagon : Shape
{
    public double SideLength { get; }

    public Pentagon(double sideLength)
    {
        SideLength = sideLength;
    }

    public override double CalculateArea()
    {
        // Pentagon area formula
        return 0.25 * Math.Sqrt(25 + 10 * Math.Sqrt(5)) * SideLength * SideLength;
    }

    public override string GetShapeInfo()
    {
        return $"Pentagon with side length {SideLength}";
    }
}

// GOOD: AreaCalculator doesn't need to change when new shapes are added
public class AreaCalculator
{
    public double CalculateArea(Shape shape)
    {
        return shape.CalculateArea();
    }

    public List<ShapeAreaInfo> CalculateAreas(IEnumerable<Shape> shapes)
    {
        return shapes.Select(shape => new ShapeAreaInfo(
            shape.GetShapeInfo(),
            shape.CalculateArea()
        )).ToList();
    }
}

// GOOD: Report generation separated (SRP) and extensible (OCP)
public interface IReportFormatter
{
    string FormatReport(IEnumerable<ShapeAreaInfo> shapeAreas);
}

public class TextReportFormatter : IReportFormatter
{
    public string FormatReport(IEnumerable<ShapeAreaInfo> shapeAreas)
    {
        var report = "Area Report:\n";
        foreach (var shapeArea in shapeAreas)
        {
            report += $"{shapeArea.ShapeInfo}: {shapeArea.Area:F2}\n";
        }
        return report;
    }
}

public class JsonReportFormatter : IReportFormatter
{
    public string FormatReport(IEnumerable<ShapeAreaInfo> shapeAreas)
    {
        // JSON formatting logic
        return "{ \"shapes\": [ ... ] }"; // Mock implementation
    }
}

// GOOD: Liskov Substitution Principle
// Derived classes can be substituted for base classes without breaking functionality

public abstract class Bird
{
    public string Name { get; protected set; }

    protected Bird(string name)
    {
        Name = name;
    }

    public abstract void Move();
    public virtual void Eat() => Console.WriteLine($"{Name} is eating");
}

// GOOD: All birds can move, but each in their own way
public class Duck : Bird
{
    public Duck() : base("Duck") { }

    public override void Move() => Console.WriteLine($"{Name} is flying");
}

public class Penguin : Bird
{
    public Penguin() : base("Penguin") { }

    public override void Move() => Console.WriteLine($"{Name} is swimming");
}

public class Ostrich : Bird
{
    public Ostrich() : base("Ostrich") { }

    public override void Move() => Console.WriteLine($"{Name} is running");
}

// GOOD: Behavioral composition instead of problematic inheritance
public interface ICanFly
{
    void Fly();
}

public interface ICanSwim
{
    void Swim();
}

public interface ICanRun
{
    void Run();
}

public class SuperDuck : Bird, ICanFly, ICanSwim
{
    public SuperDuck() : base("SuperDuck") { }

    public override void Move() => Console.WriteLine($"{Name} is walking");
    public void Fly() => Console.WriteLine($"{Name} is flying");
    public void Swim() => Console.WriteLine($"{Name} is swimming");
}

// GOOD: Interface Segregation Principle
// Many specific interfaces are better than one general-purpose interface

public interface IWorkable
{
    void Work();
}

public interface IEatable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}

public interface IMeetingAttendable
{
    void AttendMeeting();
}

public interface IReportWritable
{
    void WriteReport();
}

public interface ICommunicable
{
    void AnswerPhone();
    void SendEmail();
}

// GOOD: Robot only implements interfaces it needs
public class Robot : IWorkable, IMeetingAttendable, IReportWritable, ICommunicable
{
    public void Work() => Console.WriteLine("Robot working...");
    public void AttendMeeting() => Console.WriteLine("Robot attending meeting...");
    public void WriteReport() => Console.WriteLine("Robot writing report...");
    public void AnswerPhone() => Console.WriteLine("Robot answering phone...");
    public void SendEmail() => Console.WriteLine("Robot sending email...");
}

// GOOD: Human implements all interfaces but could choose which ones based on role
public class Human : IWorkable, IEatable, ISleepable, IMeetingAttendable, IReportWritable, ICommunicable
{
    public void Work() => Console.WriteLine("Human working...");
    public void Eat() => Console.WriteLine("Human eating...");
    public void Sleep() => Console.WriteLine("Human sleeping...");
    public void AttendMeeting() => Console.WriteLine("Human attending meeting...");
    public void WriteReport() => Console.WriteLine("Human writing report...");
    public void AnswerPhone() => Console.WriteLine("Human answering phone...");
    public void SendEmail() => Console.WriteLine("Human sending email...");
}

// GOOD: Manager might only need meeting and communication interfaces
public class Manager : IMeetingAttendable, ICommunicable
{
    public void AttendMeeting() => Console.WriteLine("Manager attending meeting...");
    public void AnswerPhone() => Console.WriteLine("Manager answering phone...");
    public void SendEmail() => Console.WriteLine("Manager sending email...");
}

// GOOD: Dependency Inversion Principle
// Depend on abstractions, not concretions

public interface IMessageSender
{
    Task SendMessageAsync(string recipient, string subject, string message);
}

public class EmailSender : IMessageSender
{
    public async Task SendMessageAsync(string recipient, string subject, string message)
    {
        Console.WriteLine($"Sending email to {recipient}: {subject}");
        await Task.CompletedTask;
    }
}

public class SmsSender : IMessageSender
{
    public async Task SendMessageAsync(string recipient, string subject, string message)
    {
        Console.WriteLine($"Sending SMS to {recipient}: {message}");
        await Task.CompletedTask;
    }
}

public class PushNotificationSender : IMessageSender
{
    public async Task SendMessageAsync(string recipient, string subject, string message)
    {
        Console.WriteLine($"Sending push notification to {recipient}: {message}");
        await Task.CompletedTask;
    }
}

// GOOD: High-level module depends on abstraction
public class NotificationService
{
    private readonly IEnumerable<IMessageSender> _messageSenders;

    public NotificationService(IEnumerable<IMessageSender> messageSenders)
    {
        _messageSenders = messageSenders ?? throw new ArgumentNullException(nameof(messageSenders));
    }

    public async Task NotifyCustomerAsync(string recipient, string subject, string message)
    {
        foreach (var sender in _messageSenders)
        {
            try
            {
                await sender.SendMessageAsync(recipient, subject, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send via {sender.GetType().Name}: {ex.Message}");
            }
        }
    }
}

// GOOD: Business logic separated from data access concerns
public interface ICustomerRepository
{
    Task SaveCustomerAsync(Customer customer);
    Task<Customer> GetCustomerByIdAsync(int id);
}

public interface ICustomerLogger
{
    void LogCustomerCreated(Customer customer);
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; }
    public string Email { get; }

    public Customer(string name, string email)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}

// GOOD: High-level business logic depends on abstractions
public class CustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly ICustomerLogger _logger;

    public CustomerService(ICustomerRepository repository, ICustomerLogger logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Customer> CreateCustomerAsync(string name, string email)
    {
        var customer = new Customer(name, email);
        await _repository.SaveCustomerAsync(customer);
        _logger.LogCustomerCreated(customer);
        return customer;
    }
}

// Supporting types
public record ValidationResult(IReadOnlyList<string> Errors)
{
    public bool IsValid => !Errors.Any();

    public ValidationResult() : this(Array.Empty<string>()) { }
    public ValidationResult(IEnumerable<string> errors) : this(errors.ToList()) { }
}

public record ProcessOrderResult(bool IsSuccess, decimal? FinalAmount, IReadOnlyList<string> Errors)
{
    public static ProcessOrderResult Success(decimal finalAmount) =>
        new(true, finalAmount, Array.Empty<string>());

    public static ProcessOrderResult Failed(IEnumerable<string> errors) =>
        new(false, null, errors.ToList());
}

public record ShapeAreaInfo(string ShapeInfo, double Area);