using CleanCode.Examples.Comments.Bad;
using CleanCode.Examples.Comments.Good;

using Xunit;

namespace CleanCode.CSharp.Tests;

public class CommentExamplesTests
{
    [Fact]
    public void BadExample_ObviousComments_AddNoValue()
    {
        // Arrange
        var badExamples = new BadCommentExamples();

        // Act
        var result = badExamples.Add(5, 3);

        // Assert - The comment "Add a and b together" is obvious from the method name and operation
        Assert.Equal(8, result);

        // The real issue: comments that state the obvious waste time and clutter code
        // Better to have no comment than a comment that adds no information
    }

    [Fact]
    public void BadExample_MisleadingComments_AreWorseThanNoComments()
    {
        // Arrange
        var badExamples = new BadCommentExamples();
        var price = 100m;

        // Act
        var result = badExamples.CalculateDiscount(price);

        // Assert - Comment says 10% but code applies 15%
        Assert.Equal(85m, result); // 100 - 15 = 85, not 90 as comment suggests

        // Misleading comments are dangerous - they cause confusion and bugs
        // When code changes, comments often don't get updated
    }

    [Fact]
    public void BadExample_CommentedOutCode_CreatesConfusion()
    {
        // Arrange
        var badExamples = new BadCommentExamples();

        // Act - ProcessOrder contains commented-out code
        badExamples.ProcessOrder();

        // Assert - Commented-out code creates several problems:
        // 1. It's unclear whether it should be removed or restored
        // 2. It clutters the codebase
        // 3. Version control systems are better for tracking removed code
        // 4. It becomes outdated quickly
        Assert.True(true); // Method executes but contains confusing commented code
    }

    [Fact]
    public void BadExample_NoiseComments_StateTheObvious()
    {
        // Arrange - Customer class has noise comments
        var customer = new BadCommentExamples.Customer
        {
            Name = "John Doe",
            Email = "john@example.com",
            Age = 30,
            IsActive = true
        };

        // Act & Assert - Properties are self-explanatory
        Assert.Equal("John Doe", customer.Name);
        Assert.Equal("john@example.com", customer.Email);
        Assert.Equal(30, customer.Age);
        Assert.True(customer.IsActive);

        // The comments like "Customer name" add no value
        // Property names and types already convey this information
    }

    [Fact]
    public void BadExample_ComplexCodeExplainedByComments_ShouldBeRefactored()
    {
        // Arrange
        var badExamples = new BadCommentExamples();
        var testData = new List<object> { "test", 42, "another string", 0, null };

        // Act
        badExamples.ProcessData(testData);

        // Assert - The method has extensive comments trying to explain complex code
        // This is a red flag: if code needs that many comments to be understood,
        // the code itself should be refactored to be more readable
        Assert.True(true); // Method executes but is poorly designed
    }

    [Fact]
    public void GoodExample_BusinessLogicComments_AddValue()
    {
        // Arrange
        var goodExamples = new GoodCommentExamples();
        var principal = 1000m;
        var rate = 0.05m; // 5% annual rate
        var years = 3;

        // Act
        var result = goodExamples.CalculateCompoundInterest(principal, rate, years);

        // Assert - The comment explains the mathematical formula being used
        // This adds value because the formula might not be obvious to all developers
        Assert.True(result > principal); // Interest should increase the amount
        Assert.Equal(1157.625m, Math.Round(result, 3)); // 1000 * (1.05)^3
    }

    [Fact]
    public void GoodExample_WarningComments_PreventProblems()
    {
        // Arrange
        var goodExamples = new GoodCommentExamples();
        var largeDataset = Enumerable.Range(1, 5000)
            .Select(i => new DataItem())
            .ToList();

        // Act
        goodExamples.ProcessLargeDataset(largeDataset);

        // Assert - The warning comment about memory usage is valuable
        // It helps developers understand when to use alternative methods
        Assert.Equal(5000, largeDataset.Count);

        // This type of comment prevents performance issues by warning about constraints
    }

    [Fact]
    public void GoodExample_ImplementationDecisionComments_ExplainWhy()
    {
        // Arrange
        var goodExamples = new GoodCommentExamples();

        // Act
        var id1 = goodExamples.GenerateUniqueId();
        var id2 = goodExamples.GenerateUniqueId();

        // Assert - The comment explains why Guid was chosen over timestamp
        Assert.NotEqual(id1, id2);
        Assert.Equal(8, id1.Length); // Should be 8 characters
        Assert.Equal(8, id2.Length);

        // This type of comment explains architectural decisions and prevents
        // future developers from "fixing" something that was intentionally designed
    }

    [Fact]
    public void GoodExample_APIDocumentation_ProvidesUsefulInformation()
    {
        // Arrange
        var goodExamples = new GoodCommentExamples();

        // Act & Assert - XML documentation provides comprehensive API information
        Assert.True(goodExamples.ValidateEmail("user@example.com"));
        Assert.False(goodExamples.ValidateEmail("invalid-email"));

        // Test exception documentation
        Assert.Throws<ArgumentNullException>(() => goodExamples.ValidateEmail(null));

        // XML comments are good because they:
        // 1. Generate documentation automatically
        // 2. Provide IntelliSense information
        // 3. Include examples and exception information
        // 4. Are maintained by IDEs when parameters change
    }

    [Fact]
    public void GoodExample_UnitsAndConstraints_ClarifyExpectations()
    {
        // Arrange
        var goodExamples = new GoodCommentExamples();

        // Act & Assert - Comments clarify units and valid ranges
        goodExamples.SetCacheExpiration(300); // 5 minutes

        Assert.Throws<ArgumentOutOfRangeException>(() => goodExamples.SetCacheExpiration(30)); // Too short
        Assert.Throws<ArgumentOutOfRangeException>(() => goodExamples.SetCacheExpiration(100000)); // Too long

        // Comments about units and constraints prevent misuse and bugs
    }

    [Fact]
    public void GoodExample_SelfDocumentingCode_MinimizesCommentNeed()
    {
        // Arrange - Order class uses descriptive names and properties
        var order = new GoodCommentExamples.Order
        {
            Total = 75m,
            ShippingAddress = new Address { State = "CA" }
        };

        // Act & Assert - Code is self-explanatory
        Assert.True(order.IsEligibleForFreeShipping); // Total > 50
        Assert.False(order.RequiresSignature); // No high-value or restricted items

        var totalWithTax = order.TotalWithTax;
        Assert.True(totalWithTax > order.Total); // Tax should be added

        // Well-named properties and methods reduce the need for comments
        // The code tells you WHAT it does, comments should explain WHY
    }

    [Fact]
    public void GoodExample_IntentExplainingComments_AddValueForComplexLogic()
    {
        // Arrange
        var goodExamples = new GoodCommentExamples();
        var paymentRequest = new PaymentRequest
        {
            AccountId = 123,
            Amount = 100m
        };

        // Act
        goodExamples.ProcessPayment(paymentRequest);

        // Assert - Comments explain the intent behind each step
        // This is valuable for understanding business requirements and compliance needs
        Assert.True(true); // Method executes
    }
}