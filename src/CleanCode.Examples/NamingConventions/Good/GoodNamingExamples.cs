namespace CleanCode.Examples.NamingConventions.Good;

// GOOD: Clear, descriptive class name
public class MathematicalCalculator
{
    // GOOD: Meaningful variable names
    public double FirstNumber { get; set; }
    public double SecondNumber { get; set; }
    public double Result { get; private set; }

    // GOOD: Clear, descriptive names without abbreviations
    public string Username { get; set; }
    public string Password { get; set; }
    public string DatabaseConnectionString { get; set; }

    // GOOD: No Hungarian notation, clear intent
    public int ItemCount { get; set; }
    public string CustomerName { get; set; }
    public bool IsAccountActive { get; set; }

    // GOOD: Method name clearly describes what it does
    public void CalculateAndDisplayResults()
    {
        // GOOD: Descriptive variable names
        const int MinimumValidValue = 5;
        const int DefaultProcessingTimeout = 42;

        int currentValue = 10;

        // GOOD: Variable name explains its purpose
        var retrievedCustomerData = GetCustomerDataFromDatabase();

        // GOOD: Named constants instead of magic numbers
        if (currentValue > MinimumValidValue)
        {
            ProcessCustomerData(retrievedCustomerData, DefaultProcessingTimeout);
        }
    }

    // GOOD: Method name clearly indicates what data is processed and how
    private void ProcessCustomerData(object customerData, int timeoutInSeconds)
    {
        // Customer data processing logic
        Console.WriteLine($"Processing customer data with timeout: {timeoutInSeconds}s");
    }

    // GOOD: Method name describes the action and parameters are clear
    public void ValidateAndSaveUserInput(object userData, bool shouldValidateImmediately)
    {
        // Validation and saving logic
    }

    // GOOD: Return type and purpose are clear from the method name
    public CustomerDataModel GetCustomerDataFromDatabase()
    {
        return new CustomerDataModel { Name = "John Doe", Email = "john@example.com" };
    }

    // GOOD: Method name clearly describes its specific purpose
    public void GenerateCustomerReportForCurrentMonth()
    {
        // Report generation logic
    }

    // GOOD: Consistent naming convention throughout
    public string UserFirstName { get; set; }
    public string UserEmail { get; set; }
    public string UserAddress { get; set; }
}

// GOOD: Clear, descriptive model class
public class CustomerDataModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActiveCustomer { get; set; }
}