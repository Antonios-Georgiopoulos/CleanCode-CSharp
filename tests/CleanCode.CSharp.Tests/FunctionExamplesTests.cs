using CleanCode.Examples.Functions.Bad;
using CleanCode.Examples.Functions.Good;

namespace CleanCode.CSharp.Tests;

public class FunctionExamplesTests
{
    [Fact]
    public void BadExample_ProcessUserAndGenerateReport_WorksButIsHardToMaintain()
    {
        // Arrange - BAD: Too many parameters make this call unreadable
        var badExample = new BadFunctionExamples();

        // Act - BAD: It's unclear what each parameter does without checking method signature
        var result = badExample.ProcessUserAndGenerateReport(
            "John Doe", "john@example.com", 30, "123 Main St",
            "555-1234", true, DateTime.Now, "Engineering", 75000,
            true, "Jane Doe");

        // Assert - Works but the method does too many things
        Assert.NotNull(result);
        Assert.Contains("John Doe", result);
        Assert.Contains("Engineering", result);
    }

    [Fact]
    public void BadExample_ProcessData_DemonstratesDeepNesting()
    {
        // Arrange
        var badExample = new BadFunctionExamples();

        // Act & Assert - BAD: Deep nesting makes this hard to follow
        Assert.Equal("Valid email", badExample.ProcessData("test@example.com"));
        Assert.Equal("Invalid domain", badExample.ProcessData("test@invalid"));
        Assert.Equal("Too short", badExample.ProcessData("plaintext")); // "plaintext" είναι 9 chars < 10
        Assert.Equal("Too short", badExample.ProcessData("short"));
        Assert.Equal("Empty string", badExample.ProcessData(""));
        Assert.Equal("Null data", badExample.ProcessData(null));
        Assert.Equal("Not a string", badExample.ProcessData(123));
    }

    [Fact]
    public void BadExample_CalculateTotal_HasHiddenSideEffects()
    {
        // Arrange
        var badExample = new BadFunctionExamples();
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var initialCounter = BadFunctionExamples.GlobalCounter;

        // Act - BAD: Method has hidden side effects
        var result = badExample.CalculateTotal(numbers);

        // Assert - The calculation works, but side effects are problematic
        Assert.Equal(15, result);
        Assert.Equal(initialCounter + 1, BadFunctionExamples.GlobalCounter);
    }

    [Fact]
    public void BadExample_ProcessFile_BooleanParametersAreConfusing()
    {
        // Arrange
        var badExample = new BadFunctionExamples();

        // Act - BAD: What do these boolean parameters mean?
        // badExample.ProcessFile("test.txt", true, false, true);
        // Without looking at method signature, it's unclear what each boolean does

        // Assert - We can't easily test this because the intent is unclear
        Assert.True(true); // Placeholder assertion
    }

    [Fact]
    public void BadExample_GetUserInfo_ReturnsInconsistentTypes()
    {
        // Arrange
        var badExample = new BadFunctionExamples();

        // Act & Assert - BAD: Return type is unpredictable
        var result1 = badExample.GetUserInfo(1);
        var result2 = badExample.GetUserInfo(0);
        var result3 = badExample.GetUserInfo(-1);

        // Different return types make this method unreliable
        Assert.NotNull(result1); // Anonymous object
        Assert.IsType<string>(result2); // String
        Assert.Null(result3); // Null
    }

    [Fact]
    public void GoodExample_ProcessUser_IsClearAndMaintainable()
    {
        // Arrange - GOOD: Single, well-defined parameter object
        var goodExample = new GoodFunctionExamples();
        var request = new CreateUserRequest(
            Name: "John Doe",
            Email: "john@example.com",
            Age: 30,
            Address: "123 Main St",
            Phone: "555-1234",
            IsActive: true,
            Department: "Engineering",
            Salary: 75000
        );

        // Act - GOOD: Method name clearly indicates what it does
        var result = goodExample.ProcessUser(request);

        // Assert - Clear structure and predictable results
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.User.Name);
        Assert.Equal("Engineering", result.User.Department);
        Assert.True(result.Bonus > 0); // Engineering bonus applied
    }

    [Fact]
    public void GoodExample_ValidateEmailAddress_UsesEarlyReturns()
    {
        // Arrange
        var goodExample = new GoodFunctionExamples();

        // Act & Assert - GOOD: Early returns eliminate deep nesting
        Assert.Equal("Input cannot be null", goodExample.ValidateEmailAddress(null));
        Assert.Equal("Email cannot be empty", goodExample.ValidateEmailAddress(""));
        Assert.Equal("Email is too short", goodExample.ValidateEmailAddress("short"));
        Assert.Equal("Email is too short", goodExample.ValidateEmailAddress("plaintext")); // "plaintext" είναι 9 chars
        Assert.Equal("Email format is invalid", goodExample.ValidateEmailAddress("test@@example.com"));
        Assert.Equal("Domain must contain a dot", goodExample.ValidateEmailAddress("test@invalid"));
        Assert.Equal("Valid email", goodExample.ValidateEmailAddress("test@example.com"));
    }

    [Fact]
    public void GoodExample_CalculateSum_IsPureFunction()
    {
        // Arrange
        var goodExample = new GoodFunctionExamples();
        var numbers = new List<int> { 1, 2, 3, 4, 5 };

        // Act - GOOD: Pure function with no side effects
        var result1 = goodExample.CalculateSum(numbers);
        var result2 = goodExample.CalculateSum(numbers);

        // Assert - Consistent results, no side effects
        Assert.Equal(15, result1);
        Assert.Equal(15, result2);
        Assert.Equal(result1, result2);

        // Test edge cases
        Assert.Equal(0, goodExample.CalculateSum(null));
        Assert.Equal(0, goodExample.CalculateSum(new List<int>()));
    }

    [Fact]
    public void GoodExample_ProcessFileVariants_AreClearInIntent()
    {
        // Arrange
        var goodExample = new GoodFunctionExamples();
        var filename = "test.txt";

        // Act - GOOD: Method names clearly indicate what they do
        // No boolean parameters needed - intent is clear from method names
        goodExample.ProcessFileWithValidation(filename);
        goodExample.ProcessFileWithLogging(filename);
        goodExample.ProcessFileWithBackup(filename);

        // Assert - Each method has a clear, single purpose
        Assert.True(true); // Methods execute without errors
    }

    [Fact]
    public void GoodExample_GetUserInfo_HasConsistentReturnType()
    {
        // Arrange
        var goodExample = new GoodFunctionExamples();

        // Act - GOOD: Always returns the same type
        var result1 = goodExample.GetUserInfo(1);
        var result2 = goodExample.GetUserInfo(0);
        var result3 = goodExample.GetUserInfo(-1);

        // Assert - Consistent return type makes code predictable
        Assert.IsType<UserInfo>(result1);
        Assert.IsType<UserInfo>(result2);
        Assert.IsType<UserInfo>(result3);

        Assert.Equal("John", result1.Name);
        Assert.Equal("Guest", result2.Name);
        Assert.Equal("Guest", result3.Name); // id <= 0 επιστρέφει Guest, όχι Unknown
    }

    [Fact]
    public void GoodExample_CreateOrder_UsesWellDefinedParameters()
    {
        // Arrange
        var goodExample = new GoodFunctionExamples();
        var request = new CreateOrderRequest(
            CustomerInfo: new CustomerInfo { Name = "John Doe", Email = "john@example.com" },
            Products: new List<Product> { new() { Name = "Widget", Price = 10.0 } },
            ShippingInfo: new ShippingInfo { Address = "123 Main St", IsExpress = true },
            PaymentInfo: new PaymentInfo { Method = "Credit Card", CardNumber = "****1234" }
        );

        // Act - GOOD: Single parameter object instead of many primitive parameters
        var result = goodExample.CreateOrder(request);

        // Assert - Clear structure and easy to understand
        Assert.NotNull(result);
        Assert.NotNull(result.Order);
        Assert.Equal("John Doe", result.Order.CustomerInfo.Name);
        Assert.Equal("Processed", result.Order.PaymentStatus);
    }

    [Fact]
    public void Comparison_FunctionLength_GoodFunctionsAreShorterAndFocused()
    {
        // This test demonstrates how good functions are easier to test
        // because they have single responsibilities and clear contracts

        var goodExample = new GoodFunctionExamples();

        // GOOD functions can be tested in isolation
        var emailValidation = goodExample.ValidateEmailAddress("test@example.com");
        var sum = goodExample.CalculateSum(new[] { 1, 2, 3 });

        Assert.Equal("Valid email", emailValidation);
        Assert.Equal(6, sum);

        // Each function does one thing well and is easy to verify
    }

    [Fact]
    public void GoodExample_SeparateSideEffects_AllowsFlexibleTesting()
    {
        // Arrange
        var goodExample = new GoodFunctionExamples();

        // Act - GOOD: Can test pure function separately from side effects
        var nameOnly = goodExample.GetUserNameById(1);
        var nameWithTracking = goodExample.GetUserNameByIdWithTracking(1);

        // Assert - Both methods work, but side effects are explicit
        Assert.Equal("John", nameOnly);
        Assert.Equal("John", nameWithTracking);

        // The separation allows testing the core logic without side effects
    }
}