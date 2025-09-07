namespace CleanCode.Examples.Functions.Bad;

public class BadFunctionExamples
{
    // BAD: Method does too many things, violates Single Responsibility Principle
    public string ProcessUserAndGenerateReport(string name, string email, int age, string address,
        string phone, bool isActive, DateTime createdAt, string department, double salary,
        bool hasInsurance, string emergencyContact)
    {
        // Validation
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name required");
        if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email required");
        if (age < 0 || age > 150) throw new ArgumentException("Invalid age");
        if (string.IsNullOrEmpty(address)) throw new ArgumentException("Address required");

        // Data processing
        var user = new { name, email, age, address, phone, isActive, createdAt, department, salary };

        // Business logic
        var bonus = 0.0;
        if (department == "Sales" && salary > 50000)
        {
            bonus = salary * 0.1;
        }
        else if (department == "Engineering" && salary > 60000)
        {
            bonus = salary * 0.15;
        }

        // Database operations
        SaveToDatabase(user);

        // Email sending
        if (isActive)
        {
            SendWelcomeEmail(email);
        }

        // Report generation
        var report = $"User: {name}\n";
        report += $"Email: {email}\n";
        report += $"Department: {department}\n";
        report += $"Salary: ${salary:F2}\n";
        report += $"Bonus: ${bonus:F2}\n";
        report += $"Total Compensation: ${salary + bonus:F2}\n";

        // Logging
        Console.WriteLine($"Processed user {name} at {DateTime.Now}");

        return report;
    }

    // BAD: Too many parameters (more than 3-4 is usually too many)
    public void CreateOrder(string customerName, string customerEmail, string customerPhone,
        string customerAddress, string productName, double productPrice, int quantity,
        double discountPercent, string couponCode, bool expressShipping,
        string shippingAddress, DateTime deliveryDate, string paymentMethod,
        string creditCardNumber, string expiryDate, string cvv)
    {
        // Implementation with too many parameters
    }

    // BAD: Deep nesting makes code hard to follow
    public string ProcessData(object data)
    {
        if (data != null)
        {
            if (data is string str)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.Length > 10)
                    {
                        if (str.Contains("@"))
                        {
                            if (str.Split('@').Length == 2)
                            {
                                var parts = str.Split('@');
                                if (parts[1].Contains("."))
                                {
                                    return "Valid email";
                                }
                                else
                                {
                                    return "Invalid domain";
                                }
                            }
                            else
                            {
                                return "Invalid format";
                            }
                        }
                        else
                        {
                            return "Not an email";
                        }
                    }
                    else
                    {
                        return "Too short";
                    }
                }
                else
                {
                    return "Empty string";
                }
            }
            else
            {
                return "Not a string";
            }
        }
        else
        {
            return "Null data";
        }
    }

    // BAD: Method has side effects that aren't obvious from name
    public int CalculateTotal(List<int> numbers)
    {
        // Hidden side effect: modifies global state
        GlobalCounter++;

        // Hidden side effect: logs to console
        Console.WriteLine("Calculating total...");

        // Hidden side effect: sends analytics
        TrackUsage("CalculateTotal", DateTime.Now);

        return numbers.Sum();
    }

    // BAD: Boolean parameters (flag arguments) make method calls unclear
    public void ProcessFile(string filename, bool shouldValidate, bool shouldLog, bool shouldBackup)
    {
        if (shouldValidate)
        {
            // Validation logic
        }

        if (shouldLog)
        {
            // Logging logic
        }

        if (shouldBackup)
        {
            // Backup logic
        }

        // File processing
    }

    // BAD: Method returns different types based on input (unpredictable)
    public object GetUserInfo(int id)
    {
        if (id > 0)
        {
            return new { Name = "John", Age = 30 }; // Returns anonymous object
        }
        else if (id == 0)
        {
            return "Guest User"; // Returns string
        }
        else
        {
            return null; // Returns null
        }
    }

    // BAD: Method name doesn't match what it actually does
    public string GetUserName(int userId)
    {
        // Actually does much more than just getting a name
        var user = GetUserFromDatabase(userId);
        UpdateLastAccessTime(userId);
        LogUserAccess(userId);
        SendAnalytics(userId);

        return user?.Name ?? "Unknown";
    }

    // Supporting methods (with intentionally bad implementations)
    private void SaveToDatabase(object user) { /* Bad: generic object parameter */ }
    private void SendWelcomeEmail(string email) { /* Implementation */ }
    private dynamic GetUserFromDatabase(int id) => new { Name = "John" };
    private void UpdateLastAccessTime(int id) { /* Side effect */ }
    private void LogUserAccess(int id) { /* Side effect */ }
    private void SendAnalytics(int id) { /* Side effect */ }
    private void TrackUsage(string method, DateTime time) { /* Side effect */ }

    // BAD: Global state that methods modify
    public static int GlobalCounter = 0;
}