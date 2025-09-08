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

## Bad Examples

### Swallowing Exceptions
```csharp
try {
    ProcessFile(filePath);
} catch {
    // BAD: Silent failure - we don't know what went wrong
}
```
**Problem:** Silent failures hide bugs and make debugging impossible. Always handle or propagate exceptions.

### Catching General Exceptions
```csharp
try {
    var user = GetUser(id);
    var profile = GetProfile(id);
} catch (Exception ex) {
    return "Error occurred"; // BAD: Treats all errors the same
}
```
**Problem:** Different exceptions need different handling. Network timeouts should be handled differently than validation errors.

### Using Exceptions for Control Flow
```csharp
try {
    var user = GetUser(userId);
    return user != null;
} catch (UserNotFoundException) {
    return false; // BAD: Exceptions for normal program flow
}
```
**Problem:** Exceptions are expensive and should be for exceptional cases, not normal business logic.

### Poor Error Messages
```csharp
if (order == null) throw new Exception("Error");
if (order.Items.Count == 0) throw new Exception("Invalid order");
```
**Problem:** Generic messages don't help developers understand what went wrong or how to fix it.

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
**Problem:** Magic numbers are easy to miss and can be ambiguous. What if -1 is a valid price?

### Inconsistent Error Handling
```csharp
// Method 1 throws SystemException
// Method 2 throws ApplicationException  
// Method 3 returns null on error
// Method 4 returns error codes
```
**Problem:** Inconsistent patterns make code unpredictable and hard to maintain.

### Nested Try-Catch Blocks
```csharp
try {
    var parsed = ParseData(data);
    try {
        var validated = ValidateData(parsed);
        try {
            var processed = ProcessData(validated);
            SaveData(processed);
        } catch (ProcessingException ex) {
            // BAD: Nested error handling is confusing
        }
    } catch (ValidationException ex) {
        // BAD: Hard to follow error flow
    }
} catch (ParseException ex) {
    // BAD: Multiple levels of nesting
}
```
**Problem:** Nested error handling is confusing and makes code flow difficult to follow.

## Good Examples

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
**Benefit:** Each exception type is handled specifically with context-rich error messages.

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
**Benefit:** Validate inputs early to fail fast and provide clear error messages.

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
**Benefit:** Use result patterns for expected validation failures instead of exceptions.

### Meaningful Exception Types
```csharp
public class InvalidOrderException : Exception {
    public InvalidOrderException(string message) : base(message) { }
}

public class InsufficientFundsException : Exception {
    public InsufficientFundsException(string message) : base(message) { }
}
```
**Benefit:** Custom exception types make error handling more specific and intentional.

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
**Benefit:** Using statements ensure resources are disposed even when exceptions occur.

### Batch Processing with Individual Error Handling
```csharp
foreach (var payment in payments) {
    try {
        await ProcessPayment(payment);
        results.Add(PaymentResult.Success(payment.Id));
    } catch (PaymentValidationException ex) {
        results.Add(PaymentResult.Failed(payment.Id, ex.Message));
    } catch (InsufficientFundsException ex) {
        results.Add(PaymentResult.Failed(payment.Id, "Insufficient funds"));
    } catch (Exception ex) {
        criticalErrors.Add(ex);
        if (criticalErrors.Count >= 3) break; // Circuit breaker
    }
}
```
**Benefit:** Individual failures don't stop entire batch processing, with circuit breaker for system issues.

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
**Benefit:** Structured results provide clear success/failure information with context.

### Configuration with Fallbacks
```csharp
public ConfigurationResult<T> GetConfigurationValue<T>(string key, T defaultValue = default) {
    try {
        var value = configService.GetValue<T>(key);
        return ConfigurationResult<T>.Success(value);
    } catch (ConfigurationNotFoundException) {
        logger.LogWarning("Config key not found: {Key}. Using default: {Default}", key, defaultValue);
        return ConfigurationResult<T>.Success(defaultValue);
    } catch (ConfigurationParseException ex) {
        return ConfigurationResult<T>.Invalid($"Invalid format for '{key}': {ex.Message}");
    }
}
```
**Benefit:** Graceful degradation with sensible defaults and clear error reporting.

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

## Common Anti-Patterns to Avoid

**Pokemon Exception Handling**: Catching all exceptions with `catch (Exception)`

**Log and Throw**: Logging an exception and then re-throwing it immediately

**Exception Driven Development**: Using exceptions for normal program flow

**Swallowing Exceptions**: Catching exceptions without proper handling

**Generic Error Messages**: "An error occurred" tells users nothing useful

**Inconsistent Error Handling**: Different patterns across similar operations

**Finally Block Misuse**: Business logic or exception-throwing code in finally blocks

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