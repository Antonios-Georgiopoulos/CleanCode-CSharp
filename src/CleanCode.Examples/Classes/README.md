# Classes

## Overview

Well-designed classes form the backbone of maintainable software. They should have clear responsibilities, proper encapsulation, and high cohesion. Poor class design leads to code that's difficult to understand, test, and modify.

## Key Principles

**Single Responsibility** - A class should have only one reason to change and serve a single purpose.

**Proper Encapsulation** - Hide internal implementation details and protect object invariants.

**High Cohesion** - All methods and properties should work together toward the class's single purpose.

**Loose Coupling** - Classes should depend on abstractions, not concrete implementations.

**Rich Domain Models** - Objects should contain both data and the behavior that operates on that data.

## Bad Examples

### God Object
```csharp
class UserManager {
    // Handles validation, database, email, logging, business rules, caching...
    void CreateUser(...) // Does everything!
}
```
**Problem:** This class violates Single Responsibility by handling multiple concerns, making it hard to modify and understand.

### Poor Encapsulation
```csharp
class User {
    public string ConnectionString;  // Exposed internal state
    public List<string> Errors;     // Can be set to null externally
    public bool IsDebugMode;        // No control over when it changes
}
```
**Problem:** Public fields expose internal implementation and allow external code to break object invariants.

### Anemic Domain Model
```csharp
class User {
    public string Name { get; set; }
    public decimal Salary { get; set; }
    // No business logic - just a data container
}
```
**Problem:** Objects that only hold data without behavior push all logic into service classes, missing opportunities for encapsulation.

### Inheritance Misuse
```csharp
class Penguin : Bird {
    public override void Fly() {
        throw new NotSupportedException(); // Violates Liskov Substitution
    }
}
```
**Problem:** Inheritance used incorrectly when "Is-A" relationship doesn't truly exist.

### Static Dependencies
```csharp
static class UserService {
    public static void CreateUser(User user) {
        DatabaseHelper.ExecuteQuery(...);  // Tightly coupled
        EmailService.SendEmail(...);       // Hard to modify
    }
}
```
**Problem:** Static dependencies create tight coupling and make the code inflexible.

## Good Examples

### Single Responsibility Classes
```csharp
class UserValidator     // Only validates
class UserRepository    // Only handles persistence  
class EmailService      // Only sends emails
class UserService       // Orchestrates the process
```
**Benefit:** Each class has one clear purpose and can evolve independently.

### Rich Domain Model
```csharp
class User {
    public User(string name, ...) { ValidateInputs(...); }
    public void UpdateSalary(decimal newSalary) { /* Validation + update */ }
    public bool IsEligibleForBonus() { /* Business logic */ }
}
```
**Benefit:** Business logic is encapsulated within the entity, making the model self-protecting and expressive.

### Proper Encapsulation
```csharp
class User {
    public string Name { get; private set; }        // Controlled access
    private void ValidateInputs(...) { ... }        // Hidden implementation
    public void UpdateSalary(decimal newSalary) {   // Controlled modification
        if (newSalary <= 0) throw new ArgumentException(...);
        Salary = newSalary;
    }
}
```
**Benefit:** Internal state is protected and modifications go through controlled methods that maintain invariants.

### Dependency Injection
```csharp
class UserService {
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;
    
    public UserService(IUserRepository repository, IEmailService emailService) {
        _repository = repository;
        _emailService = emailService;
    }
}
```
**Benefit:** Dependencies are injected as interfaces, enabling flexibility and easier modification.

### Composition Over Inheritance
```csharp
interface IFlyable { void Fly(); }
interface ISwimmable { void Swim(); }

class Duck : Bird, IFlyable, ISwimmable {
    // Implements multiple behaviors through composition
}
```
**Benefit:** Behavior is composed through interfaces rather than forced inheritance hierarchies.

## Class Design Guidelines

**Start with behavior, not data** - Think about what the class does before what it contains.

**Keep classes small** - If a class is hard to name or does multiple things, split it.

**Use constructor validation** - Ensure objects are created in a valid state.

**Prefer immutability** - Make objects immutable when possible to avoid state corruption.

**Follow the Law of Demeter** - Objects should only talk to their immediate friends.

**Depend on abstractions** - Use interfaces and abstract classes to reduce coupling.

## Common Design Patterns

**Repository Pattern** - Encapsulate data access logic.

**Service Layer** - Coordinate business operations across multiple entities.

**Factory Pattern** - Encapsulate complex object creation logic.

**Strategy Pattern** - Encapsulate algorithms and make them interchangeable.

**Observer Pattern** - Enable loose coupling between objects that need to communicate.

## Impact

**Bad class design creates technical debt:** Tightly coupled, poorly encapsulated classes become increasingly difficult to modify as the system grows.

**Good class design enables growth:** Well-designed classes with clear responsibilities and proper encapsulation make the codebase more maintainable and extensible.