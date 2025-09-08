using CleanCode.Examples.CodeFormatting.Bad;
using CleanCode.Examples.CodeFormatting.Good;

using Xunit;

namespace CleanCode.CSharp.Tests;

public class CodeFormattingExamplesTests
{
    [Fact]
    public void BadExample_InconsistentFormatting_MakesCodeHardToRead()
    {
        // Arrange
        var badExample = new BadFormattingExamples();
        var testData = new List<string> { "item1", "item2", "item3" };

        // Act - The badly formatted code still works functionally
        badExample.ProcessData(testData);

        // Assert - But readability and maintainability suffer
        Assert.True(true); // Code executes but is hard to read and maintain

        // The problems with bad formatting:
        // 1. Inconsistent indentation makes structure unclear
        // 2. Poor spacing around operators reduces readability
        // 3. Inconsistent brace placement creates visual confusion
        // 4. Makes code reviews more difficult
    }

    [Fact]
    public void BadExample_LongLines_AreHardToReadAndReview()
    {
        // Arrange
        var badExample = new BadFormattingExamples();
        var orderItems = new List<CleanCode.Examples.CodeFormatting.Bad.OrderItem>();
        var paymentMethod = new CleanCode.Examples.CodeFormatting.Bad.PaymentMethod();
        var shippingAddress = new CleanCode.Examples.CodeFormatting.Bad.ShippingAddress();
        var billingAddress = new CleanCode.Examples.CodeFormatting.Bad.BillingAddress();
        var discountCoupon = new CleanCode.Examples.CodeFormatting.Bad.DiscountCoupon();
        var taxRules = new CleanCode.Examples.CodeFormatting.Bad.TaxCalculationRules();

        // Act - Long method signature and lines are functional but problematic
        badExample.ProcessUserOrderWithComplexBusinessLogicAndMultipleValidationsAndCalculations(
            "John", "john@example.com", orderItems, paymentMethod,
            shippingAddress, billingAddress, discountCoupon, taxRules);

        // Assert - Long lines cause several issues:
        // 1. Horizontal scrolling required
        // 2. Difficult to see in code reviews
        // 3. Hard to understand parameter relationships
        // 4. Printing issues
        Assert.True(true);
    }

    [Fact]
    public void BadExample_InconsistentNaming_CreatesConfusion()
    {
        // Arrange - Demonstrating bad naming conventions (using nested class)
        var badNaming = new BadFormattingExamples.mixed_naming_Conventions();

        // Act & Assert - Mixed naming styles are confusing
        badNaming.firstName = "John";
        badNaming.LastName = "Doe";
        badNaming.email_address = "john@example.com";
        badNaming.AGE = 30;
        badNaming.is_active = true;

        badNaming.Process_Data();
        badNaming.processUser();
        badNaming.CALCULATE_TOTAL();

        // The problems:
        // 1. Mixing camelCase, PascalCase, snake_case, and UPPERCASE
        // 2. Inconsistency makes code unprofessional
        // 3. Harder to remember correct naming
        // 4. Violates language conventions
        Assert.Equal("John", badNaming.firstName);
    }

    [Fact]
    public void BadExample_PoorMemberOrganization_HampersNavigation()
    {
        // Arrange - Using the nested class correctly
        var badOrganization = new BadFormattingExamples.PoorMemberOrganization();

        // Act & Assert - Members scattered without logical grouping
        badOrganization.Name = "Test";
        badOrganization.Age = 30;
        badOrganization.Email = "test@example.com";
        badOrganization.IsActive = true;

        badOrganization.ProcessOrder();
        badOrganization.CalculateTotal();
        badOrganization.ValidateUser();

        // Problems with poor organization:
        // 1. Related members are separated
        // 2. Difficult to find specific members
        // 3. Makes code navigation harder
        // 4. Reduces code comprehension
        Assert.Equal("Test", badOrganization.Name);
    }

    [Fact]
    public void GoodExample_ConsistentFormatting_ImprovesReadability()
    {
        // Arrange
        var goodExample = new GoodFormattingExamples();
        var testData = new List<string> { "item1", "item2", "item3" };

        // Act - Well-formatted code is easy to read and understand
        goodExample.ProcessData(testData);

        // Assert - Good formatting provides several benefits
        Assert.True(true);

        // Benefits of good formatting:
        // 1. Consistent indentation shows code structure clearly
        // 2. Proper spacing improves readability
        // 3. Consistent brace placement reduces cognitive load
        // 4. Makes code reviews faster and more effective
        // 5. Professional appearance
    }

    [Fact]
    public void GoodExample_BrokenLines_ImproveReadabilityAndMaintenance()
    {
        // Arrange
        var goodExample = new GoodFormattingExamples();
        var orderItems = new List<CleanCode.Examples.CodeFormatting.Good.OrderItem>();
        var paymentMethod = new CleanCode.Examples.CodeFormatting.Good.PaymentMethod();
        var shippingAddress = new CleanCode.Examples.CodeFormatting.Good.ShippingAddress();
        var billingAddress = new CleanCode.Examples.CodeFormatting.Good.BillingAddress();
        var discountCoupon = new CleanCode.Examples.CodeFormatting.Good.DiscountCoupon();
        var taxRules = new CleanCode.Examples.CodeFormatting.Good.TaxCalculationRules();

        // Act - Well-broken method signature with clear parameter organization
        goodExample.ProcessUserOrder(
            "John",
            "john@example.com",
            orderItems,
            paymentMethod,
            shippingAddress,
            billingAddress,
            discountCoupon,
            taxRules);

        // Assert - Properly broken lines provide benefits:
        // 1. Each parameter is clearly visible
        // 2. Easy to modify individual parameters
        // 3. Better for code reviews
        // 4. No horizontal scrolling needed
        Assert.True(true);
    }

    [Fact]
    public void GoodExample_ConsistentNaming_FollowsConventions()
    {
        // Arrange - Demonstrating consistent naming conventions
        var goodNaming = new GoodFormattingExamples.ConsistentNamingConventions();

        // Act & Assert - Consistent C# naming conventions
        goodNaming.FirstName = "John";
        goodNaming.LastName = "Doe";
        goodNaming.EmailAddress = "john@example.com";
        goodNaming.Age = 30;
        goodNaming.IsActive = true;

        goodNaming.ProcessData();
        goodNaming.ProcessUser();
        goodNaming.CalculateTotal();

        // Benefits of consistent naming:
        // 1. Follows C# conventions (PascalCase for public members)
        // 2. Predictable and professional
        // 3. Easier to remember and type
        // 4. IDE support and IntelliSense work better
        Assert.Equal("John", goodNaming.FirstName);
    }

    [Fact]
    public void GoodExample_WellOrganizedMembers_ImproveNavigation()
    {
        // Arrange
        var mockLogger = new TestMockLogger();
        var goodOrganization = new GoodFormattingExamples.WellOrganizedMembers(mockLogger);

        // Act & Assert - Logically grouped members
        goodOrganization.Name = "Test";
        goodOrganization.Age = 30;
        goodOrganization.Email = "test@example.com";
        goodOrganization.IsActive = true;

        goodOrganization.ProcessOrder();
        goodOrganization.CalculateTotal();
        goodOrganization.ValidateUser();

        // Benefits of good organization:
        // 1. Related members are grouped together
        // 2. Easy to find specific functionality
        // 3. Logical code flow
        // 4. Better maintainability
        Assert.Equal("Test", goodOrganization.Name);
    }

    [Fact]
    public void GoodExample_ConsistentStringFormatting_ImprovesReadability()
    {
        // Arrange
        var goodExample = new GoodFormattingExamples();

        // Act - Consistent string interpolation usage
        goodExample.GoodStringFormatting();

        // Assert - Benefits of consistent string formatting:
        // 1. Uniform approach across codebase
        // 2. Better performance with string interpolation
        // 3. Easier to read and understand
        // 4. Consistent maintenance patterns
        Assert.True(true);
    }

    [Fact]
    public void GoodExample_CleanLinqFormatting_EnhancesReadability()
    {
        // Arrange
        var goodExample = new GoodFormattingExamples();

        // Act - Well-formatted LINQ expressions
        goodExample.GoodLinqFormatting();

        // Assert - Benefits of good LINQ formatting:
        // 1. Each operation on its own line for complex queries
        // 2. Proper indentation shows data flow
        // 3. Easy to modify individual operations
        // 4. Clear logical structure
        Assert.True(true);
    }

    [Fact]
    public void GoodExample_ConsistentExceptionHandling_ImprovesClarity()
    {
        // Arrange
        var goodExample = new GoodFormattingExamples();

        // Act - Consistently formatted exception handling
        goodExample.GoodExceptionFormatting();

        // Assert - Benefits of consistent exception formatting:
        // 1. Clear visual structure
        // 2. Easy to understand exception flow
        // 3. Consistent indentation and spacing
        // 4. Professional appearance
        Assert.True(true);
    }

    [Fact]
    public void GoodExample_ModernSwitchExpressions_ShowBestPractices()
    {
        // Arrange
        var goodExample = new GoodFormattingExamples();

        // Act - Modern switch expression formatting
        var result1 = goodExample.GoodSwitchFormatting(1);
        var result2 = goodExample.GoodSwitchFormatting(2);
        var result3 = goodExample.GoodSwitchFormatting(999);

        // Assert - Modern switch expressions are more concise
        Assert.Equal("One", result1);
        Assert.Equal("Two", result2);
        Assert.Equal("Unknown", result3);

        // Benefits of modern switch expressions:
        // 1. More concise than traditional switch
        // 2. Expression-based (returns value directly)
        // 3. Better pattern matching support
        // 4. Cleaner formatting
    }

    [Fact]
    public void GoodExample_TraditionalSwitch_ShowsAlternativeFormatting()
    {
        // Arrange
        var goodExample = new GoodFormattingExamples();

        // Act - Traditional switch with good formatting
        var result1 = goodExample.GoodTraditionalSwitchFormatting(1);
        var result2 = goodExample.GoodTraditionalSwitchFormatting(2);
        var result3 = goodExample.GoodTraditionalSwitchFormatting(999);

        // Assert - Traditional switch can also be well-formatted
        Assert.Equal("One", result1);
        Assert.Equal("Two", result2);
        Assert.Equal("Unknown", result3);

        // When to use traditional switch:
        // 1. Complex logic in each case
        // 2. Need for fallthrough behavior
        // 3. Multiple statements per case
        // 4. Team prefers traditional syntax
    }

    [Fact]
    public void GoodExample_PropertyInitialization_ShowsModernPatterns()
    {
        // Arrange & Act - Modern property initialization patterns
        var example = new GoodFormattingExamples.PropertyInitializationExamples
        {
            RequiredProperty = "Test",
            RequiredNumber = 42
        };

        // Assert - Modern C# features improve code quality
        Assert.Equal("Test", example.RequiredProperty);
        Assert.Equal(42, example.RequiredNumber);
        Assert.NotNull(example.Name); // Initialized to empty string
        Assert.NotNull(example.Items); // Initialized to empty list
        Assert.NotNull(example.Preferences); // Initialized with default values

        // Benefits of modern initialization:
        // 1. Clear default values
        // 2. Required properties enforced at compile time
        // 3. Object initializers improve readability
        // 4. Less null reference exceptions
    }

    [Fact]
    public void GoodExample_AsyncFormatting_ShowsBestPractices()
    {
        // Arrange
        var goodExample = new GoodFormattingExamples();

        // Act - Well-formatted async method
        var result = goodExample.GoodAsyncFormatting().Result;

        // Assert - Async methods should be properly formatted
        Assert.NotNull(result);

        // Best practices for async formatting:
        // 1. Proper exception handling with async
        // 2. Clear await usage
        // 3. Consistent indentation
        // 4. Logical flow structure
    }

    [Fact]
    public void Comparison_FormattingQuality_ShowsClearDifferences()
    {
        // BAD: Inconsistent formatting makes code hard to read
        var badExample = new BadFormattingExamples();
        var badData = new List<string> { "test" };
        badExample.ProcessData(badData); // Functional but poorly formatted

        // GOOD: Consistent formatting improves readability
        var goodExample = new GoodFormattingExamples();
        var goodData = new List<string> { "test" };
        goodExample.ProcessData(goodData); // Functional and well-formatted

        // The difference:
        // 1. Bad formatting works but is hard to maintain
        // 2. Good formatting works and is easy to read/modify
        // 3. Consistency reduces cognitive load
        // 4. Professional appearance matters

        Assert.True(true); // Both work, but one is clearly better
    }

    [Fact]
    public void GoodExample_ServiceImplementation_ShowsRealWorldFormatting()
    {
        // Arrange
        var mockLogger = new TestMockLogger();
        var mockRepository = new TestMockRepository();

        using var service = new GoodFormattingExamples.ServiceImplementation(mockLogger, mockRepository);

        // Act & Assert - Real-world class with proper formatting
        var user = service.GetUserAsync(1).Result;
        Assert.NotNull(user);

        // Benefits shown:
        // 1. Clear constructor parameter formatting
        // 2. Proper method formatting
        // 3. Good exception handling
        // 4. Proper disposal pattern
        // 5. Consistent indentation throughout
    }
}

// Mock implementations for testing - renamed to avoid conflicts
public class TestMockLogger : CleanCode.Examples.CodeFormatting.Good.ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"Log: {message}");
    }
}

public class TestMockRepository : CleanCode.Examples.CodeFormatting.Good.IRepository
{
    public async Task<CleanCode.Examples.CodeFormatting.Good.User> GetByIdAsync(int id)
    {
        await Task.CompletedTask;
        return new CleanCode.Examples.CodeFormatting.Good.User
        {
            Id = id,
            Name = "Test User",
            Email = "test@example.com",
            IsActive = true
        };
    }

    public void Dispose()
    {
        // Mock disposal
    }
}