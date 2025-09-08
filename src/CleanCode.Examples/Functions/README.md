# Functions

## Overview

Well-designed functions are the building blocks of clean code. They should be small, focused, and do one thing well. Poor function design leads to code that's hard to understand, test, and maintain.

## Key Principles

**Single Responsibility** - Each function should have one reason to change and do one thing well.

**Small Size** - Functions should be short enough to understand at a glance (typically 5-20 lines).

**Few Parameters** - Ideally 0-3 parameters. More than 3 suggests the function is doing too much.

**No Side Effects** - Functions should not have hidden behaviors that aren't obvious from their name.

**Consistent Abstraction Level** - All statements in a function should be at the same level of abstraction.

## Bad Examples

### Doing Too Many Things
```csharp
public string ProcessUserAndGenerateReport(string name, string email, int age, ...) {
    // Validation
    if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name required");
    
    // Business logic
    var bonus = department == "Sales" ? salary * 0.1 : 0;
    
    // Database operations
    SaveToDatabase(user);
    
    // Email sending
    SendWelcomeEmail(email);
    
    // Report generation
    var report = $"User: {name}...";
    
    // Logging
    Console.WriteLine($"Processed user {name}");
    
    return report;
}
```
**Problem:** This method violates Single Responsibility Principle by handling validation, business logic, database operations, email sending, and reporting.

### Too Many Parameters
```csharp
public void CreateOrder(string customerName, string customerEmail, string customerPhone,
    string customerAddress, string productName, double productPrice, int quantity,
    double discountPercent, string couponCode, bool expressShipping,
    string shippingAddress, DateTime deliveryDate, string paymentMethod,
    string creditCardNumber, string expiryDate, string cvv) {
    // Implementation
}
```
**Problem:** Methods with many parameters are hard to call, understand, and maintain. They often indicate missing abstractions.

### Deep Nesting
```csharp
public string ProcessData(object data) {
    if (data != null) {
        if (data is string str) {
            if (!string.IsNullOrEmpty(str)) {
                if (str.Length > 10) {
                    if (str.Contains("@")) {
                        // More nesting...
                    }
                }
            }
        }
    }
    return "Processed";
}
```
**Problem:** Deep nesting makes code hard to follow and understand the flow.

### Hidden Side Effects
```csharp
public int CalculateTotal(List<int> numbers) {
    GlobalCounter++;           // Hidden: modifies global state
    Console.WriteLine(...);    // Hidden: logs to console
    TrackUsage(...);          // Hidden: sends analytics
    return numbers.Sum();
}
```
**Problem:** Method name suggests pure calculation but performs hidden operations.

### Boolean Parameters
```csharp
ProcessFile("data.txt", true, false, true); // What do these booleans mean?
```
**Problem:** Boolean flags make method calls unclear and often indicate the method does multiple things.

### Inconsistent Return Types
```csharp
public object GetUserInfo(int id) {
    if (id > 0) return new { Name = "John", Age = 30 };  // Anonymous object
    if (id == 0) return "Guest User";                    // String
    return null;                                         // Null
}
```
**Problem:** Unpredictable return types make code unreliable and hard to work with.

## Good Examples

### Single Responsibility
```csharp
public UserProcessingResult ProcessUser(CreateUserRequest request) {
    ValidateUserRequest(request);     // Only validates
    var user = CreateUserFromRequest(request);  // Only creates user object
    var bonus = CalculateUserBonus(user);       // Only calculates bonus
    var savedUser = SaveUser(user);             // Only saves to database
    
    if (user.IsActive) {
        SendWelcomeEmail(user.Email);           // Only sends email
    }
    
    var report = GenerateUserReport(savedUser, bonus);  // Only generates report
    LogUserProcessing(savedUser);               // Only logs
    
    return new UserProcessingResult(savedUser, report, bonus);
}
```
**Benefit:** Each method has one clear purpose and can be tested independently.

### Well-Defined Parameters
```csharp
public OrderResult CreateOrder(CreateOrderRequest request) {
    // Single, meaningful parameter object instead of many primitives
}

public record CreateOrderRequest(
    CustomerInfo CustomerInfo,
    List<Product> Products,
    ShippingInfo ShippingInfo,
    PaymentInfo PaymentInfo
);
```
**Benefit:** Using parameter objects instead of primitive obsession makes code more readable and maintainable.

### Early Returns
```csharp
public string ValidateEmailAddress(string input) {
    if (input == null) return "Input cannot be null";
    if (string.IsNullOrEmpty(input)) return "Email cannot be empty";
    if (input.Length <= 10) return "Email is too short";
    if (!input.Contains("@")) return "Email must contain @ symbol";
    
    // Continue with main logic...
    return "Valid email";
}
```
**Benefit:** Early returns eliminate deep nesting and make the happy path clear.

### Pure Functions
```csharp
public int CalculateSum(IEnumerable<int> numbers) {
    return numbers?.Sum() ?? 0; // No side effects, predictable output
}
```
**Benefit:** Pure functions are easier to test, debug, and reason about.

### Explicit Side Effects
```csharp
public string GetUserNameById(int userId) {
    // Pure function - only retrieves name
    var user = GetUserFromDatabase(userId);
    return user?.Name ?? "Unknown";
}

public string GetUserNameByIdWithTracking(int userId) {
    // Explicitly indicates side effects in the name
    var userName = GetUserNameById(userId);
    TrackUserAccess(userId);
    return userName;
}
```
**Benefit:** Separating pure functions from those with side effects improves clarity.

### Separate Methods Instead of Boolean Flags
```csharp
// Instead of: ProcessFile(filename, true, false, true)
// Use explicit methods:

public void ProcessFileWithValidation(string filename) {
    ValidateFile(filename);
    ProcessFile(filename);
}

public void ProcessFileWithLogging(string filename) {
    ProcessFile(filename);
    LogFileProcessing(filename);
}

public void ProcessFileWithBackup(string filename) {
    BackupFile(filename);
    ProcessFile(filename);
}
```
**Benefit:** Method names clearly indicate intent without ambiguous boolean parameters.

### Consistent Return Types
```csharp
public UserInfo GetUserInfo(int id) {
    if (id <= 0)
        return UserInfo.CreateGuestUser();
    
    var user = GetUserFromDatabase(id);
    return user != null
        ? UserInfo.FromUser(user)
        : UserInfo.CreateUnknownUser();
}
```
**Benefit:** Always returns the same type, making code predictable and reliable.

## Function Design Guidelines

### Naming
- Use verbs for functions (Calculate, Process, Validate)
- Be specific about what they do
- Avoid abbreviations and unclear names
- Match the name to exactly what the function does

### Parameters
- Aim for 0-3 parameters maximum
- Use parameter objects for related data
- Avoid boolean flags - use separate methods instead
- Put parameters in logical order

### Return Values
- Be consistent with return types
- Use meaningful return types instead of primitives
- Consider using Result patterns for operations that can fail
- Avoid returning null when possible

### Side Effects
- Minimize side effects in functions
- Make side effects explicit in function names
- Separate query operations from command operations
- Use pure functions when possible

## Common Refactoring Patterns

**Extract Method**: Break large functions into smaller, focused ones.

**Parameter Object**: Group related parameters into a single object.

**Replace Parameter with Method Call**: If a parameter can be derived, calculate it inside the method.

**Separate Query from Modifier**: Don't mix functions that return values with those that modify state.

**Replace Conditional with Polymorphism**: Use inheritance/interfaces instead of large switch statements.

## Function Length Guidelines

**Ideal**: 5-20 lines for most functions

**Warning Signs**: Functions longer than 30 lines often do too much

**Extract When**: You need comments to explain sections within a function

**One Screen Rule**: Functions should fit on one screen without scrolling

## Impact on Development

**Bad functions slow development:**
- Complex functions with unclear responsibilities require more time to understand
- Large functions are harder to modify without breaking something
- Functions with many parameters are difficult to call correctly
- Hidden side effects make debugging much harder

**Good functions accelerate development:**
- Small, focused functions are easier to understand at a glance
- Single-responsibility functions can be tested independently
- Well-named functions serve as documentation
- Pure functions are reliable and predictable

The effort invested in writing good functions pays dividends in reduced debugging time, easier feature additions, and more maintainable codebases.