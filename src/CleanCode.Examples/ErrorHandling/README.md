# Error Handling

## Overview

Proper error handling is crucial for building robust, maintainable applications. Good error handling makes debugging easier, provides better user experience, and prevents catastrophic failures. Poor error handling leads to silent bugs, unclear error messages, and systems that are difficult to troubleshoot.

## Key Principles

**Fail Fast** - Detect and report errors as early as possible to prevent cascading failures.

**Be Specific** - Use specific exception types and provide detailed error messages that help diagnose problems.

**Handle Expected Errors Gracefully** - Anticipate common error conditions and handle them appropriately.

**Don't Swallow Exceptions** - Always handle exceptions appropriately; silent failures are dangerous.

**Use the Right Tool** - Exceptions for exceptional cases, result patterns for expected failures.

**Provide Context** - Include relevant information in error messages to aid debugging.

## Bad Examples Analysis

### Swallowing Exceptions
```csharp
try {
    ProcessFile(filePath);
} catch {
    // BAD: Silent failure - we don't know what went wrong
}
```
Silent failures hide bugs and make debugging impossible. Always handle or propagate exceptions.

### Catching General Exceptions
```csharp
try {
    var user = GetUser(id);
    var profile = GetProfile(id);
} catch (Exception ex) {
    return "Error occurred"; // BAD: Treats all errors the same
}
```
Different exceptions need different handling. Network timeouts should be handled differently than validation errors.

### Using Exceptions for Control Flow
```csharp
try {
    var user = GetUser(userId);
    return user != null;
} catch (UserNotFoundException) {
    return false; // BAD: Exceptions for normal program flow
}
```
Exceptions are expensive and should be for exceptional cases, not normal business logic.

### Poor Error Messages
```csharp
if (order == null) throw new Exception("Error");
if (order.Items.Count == 0) throw new Exception("Invalid order");
```
Generic messages don't help developers understand what went wrong or how to fix it.

### Magic Return Values
```csharp
public decimal CalculatePrice(...) {
    try {
        // calculation logic
    } catch {
        return -1; // BAD: Magic number as error indicator
    }
}
```
Magic numbers are easy to miss and can be ambiguous. What if -1 is a valid price?

### Inconsistent Error Handling
```csharp
// Method 1 throws SystemException
// Method 2 throws ApplicationException  
// Method 3 returns null on error
// Method 4 returns error codes
```
Inconsistent patterns make code unpredictable and hard to maintain.

## Good Examples Analysis

### Specific Exception Handling
```csharp
try {
    return await File.ReadAllTextAsync(filePath);
} catch (FileNotFoundException ex) {
    throw new FileProcessingException($"File '{filePath}' not found", ex);
} catch (UnauthorizedAccessException ex) {
    throw new FileProcessingException($"Access denied to '{filePath}'", ex);
} catch (IOException ex) {
    throw new FileProcessingException($"Error reading '{filePath}': {ex.Message}", ex);
}
```
Each exception type is handled specifically with context-rich error messages.

### Input Validation
```csharp
public void ProcessOrder(Order order) {
    if (order == null)
        throw new ArgumentNullException(nameof(order));
    
    if (order.Items?.Count == 0)
        throw new InvalidOrderException("Order must contain at least one item");
    
    if (order.Total < 0)
        throw new InvalidOrderException($"Order total cannot be negative: {order.Total:C}");
}
```
Validate inputs early to fail fast and provide clear error messages.

### Result Pattern for Business Logic
```csharp
public ValidationResult ValidateUser(User user) {
    var errors = new List<string>();
    
    if (string.IsNullOrWhiteSpace(user.Name))
        errors.Add("Name is required");
    
    return errors.Any() 
        ? ValidationResult.Failed(errors) 
        : ValidationResult.Success();
}
```
Use result patterns for expected validation failures instead of exceptions.

### Meaningful Exception Types
```csharp
public class InvalidOrderException : Exception {
    public InvalidOrderException(string message) : base(message) { }
}

public class InsufficientFundsException : Exception {
    public InsufficientFundsException(string message) : base(message) { }
}
```
Custom exception types make error handling more specific and intentional.

### Proper Resource Management
```csharp
try {
    using var fileStream = new FileStream(path, FileMode.Open);
    using var reader = new StreamReader(fileStream);
    return await reader.ReadToEndAsync();
} catch (Exception ex) {
    logger.LogError(ex, "Error reading file: {FilePath}", path);
    throw new FileProcessingException($"Failed to read file: {path}", ex);
}
```
Using statements ensure resources are disposed even when exceptions occur.

### Structured Error Results
```csharp
public class OperationResult {
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }
    public OperationResultType Type { get; }
    
    public static OperationResult Success() => new(true, "", Success);
    public static OperationResult Failed(string message) => new(false, message, Failed);
}
```
Structured results provide clear success/failure information with context.

## Error Handling Strategies

**Exception Handling**: Use for unexpected errors that shouldn't occur during normal operation.

**Result Patterns**: Use for expected failures like validation errors or business rule violations.

**Null Object Pattern**: Use to avoid null reference exceptions in some scenarios.

**Circuit Breaker**: Use for external service calls to prevent cascading failures.

**Retry Logic**: Use for transient errors that might resolve themselves.

**Logging**: Always log errors with sufficient context for debugging.

## When to Use Each Approach

**Exceptions for:**
- Null arguments to public methods
- Invalid method parameters
- System-level errors (file not found, network failure)
- Programming errors (index out of bounds)

**Result Patterns for:**
- Business rule validation
- User input validation
- Expected operational failures
- Batch processing where some items might fail

**Defensive Programming:**
- Validate all inputs
- Check preconditions
- Handle edge cases explicitly
- Use assertions for debugging

## Logging Best Practices

**Include Context**: Log relevant parameters, user ID, operation details.

**Use Appropriate Log Levels**: Error for exceptions, Warning for recoverable issues.

**Structured Logging**: Use structured data for better searchability.

**Don't Log Sensitive Data**: Avoid passwords, credit card numbers, personal data.

**Log at the Right Level**: Don't log the same error multiple times as it propagates.

## Testing Error Conditions

**Test Happy Path**: Ensure normal operation works correctly.

**Test Error Conditions**: Verify proper error handling for various failure modes.

**Test Edge Cases**: Boundary conditions, null inputs, empty collections.

**Test Resource Cleanup**: Ensure resources are properly disposed on errors.

**Test Error Messages**: Verify error messages are helpful and accurate.

## Common Anti-Patterns to Avoid

**Pokemon Exception Handling**: Catching all exceptions with `catch (Exception)`

**Log and Throw**: Logging an exception and then re-throwing it immediately

**Exception Driven Development**: Using exceptions for normal program flow

**Swallowing Exceptions**: Catching exceptions without proper handling

**Generic Error Messages**: "An error occurred" tells users nothing useful

**Inconsistent Error Handling**: Different patterns across similar operations

## Impact on System Quality

**Good error handling improves:**
- System reliability and stability
- Developer productivity during debugging
- User experience with clear error messages
- System monitoring and alerting capabilities

**Poor error handling causes:**
- Silent failures that hide bugs
- Difficult debugging and troubleshooting
- Poor user experience with unclear errors
- System instability and cascading failures

Invest time in proper error handling early in development. The upfront cost pays dividends in reduced debugging time and improved system reliability.