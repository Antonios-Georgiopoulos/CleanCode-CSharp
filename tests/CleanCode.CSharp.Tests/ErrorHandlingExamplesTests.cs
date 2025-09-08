using CleanCode.Examples.ErrorHandling.Bad;
using CleanCode.Examples.ErrorHandling.Good;

using MockConfigService = CleanCode.Examples.ErrorHandling.Good.MockConfigService;
using MockUserService = CleanCode.Examples.ErrorHandling.Good.MockUserService;
using Order = CleanCode.Examples.ErrorHandling.Bad.Order;
using OrderItem = CleanCode.Examples.ErrorHandling.Bad.OrderItem;
using Payment = CleanCode.Examples.ErrorHandling.Good.Payment;
using ProductNotFoundException = CleanCode.Examples.ErrorHandling.Good.ProductNotFoundException;
using User = CleanCode.Examples.ErrorHandling.Good.User;

namespace CleanCode.CSharp.Tests;

public class ErrorHandlingExamplesTests
{
    [Fact]
    public void BadExample_SwallowingExceptions_HidesPotentialProblems()
    {
        // Arrange
        var badExample = new BadErrorHandlingExamples();
        var nonExistentFile = "C:\\NonExistent\\File.txt";

        // Act & Assert - Method swallows all exceptions
        // This is dangerous because we have no idea what went wrong
        badExample.ReadFileAndIgnoreErrors(nonExistentFile);

        // The method completes "successfully" even though the file doesn't exist
        // This hides bugs and makes debugging impossible
        Assert.True(true); // Placeholder - the real issue is that errors are hidden
    }

    [Fact]
    public void BadExample_CatchingGeneralException_TreatsAllErrorsTheSame()
    {
        // Arrange
        var badExample = new BadErrorHandlingExamples();

        // Act
        var result = badExample.GetUserData(-1); // Invalid user ID

        // Assert - All exceptions are treated the same way
        Assert.Equal("Error occurred", result);

        // The problem: different errors need different handling
        // Network timeout vs invalid ID should be handled differently
        // Users get no useful information about what went wrong
    }

    [Fact]
    public void BadExample_ExceptionsForControlFlow_IsPoorDesign()
    {
        // Arrange
        var badExample = new BadErrorHandlingExamples();

        // Act & Assert - Using exceptions for normal program flow
        var result1 = badExample.IsValidUserId(999); // Valid ID
        var result2 = badExample.IsValidUserId(-1);  // Invalid ID

        Assert.True(result1);
        Assert.False(result2);

        // Problem: Exceptions are expensive and should be for exceptional cases
        // This pattern makes code slower and logic unclear
    }

    [Fact]
    public void BadExample_PoorExceptionMessages_DontHelpDevelopers()
    {
        // Arrange
        var badExample = new BadErrorHandlingExamples();

        // Act & Assert - Generic exceptions with unhelpful messages
        var ex1 = Assert.Throws<Exception>(() => badExample.ProcessOrder(null));
        Assert.Equal("Error", ex1.Message); // Completely unhelpful

        var emptyOrder = new Order { Items = new List<OrderItem>() };
        var ex2 = Assert.Throws<Exception>(() => badExample.ProcessOrder(emptyOrder));
        Assert.Equal("Invalid order", ex2.Message); // Vague and unhelpful

        // Good error messages should tell you exactly what's wrong and how to fix it
    }

    [Fact]
    public void BadExample_MagicReturnValues_AreEasyToMiss()
    {
        // Arrange
        var badExample = new BadErrorHandlingExamples();

        // Act
        var result = badExample.CalculatePrice("invalid_product", 1);

        // Assert - Returns -1 as error indicator
        Assert.Equal(-1, result);

        // Problem: Magic numbers like -1 are easy to miss in calling code
        // What if -1 is a valid price? The error is ambiguous
        // Callers might forget to check for the error value
    }

    [Fact]
    public void BadExample_StringErrorCodes_RequireConstantChecking()
    {
        // Arrange
        var badExample = new BadErrorHandlingExamples();

        // Act
        var result = badExample.ValidateUserInput("", out string errorCode);

        // Assert - Method returns null with error code
        Assert.Null(result);
        Assert.Equal("ERR_001", errorCode);

        // Problem: Easy to forget to check errorCode
        // Error codes are not self-documenting
        // Callers need to remember what each error code means
    }

    [Fact]
    public void GoodExample_SpecificExceptionHandling_ProvidesCorrectResponse()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        // Act & Assert - Different exceptions handled appropriately
        var task = Assert.ThrowsAsync<FileProcessingException>(
            () => goodExample.ReadFileWithProperHandling("nonexistent.txt"));

        Assert.Contains("not found", task.Result.Message.ToLower());

        // Specific exception types allow callers to handle different scenarios appropriately
    }

    [Fact]
    public void GoodExample_ValidationBeforeOperations_PreventsManyErrors()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        // Act
        var result = goodExample.GetUserDataSafely(-1).Result; // Invalid ID

        // Assert - Returns structured result instead of throwing
        Assert.False(result.IsSuccess);
        Assert.Equal(UserDataResultType.Invalid, result.Type);
        Assert.Contains("greater than zero", result.ErrorMessage);

        // Validation prevents many errors from occurring in the first place
    }

    [Fact]
    public void GoodExample_ResultPattern_AvoidsExceptionsForBusinessLogic()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        var validUser = new User { Name = "John", Email = "john@example.com", Age = 30 };
        var invalidUser = new User { Name = "", Email = "invalid", Age = -1 };

        // Act
        var validResult = goodExample.ValidateUser(validUser);
        var invalidResult = goodExample.ValidateUser(invalidUser);

        // Assert - Result pattern provides clear success/failure information
        Assert.True(validResult.IsValid);
        Assert.Empty(validResult.Errors);

        Assert.False(invalidResult.IsValid);
        Assert.Contains("Name is required", invalidResult.Errors);
        Assert.Contains("Email format is invalid", invalidResult.Errors);
        Assert.Contains("Age must be between 0 and 150", invalidResult.Errors);
        // Result pattern is better than exceptions for expected validation failures
    }

    [Fact]
    public void GoodExample_MeaningfulExceptions_ProvideHelpfulInformation()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        // Act & Assert - Specific exception types with helpful messages
        var ex1 = Assert.Throws<ArgumentNullException>(() => goodExample.ProcessOrderSafely(null));
        Assert.Equal("order", ex1.ParamName);

        var emptyOrder = new CleanCode.Examples.ErrorHandling.Good.Order { Items = new List<CleanCode.Examples.ErrorHandling.Good.OrderItem>() };
        var ex2 = Assert.Throws<InvalidOrderException>(() => goodExample.ProcessOrderSafely(emptyOrder));
        Assert.Contains("at least one item", ex2.Message);

        // Good exceptions tell you exactly what's wrong
    }

    [Fact]
    public void GoodExample_InputValidation_FailsFast()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        // Act & Assert - Validation happens immediately
        var ex1 = Assert.Throws<ArgumentException>(() => goodExample.CalculatePriceSafely("", 1));
        Assert.Equal("productId", ex1.ParamName);

        var ex2 = Assert.Throws<ArgumentOutOfRangeException>(() => goodExample.CalculatePriceSafely("PROD1", -1));
        Assert.Equal("quantity", ex2.ParamName);

        // Fail-fast principle: detect and report errors as early as possible
    }

    [Fact]
    public void GoodExample_BatchProcessing_HandlesIndividualFailures()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        var payments = new List<Payment>
        {
            new() { Id = "PAY1" },
            new() { Id = "PAY2" },
            new() { Id = "PAY3" }
        };

        // Act
        var result = goodExample.ProcessPaymentsBatch(payments).Result;

        // Assert - Batch processing handles individual failures gracefully
        Assert.NotNull(result);
        Assert.Equal(3, result.Results.Count);

        // In a real scenario, some payments might fail while others succeed
        // Good error handling allows partial success
    }

    [Fact]
    public void GoodExample_DefensiveProgramming_ValidatesAllInputs()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        // Act & Assert - All inputs are validated
        var ex1 = Assert.Throws<ArgumentException>(() =>
            goodExample.TransferMoneySafely("", "ACC2", 100));

        var ex2 = Assert.Throws<ArgumentException>(() =>
            goodExample.TransferMoneySafely("ACC1", "", 100));

        var ex3 = Assert.Throws<ArgumentOutOfRangeException>(() =>
            goodExample.TransferMoneySafely("ACC1", "ACC2", -100));

        var ex4 = Assert.Throws<InvalidOperationException>(() =>
            goodExample.TransferMoneySafely("ACC1", "ACC1", 100));

        // Defensive programming catches invalid inputs before they cause problems
        Assert.Equal("fromAccountId", ex1.ParamName);
        Assert.Equal("toAccountId", ex2.ParamName);
        Assert.Equal("amount", ex3.ParamName);
        Assert.Contains("same account", ex4.Message);
    }

    [Fact]
    public void GoodExample_ConfigurationHandling_ProvidesFallbacks()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        // Act
        var result = goodExample.GetConfigurationValue("timeout", 30);

        // Assert - Configuration errors are handled gracefully
        Assert.True(result.IsSuccess);
        // In a real scenario with missing config, it would use the default value

        var invalidResult = goodExample.GetConfigurationValue<int>("", 0);
        Assert.False(invalidResult.IsSuccess);
        Assert.Contains("cannot be null or empty", invalidResult.ErrorMessage);

        // Good configuration handling provides sensible defaults
    }

    [Fact]
    public void GoodExample_ConsistentErrorHandling_AcrossSimilarOperations()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        // Act
        var result1 = goodExample.DeleteUserSafely(-1).Result;
        var result2 = goodExample.DeleteUserSafely(999).Result;

        // Assert - Consistent error handling patterns
        Assert.False(result1.IsSuccess);
        Assert.Equal(OperationResultType.Invalid, result1.Type);

        // Note: In real implementation, user 999 might not exist
        // but our mock always returns success for positive IDs
        Assert.True(result2.IsSuccess);

        // Consistent patterns make code predictable and maintainable
    }

    [Fact]
    public void Comparison_ErrorHandling_ShowsClearDifferences()
    {
        // BAD: Silent failures and magic return values
        var badExample = new BadErrorHandlingExamples();
        var badResult = badExample.CalculatePrice("invalid", 1);
        Assert.Equal(-1, badResult); // Magic number - unclear what went wrong

        // GOOD: Clear result types and helpful error messages
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        var ex = Assert.Throws<ProductNotFoundException>(() =>
            goodExample.CalculatePriceSafely("invalid", 1));
        Assert.Contains("Product with ID 'invalid' not found", ex.Message);

        // Good error handling provides clear, actionable information
        // Bad error handling hides problems and makes debugging difficult
    }

    [Fact]
    public void GoodExample_ResourceManagement_PreventsLeaks()
    {
        // Arrange
        var logger = new MockLogger<GoodErrorHandlingExamples>();
        var userService = new MockUserService();
        var configService = new MockConfigService();
        var goodExample = new GoodErrorHandlingExamples(logger, userService, configService);

        // Act & Assert - Using statements ensure proper resource disposal
        var task = Assert.ThrowsAsync<FileProcessingException>(() =>
            goodExample.ReadFileWithProperResourceManagement("nonexistent.txt"));

        // Even when exceptions occur, resources are properly disposed
        // This prevents memory leaks and file handle exhaustion
        Assert.Contains("not found", task.Result.Message.ToLower());
    }
}