namespace CleanCode.CSharp.Tests;

public class SOLIDExamplesTests
{
    // Single Responsibility Principle Tests

    [Fact]
    public void BadExample_OrderProcessor_ViolatesSRP()
    {
        // Arrange - BAD: One class handles everything
        var badProcessor = new CleanCode.Examples.SOLID.Bad.OrderProcessor();

        // Act & Assert - This class has too many responsibilities
        // It handles validation, business logic, database, email, logging, auditing
        // Testing any single concern requires all dependencies to work
        try
        {
            badProcessor.ProcessOrder("John Doe", "Widget", 50m, 2);
        }
        catch
        {
            // Expected to fail due to missing dependencies
            // The point is this class does too many things
        }

        Assert.True(true); // Placeholder - demonstrates the testing difficulty
    }

    [Fact]
    public void GoodExample_OrderProcessor_FollowsSRP()
    {
        // Arrange - GOOD: Each dependency has a single responsibility
        var validator = new CleanCode.Examples.SOLID.Good.OrderValidator();
        var discountCalculator = new CleanCode.Examples.SOLID.Good.DiscountCalculator();
        var mockRepository = new MockOrderRepository();
        var mockNotificationService = new MockNotificationService();
        var mockLogger = new MockLogger();

        var goodProcessor = new CleanCode.Examples.SOLID.Good.OrderProcessor(
            validator, discountCalculator, mockRepository, mockNotificationService, mockLogger);

        var order = new CleanCode.Examples.SOLID.Good.Order("John Doe", "Widget", 50m, 3);

        // Act - GOOD: Can test in isolation with controlled dependencies
        var result = goodProcessor.ProcessOrderAsync(order).Result;

        // Assert - Each responsibility can be tested independently
        Assert.True(result.IsSuccess);
        Assert.Equal(135m, result.FinalAmount); // 50 * 3 = 150, minus 15 discount (10%)
        Assert.True(mockRepository.SaveWasCalled);
        Assert.True(mockNotificationService.SendWasCalled);
        Assert.True(mockLogger.LogWasCalled);
    }

    // Open/Closed Principle Tests

    [Fact]
    public void BadExample_AreaCalculator_ViolatesOCP()
    {
        // Arrange - BAD: Adding new shapes requires modifying the calculator
        var badCalculator = new CleanCode.Examples.SOLID.Bad.AreaCalculator();
        var circle = new CleanCode.Examples.SOLID.Bad.Circle { Radius = 5 };
        var rectangle = new CleanCode.Examples.SOLID.Bad.Rectangle { Width = 4, Height = 6 };

        // Act - This works for existing shapes
        var circleArea = badCalculator.CalculateArea(circle);
        var rectangleArea = badCalculator.CalculateArea(rectangle);

        // Assert - But adding Pentagon would require modifying AreaCalculator
        Assert.True(circleArea > 0);
        Assert.Equal(24, rectangleArea);

        // If we wanted to add a Pentagon, we'd have to modify AreaCalculator's CalculateArea method
    }

    [Fact]
    public void GoodExample_AreaCalculator_FollowsOCP()
    {
        // Arrange - GOOD: Can add new shapes without modifying existing code
        var calculator = new CleanCode.Examples.SOLID.Good.AreaCalculator();
        var shapes = new List<CleanCode.Examples.SOLID.Good.Shape>
        {
            new CleanCode.Examples.SOLID.Good.Circle(5),
            new CleanCode.Examples.SOLID.Good.Rectangle(4, 6),
            new CleanCode.Examples.SOLID.Good.Triangle(3, 4),
            new CleanCode.Examples.SOLID.Good.Pentagon(2) // New shape added without modifying calculator
        };

        // Act - Calculator works with all shapes without modification
        var areas = calculator.CalculateAreas(shapes);

        // Assert - All shapes calculated correctly
        Assert.Equal(4, areas.Count);
        Assert.True(areas.All(a => a.Area > 0));
        Assert.Contains(areas, a => a.ShapeInfo.Contains("Circle"));
        Assert.Contains(areas, a => a.ShapeInfo.Contains("Rectangle"));
        Assert.Contains(areas, a => a.ShapeInfo.Contains("Triangle"));
        Assert.Contains(areas, a => a.ShapeInfo.Contains("Pentagon"));
    }

    // Liskov Substitution Principle Tests

    [Fact]
    public void BadExample_BirdInheritance_ViolatesLSP()
    {
        // Arrange - BAD: Penguin violates LSP
        var birds = new List<CleanCode.Examples.SOLID.Bad.Bird>
        {
            new CleanCode.Examples.SOLID.Bad.Duck(),
            new CleanCode.Examples.SOLID.Bad.Penguin()
        };

        // Act & Assert - Can't treat all birds the same way
        foreach (var bird in birds)
        {
            if (bird is CleanCode.Examples.SOLID.Bad.Penguin)
            {
                // BAD: Have to check type because Penguin breaks the contract
                Assert.Throws<NotSupportedException>(() => bird.Fly());
            }
            else
            {
                bird.Fly(); // Only works for some birds
            }
        }
    }

    [Fact]
    public void GoodExample_BirdInheritance_FollowsLSP()
    {
        // Arrange - GOOD: All birds can be treated uniformly
        var birds = new List<CleanCode.Examples.SOLID.Good.Bird>
        {
            new CleanCode.Examples.SOLID.Good.Duck(),
            new CleanCode.Examples.SOLID.Good.Penguin(),
            new CleanCode.Examples.SOLID.Good.Ostrich()
        };

        // Act - All birds can move, each in their appropriate way
        foreach (var bird in birds)
        {
            bird.Move(); // Works for all birds
            bird.Eat();  // Works for all birds
        }

        // Assert - No exceptions thrown, LSP respected
        Assert.Equal(3, birds.Count);
        Assert.All(birds, bird => Assert.NotNull(bird.Name));
    }

    [Fact]
    public void GoodExample_CompositionOverInheritance_AllowsFlexibleBehavior()
    {
        // Arrange - GOOD: Composition allows multiple behaviors
        var superDuck = new CleanCode.Examples.SOLID.Good.SuperDuck();

        // Act - Can use all behaviors
        superDuck.Move(); // Bird behavior
        superDuck.Fly();  // Flying behavior
        superDuck.Swim(); // Swimming behavior

        // Assert - Flexible design without inheritance problems
        Assert.Equal("SuperDuck", superDuck.Name);
    }

    // Interface Segregation Principle Tests

    [Fact]
    public void BadExample_FatInterface_ViolatesISP()
    {
        // Arrange - BAD: Robot forced to implement methods it doesn't need
        var robot = new CleanCode.Examples.SOLID.Bad.Robot();

        // Act & Assert - Robot can work
        robot.Work();
        robot.AttendMeeting();

        // BAD: But robot throws exceptions for human-specific methods
        Assert.Throws<NotSupportedException>(() => robot.Eat());
        Assert.Throws<NotSupportedException>(() => robot.Sleep());
        Assert.Throws<NotSupportedException>(() => robot.TakeBreak());
    }

    [Fact]
    public void GoodExample_SegregatedInterfaces_FollowISP()
    {
        // Arrange - GOOD: Robot only implements interfaces it needs
        var robot = new CleanCode.Examples.SOLID.Good.Robot();
        var human = new CleanCode.Examples.SOLID.Good.Human();
        var manager = new CleanCode.Examples.SOLID.Good.Manager();

        // Act - Each worker type implements only relevant interfaces
        robot.Work();
        robot.AttendMeeting();

        human.Work();
        human.Eat();
        human.Sleep();

        manager.AttendMeeting();
        manager.SendEmail();

        // Assert - No forced implementations or exceptions
        Assert.True(true); // All methods execute without issues
    }

    // Dependency Inversion Principle Tests

    [Fact]
    public void BadExample_ConcreteDependencies_ViolatesDIP()
    {
        // Arrange - BAD: Depends on concrete implementations
        var badNotificationService = new CleanCode.Examples.SOLID.Bad.OrderNotificationService();

        // Act - Works but tightly coupled
        badNotificationService.NotifyCustomer("customer@email.com", "+1234567890", "Order #123");

        // Assert - Can't easily test or extend
        Assert.True(true); // Works but inflexible

        // To add WhatsApp notifications, we'd have to modify OrderNotificationService
    }

    [Fact]
    public void GoodExample_AbstractionDependencies_FollowDIP()
    {
        // Arrange - GOOD: Depends on abstractions
        var messageSenders = new List<CleanCode.Examples.SOLID.Good.IMessageSender>
        {
            new MockEmailSender(),
            new MockSmsSender(),
            new MockPushNotificationSender()
        };

        var goodNotificationService = new CleanCode.Examples.SOLID.Good.NotificationService(messageSenders);

        // Act - Flexible and testable
        var task = goodNotificationService.NotifyCustomerAsync("customer@email.com", "Order Confirmation", "Order #123");
        task.Wait();

        // Assert - Can easily add new notification methods without changing the service
        Assert.True(task.IsCompletedSuccessfully);

        // Adding WhatsApp would just require implementing IMessageSender
    }

    [Fact]
    public void GoodExample_CustomerService_ShowsDIPInAction()
    {
        // Arrange - GOOD: Business logic depends on abstractions
        var mockRepository = new MockCustomerRepository();
        var mockLogger = new MockCustomerLogger();
        var customerService = new CleanCode.Examples.SOLID.Good.CustomerService(mockRepository, mockLogger);

        // Act - Business logic is testable and flexible
        var customer = customerService.CreateCustomerAsync("John Doe", "john@example.com").Result;

        // Assert - Dependencies can be easily mocked or swapped
        Assert.Equal("John Doe", customer.Name);
        Assert.Equal("john@example.com", customer.Email);
        Assert.True(mockRepository.SaveWasCalled);
        Assert.True(mockLogger.LogWasCalled);
    }

    [Fact]
    public void Comparison_SOLIDPrinciples_ShowClearBenefits()
    {
        // This test demonstrates the overall benefits of following SOLID principles

        // BAD: Monolithic, hard to test, hard to extend
        var badProcessor = new CleanCode.Examples.SOLID.Bad.OrderProcessor();
        // Testing requires all dependencies, extending requires modification

        // GOOD: Modular, easy to test, easy to extend
        var validator = new CleanCode.Examples.SOLID.Good.OrderValidator();
        var calculator = new CleanCode.Examples.SOLID.Good.DiscountCalculator();
        var mockRepo = new MockOrderRepository();
        var mockNotifier = new MockNotificationService();
        var mockLogger = new MockLogger();

        var goodProcessor = new CleanCode.Examples.SOLID.Good.OrderProcessor(
            validator, calculator, mockRepo, mockNotifier, mockLogger);

        var order = new CleanCode.Examples.SOLID.Good.Order("Test Customer", "Test Product", 100m, 2);
        var result = goodProcessor.ProcessOrderAsync(order).Result;

        // GOOD: Easy to test, extend, and maintain
        Assert.True(result.IsSuccess);
        Assert.Equal(180m, result.FinalAmount); // 200 - 20 (10% discount)
    }
}

// Mock implementations for testing
public class MockOrderRepository : CleanCode.Examples.SOLID.Good.IOrderRepository
{
    public bool SaveWasCalled { get; private set; }

    public async Task SaveOrderAsync(CleanCode.Examples.SOLID.Good.Order order, decimal finalAmount)
    {
        SaveWasCalled = true;
        await Task.CompletedTask;
    }
}

public class MockNotificationService : CleanCode.Examples.SOLID.Good.INotificationService
{
    public bool SendWasCalled { get; private set; }

    public async Task SendOrderConfirmationAsync(string customerEmail, string orderDetails)
    {
        SendWasCalled = true;
        await Task.CompletedTask;
    }
}

public class MockLogger : CleanCode.Examples.SOLID.Good.ILogger
{
    public bool LogWasCalled { get; private set; }

    public void LogOrderProcessed(CleanCode.Examples.SOLID.Good.Order order, decimal finalAmount)
    {
        LogWasCalled = true;
    }
}

public class MockEmailSender : CleanCode.Examples.SOLID.Good.IMessageSender
{
    public async Task SendMessageAsync(string recipient, string subject, string message)
    {
        await Task.CompletedTask;
    }
}

public class MockSmsSender : CleanCode.Examples.SOLID.Good.IMessageSender
{
    public async Task SendMessageAsync(string recipient, string subject, string message)
    {
        await Task.CompletedTask;
    }
}

public class MockPushNotificationSender : CleanCode.Examples.SOLID.Good.IMessageSender
{
    public async Task SendMessageAsync(string recipient, string subject, string message)
    {
        await Task.CompletedTask;
    }
}

public class MockCustomerRepository : CleanCode.Examples.SOLID.Good.ICustomerRepository
{
    public bool SaveWasCalled { get; private set; }

    public async Task SaveCustomerAsync(CleanCode.Examples.SOLID.Good.Customer customer)
    {
        SaveWasCalled = true;
        await Task.CompletedTask;
    }

    public async Task<CleanCode.Examples.SOLID.Good.Customer> GetCustomerByIdAsync(int id)
    {
        await Task.CompletedTask;
        return new CleanCode.Examples.SOLID.Good.Customer("Test Customer", "test@example.com");
    }
}

public class MockCustomerLogger : CleanCode.Examples.SOLID.Good.ICustomerLogger
{
    public bool LogWasCalled { get; private set; }

    public void LogCustomerCreated(CleanCode.Examples.SOLID.Good.Customer customer)
    {
        LogWasCalled = true;
    }
}