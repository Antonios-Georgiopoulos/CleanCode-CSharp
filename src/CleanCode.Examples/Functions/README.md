# Functions

## Overview

Well-designed functions are the building blocks of clean code. They should be small, focused, and do one thing well. Poor function design leads to code that's hard to understand, test, and maintain.

## Key Principles

**Single Responsibility** - Each function should have one reason to change and do one thing well.

**Small Size** - Functions should be short enough to understand at a glance (typically 5-20 lines).

**Few Parameters** - Ideally 0-3 parameters. More than 3 suggests the function is doing too much.

**No Side Effects** - Functions should not have hidden behaviors that aren't obvious from their name.

**Consistent Abstraction Level** - All statements in a function should be at the same level of abstraction.

## Bad Examples Analysis

### Doing Too Many Things
```csharp
ProcessUserAndGenerateReport() // Validates, processes, saves, emails, generates report, logs
```
This method violates Single Responsibility Principle by handling validation, business logic, database operations, email sending, and reporting.

### Too Many Parameters
```csharp
CreateOrder(customerName, customerEmail, customerPhone, customerAddress, 
           productName, productPrice, quantity, discountPercent, ...)
```
Methods with many parameters are hard to call, understand, and maintain. They often indicate missing abstractions.

### Deep Nesting
```csharp
if (data != null) {
    if (data is string str) {
        if (!string.IsNullOrEmpty(str)) {
            if (str.Length > 10) {
                // More nesting...
```
Deep nesting makes code hard to follow and understand the flow.

### Hidden Side Effects
```csharp
int CalculateTotal(List<int> numbers) {
    GlobalCounter++;           // Hidden: modifies global state
    Console.WriteLine(...);    // Hidden: logs to console
    TrackUsage(...);          // Hidden: sends analytics
    return numbers.Sum();
}
```
Method name suggests pure calculation but performs hidden operations.

### Boolean Parameters
```csharp
ProcessFile(filename, true, false, true) // What do these booleans mean?
```
Boolean flags make method calls unclear and often indicate the method does multiple things.

## Good Examples Analysis

### Single Responsibility
```csharp
ValidateUserRequest()     // Only validates
CreateUserFromRequest()   // Only creates user object
CalculateUserBonus()      // Only calculates bonus
SaveUser()               // Only saves to database
```
Each method has one clear purpose and can be tested independently.

### Well-Defined Parameters
```csharp
ProcessUser(CreateUserRequest request) // Single, meaningful parameter object
```
Using parameter objects instead of primitive obsession makes code more readable and maintainable.

### Early Returns
```csharp
if (input == null) return "Input cannot be null";
if (string.IsNullOrEmpty(input)) return "Email cannot be empty";
// Continue with main logic...
```
Early returns eliminate deep nesting and make the happy path clear.

### Pure Functions
```csharp
int CalculateSum(IEnumerable<int> numbers) {
    return numbers?.Sum() ?? 0; // No side effects, predictable output
}
```
Pure functions are easier to test, debug, and reason about.

### Explicit Side Effects
```csharp
GetUserNameById()                    // Pure function
GetUserNameByIdWithTracking()        // Explicitly indicates side effects
```
Separating pure functions from those with side effects improves clarity.

## Function Design Guidelines

**Naming**: Use verbs for functions (Calculate, Process, Validate) and be specific about what they do.

**Return Types**: Be consistent. Don't return different types based on input conditions.

**Error Handling**: Use exceptions for exceptional cases, not for control flow.

**Testing**: If a function is hard to test, it's probably doing too much.

**Abstraction**: Group related parameters into objects rather than passing many primitives.

## Common Refactoring Patterns

**Extract Method**: Break large functions into smaller, focused ones.

**Parameter Object**: Group related parameters into a single object.

**Replace Parameter with Method Call**: If a parameter can be derived, calculate it inside the method.

**Separate Query from Modifier**: Don't mix functions that return values with those that modify state.

## Impact

**Bad functions slow development:** Complex functions with unclear responsibilities require more time to understand, modify, and debug.

**Good functions accelerate development:** Small, focused functions are easier to understand, test, modify, and reuse across the codebase.