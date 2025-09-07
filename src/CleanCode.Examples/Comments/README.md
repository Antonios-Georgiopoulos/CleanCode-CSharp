# Comments

## Overview

Good comments explain WHY, not WHAT. The best code is self-documenting and needs minimal comments. When comments are necessary, they should add value by explaining business logic, constraints, or design decisions that aren't obvious from the code itself.

## Key Principles

**Comments should explain intent, not implementation** - Focus on WHY something is done, not HOW it's done.

**Self-documenting code is better than comments** - Clear naming and structure reduce the need for comments.

**Maintain comment accuracy** - Outdated comments are worse than no comments.

**Avoid noise** - Don't comment the obvious or restate what the code clearly shows.

**Use comments for complex business logic** - Algorithms, formulas, and business rules benefit from explanation.

## Bad Examples Analysis

### Obvious Comments
```csharp
// Add a and b together
return a + b;

// Increment the counter by 1
counter++;
```
These comments state the obvious and waste time. The code is already clear.

### Misleading Comments
```csharp
// Apply 10% discount
return price * 0.15m; // Actually applies 15%!
```
Misleading comments are dangerous. When code changes, comments often don't get updated.

### Commented-Out Code
```csharp
// CalculateShipping(); // Old shipping calculation
// SendEmail();          // Email sending removed
// LogOrder();           // Logging disabled for now
```
Version control systems track removed code better than comments. Commented code creates confusion.

### Noise Comments
```csharp
public class Customer
{
    // Customer name
    public string Name { get; set; }
    
    // Customer email address
    public string Email { get; set; }
}
```
Property names and types already convey this information. These comments add no value.

### Comments Explaining Bad Code
```csharp
// This is a complex algorithm that processes data
// First we iterate through all items
// Then we check if each item is valid
// If valid, we process it differently based on type
```
If code needs extensive comments to be understood, refactor the code instead.

### Outdated Comments
```csharp
// Uses the old pricing model from 2020
// Apply senior discount if applicable (removed this feature)
// Save to database (now we save to cloud)
```
Outdated comments mislead developers and indicate poor maintenance practices.

## Good Examples Analysis

### Business Logic Explanation
```csharp
// Using the compound interest formula: A = P(1 + r)^t
// Where P = principal, r = annual rate, t = time in years
return principal * (decimal)Math.Pow((double)(1 + rate), years);
```
Explains the mathematical formula being implemented, which isn't obvious from code alone.

### Warning Comments
```csharp
// WARNING: This method loads all items into memory.
// For datasets > 10,000 items, consider using ProcessLargeDatasetStreaming() instead.
```
Prevents performance issues by warning about constraints and suggesting alternatives.

### Implementation Decisions
```csharp
// Using Guid.NewGuid() instead of timestamp to avoid collisions
// in high-concurrency scenarios where multiple IDs might be generated
// within the same millisecond
```
Explains WHY a particular approach was chosen over alternatives.

### Legal/Compliance Requirements
```csharp
// Required by GDPR: Personal data must be anonymized in logs
var anonymizedUserId = HashUserId(userId);

// Retention policy: Activity logs are automatically purged after 90 days
```
Documents regulatory requirements that drive implementation decisions.

### API Documentation
```csharp
/// <summary>
/// Validates an email address according to RFC 5322 standards.
/// </summary>
/// <param name="email">The email address to validate</param>
/// <returns>True if the email is valid; otherwise, false</returns>
/// <exception cref="ArgumentNullException">Thrown when email is null</exception>
```
XML documentation provides comprehensive API information for public interfaces.

### Performance Explanations
```csharp
// Cache user recommendations for 1 hour to reduce database load
// Recommendation calculation is expensive (ML model inference)
```
Explains optimization decisions and trade-offs made for performance.

## Self-Documenting Code Techniques

**Descriptive Method Names**: `CalculateCompoundInterest()` instead of `Calculate()`

**Intention-Revealing Variables**: `isEligibleForFreeShipping` instead of `flag`

**Small, Focused Methods**: Break complex operations into well-named smaller methods

**Domain-Specific Language**: Use terminology from the business domain in code

**Clear Return Types**: `ValidationResult` instead of `bool` when more context is needed

**Enumeration over Magic Numbers**: `NotificationPriority.High` instead of `1`

## When Comments Add Value

**Complex Algorithms**: Mathematical formulas, encryption, compression algorithms

**Business Rules**: Domain-specific logic that isn't obvious to all developers

**Performance Optimizations**: Caching strategies, memory management decisions

**Integration Points**: External API quirks, third-party library workarounds

**Regulatory Requirements**: GDPR, SOX, PCI-DSS compliance requirements

**Concurrency Concerns**: Thread safety, locking strategies, race condition prevention

**Security Decisions**: Why certain approaches were chosen for security reasons

## Comment Maintenance

**Update comments when code changes** - Treat comments as part of the code that needs maintenance

**Remove obsolete comments** - Delete comments that no longer apply

**Review comments during code reviews** - Check for accuracy and necessity

**Use linting tools** - Automated tools can catch some comment quality issues

## XML Documentation Guidelines

**Use for public APIs** - Document all public methods, properties, and classes

**Include examples** - Show how to use the API correctly

**Document exceptions** - List what exceptions can be thrown and when

**Explain parameters** - Clarify what each parameter represents and valid ranges

**Describe return values** - Explain what the method returns and possible values

## Impact on Code Quality

**Good comments improve maintainability** - Help future developers (including yourself) understand complex logic

**Bad comments hurt productivity** - Misleading or obvious comments waste time and cause confusion

**Self-documenting code reduces comment needs** - Well-structured code with clear names needs fewer comments

**Comments should complement, not replace, good code structure** - Clean code first, helpful comments second

The goal is code that tells a clear story. Comments should only be added when they genuinely help tell that story better.