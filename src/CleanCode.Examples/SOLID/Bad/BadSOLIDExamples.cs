namespace CleanCode.Examples.SOLID.Bad;

using System;
using System.IO;


// SRP violation: validation + business + persistence + notification
public class OrderService
{
    public void Process(string customerEmail, decimal price)
    {
        if (string.IsNullOrWhiteSpace(customerEmail) || price <= 0)
            throw new ArgumentException("Invalid order");


        var total = price > 100 ? price * 0.9m : price; // business
        File.AppendAllText("orders.txt", $"{customerEmail}:{total}\n"); // persistence
        new EmailSender().Send(customerEmail, "Order", $"Total: {total}"); // notification
    }
}


// OCP violation: type checks for each shape
public class AreaCalculator
{
    public double Calculate(object shape)
    {
        if (shape is Circle c) return Math.PI * c.Radius * c.Radius;
        if (shape is Rectangle r) return r.Width * r.Height;
        throw new ArgumentException("Unknown shape");
    }
}


public class Circle { public double Radius { get; set; } }
public class Rectangle { public double Width { get; set; } public double Height { get; set; } }


// LSP violation: subclass breaks base contract
public class Bird { public virtual void Fly() => Console.WriteLine("Flying"); }
public class Penguin : Bird { public override void Fly() => throw new NotSupportedException("Penguins can't fly"); }


// ISP violation: fat interface forces unused members
public interface IWorker { void Work(); void Eat(); }
public class Robot : IWorker
{
    public void Work() => Console.WriteLine("Working");
    public void Eat() => throw new NotSupportedException();
}


// DIP violation: high-level depends on concretion
public class EmailSender { public void Send(string to, string subject, string body) => Console.WriteLine($"Email to {to}: {subject}"); }
public class OrderNotifier
{
    private readonly EmailSender _email = new EmailSender(); // concrete dependency
    public void Notify(string email, string message) => _email.Send(email, "Order", message);
}