namespace CleanCode.Examples.NamingConventions.Good;

// GOOD: Clear, descriptive class name
public class OrderCalculator
{
    // GOOD: Intention-revealing names
    public double FirstNumber { get; set; }
    public double SecondNumber { get; set; }

    // GOOD: Consistent casing & boolean phrasing
    public bool IsActive { get; set; }

    // GOOD: Named constants instead of magic numbers
    private const int MinimumValidValue = 5;
    private const int DefaultProcessingTimeoutSeconds = 42;

    // GOOD: Self-descriptive methods & parameters
    public void CalculateIfAboveMinimum()
    {
        var currentValue = 10; // Example value for illustration
        if (currentValue > MinimumValidValue)
        {
            var customer = GetCustomerFromRepository();
            ProcessCustomer(customer, DefaultProcessingTimeoutSeconds);
        }
    }

    private Customer GetCustomerFromRepository() =>
        new Customer { Name = "Jane Doe", Email = "jane@example.com" };

    private void ProcessCustomer(Customer customer, int timeoutSeconds) { }
}

// GOOD: Simple, consistent model
public class Customer
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}