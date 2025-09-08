namespace CleanCode.Examples.Classes.Bad;

// BAD: God Object - does everything
public class UserManager
{
    // BAD: Public fields expose internal state
    public string ConnectionString;
    public List<string> Errors;
    public bool IsDebugMode;

    // BAD: Too many responsibilities in one class
    public void CreateUser(string name, string email, string password)
    {
        // Database connection logic
        var connection = new SqlConnection(ConnectionString);

        // Validation logic
        if (string.IsNullOrEmpty(name)) Errors.Add("Name required");
        if (string.IsNullOrEmpty(email)) Errors.Add("Email required");

        // Password hashing logic
        var hashedPassword = HashPassword(password); // Simple mock implementation

        // Email validation logic
        if (!email.Contains("@")) Errors.Add("Invalid email");

        // Logging logic
        if (IsDebugMode)
        {
            Console.WriteLine($"Creating user: {name}");
            File.AppendAllText("log.txt", $"User creation: {DateTime.Now}\n");
        }

        // Business rules logic
        if (GetUserCount() > 1000)
        {
            throw new InvalidOperationException("Maximum users reached");
        }

        // SQL generation logic
        var sql = $"INSERT INTO Users (Name, Email, Password) VALUES ('{name}', '{email}', '{hashedPassword}')";

        // Database execution logic
        var command = new SqlCommand(sql, connection);
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();

        // Email sending logic
        var emailService = new SmtpClient("smtp.gmail.com");
        emailService.Send("admin@company.com", email, "Welcome", "Welcome to our system!");

        // Cache invalidation logic
        ClearUserCache();

        // Analytics logic
        TrackUserCreation(name, email);
    }

    // BAD: Tight coupling to concrete classes
    public void SendNotification(string message)
    {
        var emailSender = new SmtpClient("smtp.gmail.com"); // Tightly coupled
        var smsSender = new TwilioClient("key", "secret");   // Tightly coupled
        var pushSender = new FirebaseClient("token");        // Tightly coupled

        emailSender.Send("admin@company.com", "user@example.com", "Notification", message);
        smsSender.SendSms("+1234567890", message);
        pushSender.SendPush("user123", message);
    }

    // BAD: Mixed abstraction levels
    public void ProcessUserData()
    {
        // High-level business logic mixed with low-level details
        var users = GetAllUsers();

        foreach (var user in users)
        {
            // Low-level string manipulation
            var cleanName = user.Name.Trim().ToLower().Replace(" ", "_");

            // High-level business rule
            if (ShouldProcessUser(user))
            {
                // Low-level database operation
                var sql = $"UPDATE Users SET ProcessedName = '{cleanName}' WHERE Id = {user.Id}";
                ExecuteQuery(sql);

                // High-level notification
                NotifyUserProcessed(user);
            }
        }
    }

    // BAD: Poor encapsulation - exposes internal implementation
    public Dictionary<string, object> GetUserData(int userId)
    {
        var data = new Dictionary<string, object>();
        data["raw_sql_query"] = $"SELECT * FROM Users WHERE Id = {userId}";
        data["connection_string"] = ConnectionString;
        data["internal_cache_key"] = $"user_{userId}_cache";
        data["debug_info"] = GetDebugInfo();
        return data;
    }

    // BAD: Class with unclear single responsibility
    public void BackupAndOptimizeDatabase()
    {
        // Database backup logic
        CreateDatabaseBackup();

        // Performance optimization logic
        OptimizeIndexes();

        // Cleanup logic
        DeleteOldLogs();

        // Reporting logic
        GeneratePerformanceReport();

        // Email notification logic
        SendBackupNotification();
    }

    // Supporting methods with bad practices
    private int GetUserCount() => 100; // Hardcoded for example
    private void ClearUserCache() { /* Implementation */ }
    private void TrackUserCreation(string name, string email) { /* Implementation */ }
    private List<User> GetAllUsers() => new List<User>();
    private bool ShouldProcessUser(User user) => true;
    private void ExecuteQuery(string sql) { /* Implementation */ }
    private void NotifyUserProcessed(User user) { /* Implementation */ }
    private string GetDebugInfo() => "Debug info";
    private void CreateDatabaseBackup() { /* Implementation */ }
    private void OptimizeIndexes() { /* Implementation */ }
    private void DeleteOldLogs() { /* Implementation */ }
    private void GeneratePerformanceReport() { /* Implementation */ }
    private void SendBackupNotification() { /* Implementation */ }
    private string HashPassword(string password)
    {
        return $"hashed_{password}"; // Mock implementation for example
    }
}

// BAD: Anemic Domain Model - just data holders with no behavior
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public string Department { get; set; } = string.Empty;
}

// BAD: Static dependencies make testing difficult
public static class UserService
{
    private static string ConnectionString = "server=localhost;database=users";

    public static void CreateUser(User user)
    {
        // Direct dependency on static database class
        DatabaseHelper.ExecuteQuery($"INSERT INTO Users...");

        // Direct dependency on static email service
        EmailService.SendWelcomeEmail(user.Email);

        // Direct dependency on static logger
        Logger.Log($"User created: {user.Name}");
    }
}

// BAD: Utility classes that are hard to extend
public static class DatabaseHelper
{
    public static void ExecuteQuery(string sql) { /* Implementation */ }
}

public static class EmailService
{
    public static void SendWelcomeEmail(string email) { /* Implementation */ }
}

public static class Logger
{
    public static void Log(string message) { /* Implementation */ }
}

// BAD: Inheritance misuse - "Is-A" relationship doesn't make sense
public class Bird
{
    public virtual void Fly() { Console.WriteLine("Flying"); }
}

public class Penguin : Bird
{
    public override void Fly()
    {
        throw new NotSupportedException("Penguins can't fly!"); // Violates Liskov Substitution
    }
}

// BAD: Feature envy - class that uses methods from other classes more than its own
public class OrderProcessor
{
    public void ProcessOrder(Order order, Customer customer, PaymentService paymentService)
    {
        // Uses customer methods more than its own
        var discount = customer.GetDiscount();
        var address = customer.GetShippingAddress();
        var paymentMethod = customer.GetPreferredPaymentMethod();

        // Uses payment service methods
        var fee = paymentService.CalculateFee(order.Total);
        var result = paymentService.ProcessPayment(order.Total + fee, paymentMethod);

        // Minimal own logic
        Console.WriteLine("Order processed");
    }
}

// Supporting classes for examples
public class Order { public decimal Total { get; set; } }
public class Customer
{
    public decimal GetDiscount() => 0.1m;
    public string GetShippingAddress() => "123 Main St";
    public string GetPreferredPaymentMethod() => "Credit Card";
}

public class PaymentService
{
    public decimal CalculateFee(decimal amount) => amount * 0.03m;
    public bool ProcessPayment(decimal amount, string method) => true;
}

// BAD: Fake classes for compilation
public class SqlConnection
{
    public SqlConnection(string connectionString) { }
    public void Open() { }
    public void Close() { }
}

public class SqlCommand
{
    public SqlCommand(string sql, SqlConnection connection) { }
    public void ExecuteNonQuery() { }
}

public class SmtpClient
{
    public SmtpClient(string server) { }
    public void Send(string from, string to, string subject, string body) { }
}

public class TwilioClient
{
    public TwilioClient(string key, string secret) { }
    public void SendSms(string to, string message) { }
}

public class FirebaseClient
{
    public FirebaseClient(string token) { }
    public void SendPush(string userId, string message) { }
}
