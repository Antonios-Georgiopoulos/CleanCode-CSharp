using CleanCode.Examples.NamingConventions.Bad;
using CleanCode.Examples.NamingConventions.Good;

using Xunit;

namespace CleanCode.CSharp.Tests;

public class NamingConventionsTests
{
    [Fact]
    public void BadExample_Calc_ShouldWork_ButCodeIsHardToUnderstand()
    {
        // Arrange - BAD: Variable names don't explain purpose
        var calc = new Calc();
        calc.a = 10;
        calc.b = 5;

        // Act - BAD: Method name doesn't explain what it does
        calc.DoStuff();

        // Assert - We can test it works, but code is unclear
        Assert.NotNull(calc);
        Assert.Equal(10, calc.a);
    }

    [Fact]
    public void BadExample_Variables_DemonstrateCommonNamingProblems()
    {
        // Arrange
        var calc = new Calc();

        // Act & Assert - BAD: These names tell us nothing about purpose
        calc.usr = "john_doe";           // What kind of user? Why abbreviate?
        calc.pwd = "secret123";          // Why not 'password'?
        calc.iCount = 42;                // Hungarian notation is outdated
        calc.strName = "Test";           // Type is already clear from declaration
        calc.bIsActive = true;           // Again, redundant type prefix

        // These assignments work, but code is hard to read and maintain
        Assert.Equal("john_doe", calc.usr);
        Assert.Equal("secret123", calc.pwd);
        Assert.Equal(42, calc.iCount);
    }

    [Fact]
    public void GoodExample_MathematicalCalculator_ShouldBeEasyToUnderstand()
    {
        // Arrange - GOOD: Clear, descriptive names
        var calculator = new MathematicalCalculator();
        calculator.FirstNumber = 10;
        calculator.SecondNumber = 5;

        // Act - GOOD: Method name clearly describes what it does
        calculator.CalculateAndDisplayResults();

        // Assert - Code is self-documenting
        Assert.NotNull(calculator);
        Assert.Equal(10, calculator.FirstNumber);
        Assert.Equal(5, calculator.SecondNumber);
    }

    [Fact]
    public void GoodExample_Properties_ShouldHaveMeaningfulNames()
    {
        // Arrange
        var calculator = new MathematicalCalculator();

        // Act & Assert - GOOD: Names clearly indicate purpose and intent
        calculator.Username = "john.doe@example.com";
        calculator.Password = "SecurePassword123!";
        calculator.ItemCount = 100;
        calculator.CustomerName = "John Doe";
        calculator.IsAccountActive = true;

        // Reading this code, we immediately understand what each variable represents
        Assert.Equal("john.doe@example.com", calculator.Username);
        Assert.Equal("SecurePassword123!", calculator.Password);
        Assert.Equal(100, calculator.ItemCount);
        Assert.Equal("John Doe", calculator.CustomerName);
        Assert.True(calculator.IsAccountActive);
    }

    [Fact]
    public void GoodExample_CustomerDataModel_ShouldBeWellStructured()
    {
        // Arrange & Act - GOOD: Model with clear, consistent naming
        var customer = new CustomerDataModel
        {
            Name = "Jane Smith",
            Email = "jane.smith@example.com",
            CreatedAt = DateTime.Now,
            IsActiveCustomer = true
        };

        // Assert - The model is self-explanatory
        Assert.Equal("Jane Smith", customer.Name);
        Assert.Equal("jane.smith@example.com", customer.Email);
        Assert.True(customer.IsActiveCustomer);
        Assert.True(customer.CreatedAt <= DateTime.Now);
    }

    [Fact]
    public void Comparison_ReadabilityDifference_ShouldBeObvious()
    {
        // BAD: What does this code do?
        var calc = new Calc();
        calc.usr = "test";
        var temp = calc.GetData();
        calc.Process(temp, 1);

        // GOOD: This code is self-explanatory
        var calculator = new MathematicalCalculator();
        calculator.Username = "test_user";
        var customerData = calculator.GetCustomerDataFromDatabase();
        calculator.ValidateAndSaveUserInput(customerData, shouldValidateImmediately: true);

        // Both work, but the second is immediately understandable
        Assert.NotNull(calc);
        Assert.NotNull(calculator);
    }
}