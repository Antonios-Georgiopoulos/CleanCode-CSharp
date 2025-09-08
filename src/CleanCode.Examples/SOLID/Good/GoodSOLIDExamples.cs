namespace CleanCode.Examples.SOLID.Good;

using System;
using System.IO;


// SRP + DIP: separate responsibilities & depend on abstractions
public record Order(string CustomerEmail, decimal Price)
{
    public decimal Total => Price > 100 ? Price * 0.9m : Price;
}


public class OrderValidator
{
    public bool IsValid(Order order) => !string.IsNullOrWhiteSpace(order.CustomerEmail) && order.Price > 0;
}


public interface IOrderStore { void Save(Order order); }
public class FileOrderStore : IOrderStore
{
    public void Save(Order order) => File.AppendAllText("orders.txt", $"{order.CustomerEmail}:{order.Total}\n");
}


public interface INotifier { void Send(string to, string message); }
public class EmailNotifier : INotifier
{
    public void Send(string to, string message) => Console.WriteLine($"Email to {to}: {message}");
}


public class OrderProcessor
{
    private readonly OrderValidator _validator;
    private readonly IOrderStore _store;
    private readonly INotifier _notifier;


    public OrderProcessor(OrderValidator validator, IOrderStore store, INotifier notifier)
    { _validator = validator; _store = store; _notifier = notifier; }


    public void Process(Order order)
    {
        if (!_validator.IsValid(order)) throw new ArgumentException("Invalid order");
        _store.Save(order);
        _notifier.Send(order.CustomerEmail, $"Total: {order.Total}");
    }
}


// OCP: new shapes extend without modifying calculator
public abstract class Shape { public abstract double Area(); }
public class Circle : Shape { public double Radius { get; } public Circle(double r) { Radius = r; } public override double Area() => Math.PI * Radius * Radius; }
public class Rectangle : Shape { public double Width { get; } public double Height { get; } public Rectangle(double w, double h) { Width = w; Height = h; } public override double Area() => Width * Height; }
public class AreaCalculator { public double Calculate(Shape shape) => shape.Area(); }


// LSP: subtypes honor the base contract
public abstract class Bird { public abstract void Move(); }
public class Duck : Bird { public override void Move() => Console.WriteLine("Flying"); }
public class Penguin : Bird { public override void Move() => Console.WriteLine("Swimming"); }


// ISP: focused interfaces
public interface IWork { void Work(); }
public interface IEat { void Eat(); }
public class Robot : IWork { public void Work() => Console.WriteLine("Working"); }
public class Human : IWork, IEat { public void Work() => Console.WriteLine("Working"); public void Eat() => Console.WriteLine("Eating"); }