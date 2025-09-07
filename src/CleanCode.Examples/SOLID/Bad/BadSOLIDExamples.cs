namespace CleanCode.Examples.SOLID.Bad;

// BAD: Violates Single Responsibility Principle
// This class has multiple reasons to change: data access, business logic, validation, logging, email
public class OrderProcessor
{
    public void ProcessOrder(string customerName, string productName, decimal price, int quantity)
    {
        // Validation responsibility
        if (string.IsNullOrEmpty(customerName))
            throw new ArgumentException("Customer name required");

        if (price <= 0)
            throw new ArgumentException("Price must be positive");

        // Business logic responsibility
        var total = price * quantity;
        var discount = 0m;

        if (total > 100)
            discount = total * 0.1m;

        var finalAmount = total - discount;

        // Database responsibility
        var connectionString = "server=localhost;database=orders";
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var sql = $"INSERT INTO Orders (CustomerName, Product, Amount) VALUES ('{customerName}', '{productName}', {finalAmount})";
        var command = new SqlCommand(sql, connection);
        command.ExecuteNonQuery();

        // Email responsibility
        var emailBody = $"Dear {customerName}, your order for {productName} has been processed. Total: ${finalAmount}";
        var emailSender = new SmtpClient("smtp.gmail.com");
        emailSender.Send("noreply@company.com", "customer@email.com", "Order Confirmation", emailBody);

        // Logging responsibility
        var logMessage = $"Order processed: {customerName} - {productName} - ${finalAmount}";
        File.AppendAllText("orders.log", $"{DateTime.Now}: {logMessage}\n");

        // Audit responsibility
        var auditEntry = $"User processed order {customerName} at {DateTime.Now}";
        File.AppendAllText("audit.log", auditEntry + "\n");
    }
}

// BAD: Violates Open/Closed Principle
// Adding new shapes requires modifying this class
public class AreaCalculator
{
    public double CalculateArea(object shape)
    {
        if (shape is Circle circle)
        {
            return Math.PI * circle.Radius * circle.Radius;
        }
        else if (shape is Rectangle rectangle)
        {
            return rectangle.Width * rectangle.Height;
        }
        else if (shape is Triangle triangle)
        {
            return 0.5 * triangle.Base * triangle.Height;
        }
        // BAD: To add a new shape, we must modify this method
        // What if we want to add Pentagon, Hexagon, etc.?
        else
        {
            throw new ArgumentException("Unknown shape type");
        }
    }

    // BAD: Also violates SRP - this class now handles both calculation and formatting
    public string GenerateReport(List<object> shapes)
    {
        var report = "Area Report:\n";
        foreach (var shape in shapes)
        {
            var area = CalculateArea(shape);
            report += $"Shape: {shape.GetType().Name}, Area: {area:F2}\n";
        }
        return report;
    }
}

// BAD: Violates Liskov Substitution Principle
public class Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Flying...");
    }
}

public class Penguin : Bird
{
    public override void Fly()
    {
        // BAD: Penguin can't fly, but we're forced to override
        throw new NotSupportedException("Penguins cannot fly!");
    }
}

public class Duck : Bird
{
    public override void Fly()
    {
        Console.WriteLine("Duck flying...");
    }
}

// BAD: Violates Interface Segregation Principle
// Fat interface that forces classes to implement methods they don't need
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
    void TakeBreak();
    void AttendMeeting();
    void WriteReport();
    void AnswerPhone();
    void SendEmail();
}

// BAD: Robot doesn't eat or sleep, but is forced to implement these methods
public class Robot : IWorker
{
    public void Work() => Console.WriteLine("Robot working...");

    public void Eat() => throw new NotSupportedException("Robots don't eat!");
    public void Sleep() => throw new NotSupportedException("Robots don't sleep!");
    public void TakeBreak() => throw new NotSupportedException("Robots don't take breaks!");

    public void AttendMeeting() => Console.WriteLine("Robot attending meeting...");
    public void WriteReport() => Console.WriteLine("Robot writing report...");
    public void AnswerPhone() => Console.WriteLine("Robot answering phone...");
    public void SendEmail() => Console.WriteLine("Robot sending email...");
}

// BAD: Human might not need to do all these things either
public class Human : IWorker
{
    public void Work() => Console.WriteLine("Human working...");
    public void Eat() => Console.WriteLine("Human eating...");
    public void Sleep() => Console.WriteLine("Human sleeping...");
    public void TakeBreak() => Console.WriteLine("Human taking break...");
    public void AttendMeeting() => Console.WriteLine("Human attending meeting...");
    public void WriteReport() => Console.WriteLine("Human writing report...");
    public void AnswerPhone() => Console.WriteLine("Human answering phone...");
    public void SendEmail() => Console.WriteLine("Human sending email...");
}

// BAD: Violates Dependency Inversion Principle
// High-level modules depend on low-level modules
public class EmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        Console.WriteLine($"Sending email to {to}: {subject}");
    }
}

public class SmsService
{
    public void SendSms(string phoneNumber, string message)
    {
        Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");
    }
}

// BAD: OrderNotificationService depends directly on concrete classes
public class OrderNotificationService
{
    private readonly EmailService _emailService; // Concrete dependency
    private readonly SmsService _smsService;     // Concrete dependency

    public OrderNotificationService()
    {
        _emailService = new EmailService(); // BAD: Direct instantiation
        _smsService = new SmsService();     // BAD: Direct instantiation
    }

    public void NotifyCustomer(string customerEmail, string phoneNumber, string orderDetails)
    {
        // BAD: Tightly coupled to specific implementations
        _emailService.SendEmail(customerEmail, "Order Confirmation", orderDetails);
        _smsService.SendSms(phoneNumber, $"Order confirmed: {orderDetails}");

        // What if we want to add push notifications, WhatsApp, etc.?
        // We'd have to modify this class!
    }
}

// BAD: Another DIP violation - business logic depends on data access details
public class CustomerService
{
    public void CreateCustomer(string name, string email)
    {
        // BAD: Business logic mixed with data access concerns
        var connectionString = "server=localhost;database=customers";
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        // BAD: Direct SQL in business logic
        var sql = $"INSERT INTO Customers (Name, Email) VALUES ('{name}', '{email}')";
        var command = new SqlCommand(sql, connection);
        command.ExecuteNonQuery();

        // BAD: Direct coupling to specific logging implementation
        Console.WriteLine($"Customer created: {name}");
        File.AppendAllText("customers.log", $"Created: {name} at {DateTime.Now}\n");
    }
}

// Supporting classes for examples
public class Circle
{
    public double Radius { get; set; }
}

public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
}

public class Triangle
{
    public double Base { get; set; }
    public double Height { get; set; }
}

// BAD: Multiple violations in one class
public class ReportGenerator
{
    // Violates SRP: handles data access, business logic, formatting, and delivery
    public void GenerateAndSendCustomerReport(int customerId)
    {
        // Data access responsibility
        var connectionString = "server=localhost;database=crm";
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var sql = $"SELECT * FROM Customers WHERE Id = {customerId}";
        var command = new SqlCommand(sql, connection);
        var reader = command.ExecuteReader();

        string customerData = "";
        if (reader.Read())
        {
            customerData = $"Customer: {reader["Name"]}, Email: {reader["Email"]}";
        }
        reader.Close();

        // Business logic responsibility
        var orders = GetCustomerOrders(customerId);
        var totalSpent = orders.Sum(o => o.Amount);
        var customerTier = totalSpent > 1000 ? "Gold" : totalSpent > 500 ? "Silver" : "Bronze";

        // Formatting responsibility
        var report = $"Customer Report\n{customerData}\nTier: {customerTier}\nTotal Spent: ${totalSpent}";

        // Delivery responsibility - violates OCP (need to modify for new delivery methods)
        var emailService = new EmailService();
        emailService.SendEmail("manager@company.com", "Customer Report", report);

        // File responsibility
        File.WriteAllText($"report_{customerId}.txt", report);

        // Logging responsibility
        Console.WriteLine($"Report generated for customer {customerId}");
    }

    private List<Order> GetCustomerOrders(int customerId)
    {
        // More data access code...
        return new List<Order>();
    }
}

public class Order
{
    public decimal Amount { get; set; }
}

// Fake classes for compilation
public class SqlConnection : IDisposable
{
    public SqlConnection(string connectionString) { }
    public void Open() { }
    public void Dispose() { }
}

public class SqlCommand
{
    public SqlCommand(string sql, SqlConnection connection) { }
    public void ExecuteNonQuery() { }
    public SqlDataReader ExecuteReader() => new SqlDataReader();
}

public class SqlDataReader
{
    public bool Read() => true;
    public object this[string columnName] => columnName == "Name" ? "John Doe" : "john@example.com";
    public void Close() { }
}

public class SmtpClient
{
    public SmtpClient(string server) { }
    public void Send(string from, string to, string subject, string body) { }
}