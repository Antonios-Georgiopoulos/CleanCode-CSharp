namespace CleanCode.CSharp.Tests;

public class ClassExamplesTests
{
    [Fact]
    public void BadExample_UserManager_DoesTooManyThings()
    {
        // Arrange - BAD: God object with too many responsibilities
        var badUserManager = new CleanCode.Examples.Classes.Bad.UserManager
        {
            ConnectionString = "test_connection",
            Errors = new List<string>(),
            IsDebugMode = true
        };

        // Act & Assert - This class handles validation, database, email, logging, etc.
        // It's impossible to test individual concerns in isolation
        try
        {
            badUserManager.CreateUser("John Doe", "john@example.com", "password123");
            // The method does too many things, making it hard to test specific behaviors
        }
        catch
        {
            // Expected to fail due to missing dependencies, but the point is
            // that this class has too many reasons to change
        }

        Assert.True(badUserManager.Errors != null); // At least we can verify it's initialized
    }

    [Fact]
    public void BadExample_UserManager_ExposesInternalState()
    {
        // Arrange
        var badUserManager = new CleanCode.Examples.Classes.Bad.UserManager();

        // Act - BAD: Public fields can be modified from outside
        badUserManager.ConnectionString = "malicious_connection";
        badUserManager.IsDebugMode = true;
        badUserManager.Errors = null; // This could break internal logic!

        // Assert - The class has no control over its internal state
        Assert.Equal("malicious_connection", badUserManager.ConnectionString);
        Assert.True(badUserManager.IsDebugMode);
        Assert.Null(badUserManager.Errors);
    }

    [Fact]
    public void BadExample_AnémicUser_HasNoBusinessLogic()
    {
        // Arrange - BAD: Just a data container with no behavior
        var anemicUser = new CleanCode.Examples.Classes.Bad.User
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            Salary = 50000
        };

        // Act - BAD: Business logic has to be implemented elsewhere
        // There's no user.Activate(), user.CalculateBonus(), etc.

        // All we can do is modify properties directly
        anemicUser.IsActive = false;
        anemicUser.Salary = -1000; // No validation!

        // Assert - The object doesn't protect its invariants
        Assert.False(anemicUser.IsActive);
        Assert.Equal(-1000, anemicUser.Salary); // Invalid state allowed
    }

    [Fact]
    public void BadExample_PenguinInheritance_ViolatesLiskovSubstitution()
    {
        // Arrange - BAD: Inheritance used incorrectly
        CleanCode.Examples.Classes.Bad.Bird penguin = new CleanCode.Examples.Classes.Bad.Penguin();

        // Act & Assert - BAD: Substitution principle violated
        // We expect all birds to be able to fly, but penguin throws exception
        Assert.Throws<NotSupportedException>(() => penguin.Fly());
    }

    [Fact]
    public void BadExample_StaticDependencies_MakeTestingDifficult()
    {
        // Arrange - BAD: Static dependencies can't be mocked or controlled
        var user = new CleanCode.Examples.Classes.Bad.User
        {
            Name = "Test User",
            Email = "test@example.com"
        };

        // Act - BAD: Can't test this in isolation due to static dependencies
        // UserService.CreateUser(user);

        // Assert - We can't verify the behavior without actually hitting
        // the database, sending emails, and writing to logs
        Assert.True(true); // Placeholder - actual testing would be problematic
    }

    [Fact]
    public void GoodExample_User_EncapsulatesBusinessLogic()
    {
        // Arrange - GOOD: Rich domain model with behavior
        var user = new CleanCode.Examples.Classes.Good.User(
            "John Doe",
            "john@example.com",
            "Developer",
            "Engineering",
            75000);

        // Act - GOOD: Business logic is encapsulated within the entity
        user.UpdateSalary(80000);
        user.Deactivate();

        // Assert - The object controls its own state and enforces invariants
        Assert.Equal(80000, user.Salary);
        Assert.False(user.IsActive);
        Assert.True(user.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void GoodExample_User_ValidatesInvariants()
    {
        // Arrange & Act & Assert - GOOD: Constructor validates input
        Assert.Throws<ArgumentException>(() =>
            new CleanCode.Examples.Classes.Good.User("", "john@example.com", "Dev", "Engineering", 50000));

        Assert.Throws<ArgumentException>(() =>
            new CleanCode.Examples.Classes.Good.User("John", "invalid-email", "Dev", "Engineering", 50000));

        Assert.Throws<ArgumentException>(() =>
            new CleanCode.Examples.Classes.Good.User("John", "john@example.com", "Dev", "Engineering", -1000));
    }

    [Fact]
    public void GoodExample_User_BusinessMethodsWorkCorrectly()
    {
        // Arrange
        var activeUser = new CleanCode.Examples.Classes.Good.User(
            "Jane Doe", "jane@example.com", "Manager", "Sales", 60000);

        var internUser = new CleanCode.Examples.Classes.Good.User(
            "Bob Smith", "bob@example.com", "Intern", "Intern", 0);

        // Act & Assert - GOOD: Domain behavior is clear and testable
        Assert.True(activeUser.IsEligibleForBonus());
        Assert.False(internUser.IsEligibleForBonus());

        activeUser.Deactivate();
        Assert.False(activeUser.IsEligibleForBonus()); // Inactive users not eligible
    }

    [Fact]
    public void GoodExample_UserValidator_HasSingleResponsibility()
    {
        // Arrange - GOOD: Single responsibility - only validates
        var validator = new CleanCode.Examples.Classes.Good.UserValidator();

        // Act - GOOD: Clear, focused behavior
        var validResult = validator.ValidateForCreation("John Doe", "john@example.com", "Dev", "Engineering");
        var invalidResult = validator.ValidateForCreation("", "invalid-email", "", "");

        // Assert - GOOD: Easy to test because it only does one thing
        Assert.True(validResult.IsValid);
        Assert.False(invalidResult.IsValid);
        Assert.Contains("Name is required", invalidResult.Errors);
        Assert.Contains("Email format is invalid", invalidResult.Errors);
    }

    [Fact]
    public void GoodExample_BirdInheritance_FollowsLiskovSubstitution()
    {
        // Arrange - GOOD: Proper inheritance design
        var birds = new CleanCode.Examples.Classes.Good.Bird[]
        {
            new CleanCode.Examples.Classes.Good.Eagle(),
            new CleanCode.Examples.Classes.Good.Penguin()
        };

        // Act & Assert - GOOD: All birds can move, but in their own way
        foreach (var bird in birds)
        {
            bird.Move(); // Each bird moves appropriately
            bird.Eat();  // All birds can eat
        }

        // GOOD: Composition allows flexible behavior combinations
        var duck = new CleanCode.Examples.Classes.Good.Duck();
        duck.Move(); // Walking
        duck.Fly();  // Flying
        duck.Swim(); // Swimming

        Assert.Equal("Eagle", birds[0].Name);
        Assert.Equal("Penguin", birds[1].Name);
    }

    [Fact]
    public void GoodExample_OrderCalculator_HasHighCohesion()
    {
        // Arrange - GOOD: All methods work together for order calculation
        var calculator = new CleanCode.Examples.Classes.Good.OrderCalculator();
        var order = new CleanCode.Examples.Classes.Good.Order
        {
            Items = new List<CleanCode.Examples.Classes.Good.OrderItem>
            {
                new() { Price = 100, Quantity = 2, Weight = 1.5m },
                new() { Price = 50, Quantity = 1, Weight = 0.5m }
            },
            Customer = new CleanCode.Examples.Classes.Good.Customer
            {
                DiscountPercentage = 0.1m,
                TaxRate = 0.08m
            },
            ShippingMethod = new CleanCode.Examples.Classes.Good.ShippingMethod()
        };

        // Act - GOOD: Single method that orchestrates cohesive calculations
        var total = calculator.CalculateTotal(order);

        // Assert - GOOD: All calculations work together
        Assert.Equal(250, total.Subtotal); // (100*2) + (50*1)
        Assert.Equal(25, total.Discount);  // 250 * 0.1
        Assert.Equal(18, total.Tax);       // (250-25) * 0.08
        Assert.Equal(8, total.Shipping);   // (1.5*2 + 0.5*1) -> 3.5kg -> ceil 4 * 2
        Assert.Equal(251, total.Total);    // 250 - 25 + 18 + 8
    }

    [Fact]
    public void GoodExample_DependencyInjection_EnablesTestability()
    {
        // Arrange - GOOD: Dependencies can be mocked for testing
        var mockValidator = new TestUserValidator();
        var mockRepository = new TestUserRepository();
        var mockNotificationService = new TestNotificationService();
        var mockLogger = new TestLogger();

        var userService = new CleanCode.Examples.Classes.Good.UserService(
            mockValidator, mockRepository, mockNotificationService, mockLogger);

        var request = new CleanCode.Examples.Classes.Good.CreateUserRequest(
            "John Doe", "john@example.com", "Developer", "Engineering", 75000);

        // Act - GOOD: Can test in isolation with controlled dependencies
        var result = userService.CreateUserAsync(request).Result;

        // Assert - GOOD: Behavior is predictable and testable
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.User);
        Assert.Equal("John Doe", result.User!.Name);

        // GOOD: Can verify interactions with dependencies
        Assert.True(mockValidator.ValidateWasCalled);
        Assert.True(mockRepository.SaveWasCalled);
        Assert.True(mockNotificationService.SendWelcomeEmailWasCalled);
    }

    [Fact]
    public void Comparison_ClassDesign_ShowsClearDifferences()
    {
        // BAD: Anemic model requires external logic
        var badUser = new CleanCode.Examples.Classes.Bad.User();
        badUser.Salary = -1000; // No validation
                                // Need external service to check bonus eligibility

        // GOOD: Rich model with encapsulated behavior
        var goodUser = new CleanCode.Examples.Classes.Good.User(
            "John", "john@example.com", "Dev", "Engineering", 75000);

        // Encapsulated validation
        Assert.Throws<ArgumentException>(() => goodUser.UpdateSalary(-1000));

        // Encapsulated business logic
        Assert.True(goodUser.IsEligibleForBonus());

        Assert.Equal(-1000, badUser.Salary); // Bad: allows invalid state
        Assert.Equal(75000, goodUser.Salary); // Good: maintains valid state
    }
}

// Test doubles for dependency injection testing
public class TestUserValidator : CleanCode.Examples.Classes.Good.UserValidator
{
    public bool ValidateWasCalled { get; private set; }

    public override CleanCode.Examples.Classes.Good.ValidationResult ValidateForCreation(
        string name, string email, string role, string department)
    {
        ValidateWasCalled = true;
        return new CleanCode.Examples.Classes.Good.ValidationResult(Array.Empty<string>());
    }
}

public class TestUserRepository : CleanCode.Examples.Classes.Good.UserRepository
{
    public bool SaveWasCalled { get; private set; }

    public TestUserRepository() : base(null!) { } // Null is OK for test

    public override async Task<CleanCode.Examples.Classes.Good.User> SaveAsync(
        CleanCode.Examples.Classes.Good.User user)
    {
        SaveWasCalled = true;
        await Task.CompletedTask;
        return user with { Id = 1 };
    }
}

public class TestNotificationService : CleanCode.Examples.Classes.Good.INotificationService
{
    public bool SendWelcomeEmailWasCalled { get; private set; }

    public async Task SendWelcomeEmailAsync(string email, string name)
    {
        SendWelcomeEmailWasCalled = true;
        await Task.CompletedTask;
    }

    public async Task SendNotificationAsync(string email, string subject, string message)
    {
        await Task.CompletedTask;
    }
}

public class TestLogger : CleanCode.Examples.Classes.Good.ILogger<CleanCode.Examples.Classes.Good.UserService>
{
    public void LogInformation(string message, params object[] args) { }
    public void LogError(Exception exception, string message, params object[] args) { }
}
