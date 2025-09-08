## SOLID Principles — Minimal Guide

SOLID helps you write code that is easy to change and reason about. Below are tiny, focused examples.

### S — Single Responsibility Principle (SRP)
**Bad**
```csharp
class OrderService { /* validates + calculates + saves + emails */ }
```
**Good**
```csharp
record Order(string Email, decimal Price);
class OrderValidator { bool IsValid(Order o) => !string.IsNullOrWhiteSpace(o.Email) && o.Price > 0; }
interface IOrderStore { void Save(Order o); }
interface INotifier { void Send(string to, string msg); }
class OrderProcessor { /* orchestrates using the above */ }
```

### O — Open/Closed Principle (OCP)
**Bad**
```csharp
double Calculate(object s) { if (s is Circle c) return ...; if (s is Rectangle r) return ...; }
```
**Good**
```csharp
abstract class Shape { public abstract double Area(); }
class Circle(double r) : Shape { public override double Area() => Math.PI * r * r; }
class Rectangle(double w, double h) : Shape { public override double Area() => w * h; }
```

### L — Liskov Substitution Principle (LSP)
**Bad**
```csharp
class Penguin : Bird { public override void Fly() => throw new NotSupportedException(); }
```
**Good**
```csharp
abstract class Bird { public abstract void Move(); }
class Duck : Bird { public override void Move() => Console.WriteLine("Flying"); }
class Penguin : Bird { public override void Move() => Console.WriteLine("Swimming"); }
```

### I — Interface Segregation Principle (ISP)
**Bad**
```csharp
interface IWorker { void Work(); void Eat(); }
class Robot : IWorker { /* Eat() not applicable */ }
```
**Good**
```csharp
interface IWork { void Work(); } interface IEat { void Eat(); }
class Robot : IWork { public void Work() { /* ... */ } }
```

### D — Dependency Inversion Principle (DIP)
**Bad**
```csharp
class OrderNotifier { EmailSender _email = new(); }
```
**Good**
```csharp
interface INotifier { void Send(string to, string msg); }
class OrderProcessor(INotifier notifier) { /* depends on abstraction */ }
```

---

### Checklist
- One class → one reason to change.
- New behavior via **extension**, not modification.
- Subtypes never weaken base class expectations.
- Small, focused interfaces only.
- High‑level code depends on **abstractions**.
