# Code Formatting

## Overview

Consistent code formatting is essential for maintainable, professional code. Good formatting makes code easier to read, understand, and modify. It reduces cognitive load and helps teams collaborate more effectively. Poor formatting creates visual noise and makes code harder to navigate and maintain.

## Key Principles

**Consistency Above All** - The specific style matters less than applying it consistently throughout the codebase.

**Readability First** - Format code to be as readable as possible for humans, not just functional for computers.

**Follow Language Conventions** - Adhere to established conventions for the programming language and framework.

**Use Tools** - Leverage formatters and linters to maintain consistency automatically.

**Team Standards** - Establish and document formatting standards for the entire team.

## Bad Examples

### Inconsistent Indentation and Spacing
```csharp
public class BadExample {
public string Name{get;set;}
    public int Age { get ;set; }
  public bool IsActive{get;set;}
}
```
**Problem:** Mixed indentation makes code structure unclear and unprofessional.

### Poor Spacing Around Operators
```csharp
if(data==null){
throw new ArgumentNullException(nameof(data));
}
for(int i=0;i<data.Count;i++){
var item=data[i];
```
**Problem:** Lack of spacing around operators and keywords reduces readability significantly.

### Excessively Long Lines
```csharp
public void ProcessUserOrderWithComplexBusinessLogicAndMultipleValidationsAndCalculations(string userName, string userEmail, List<OrderItem> orderItems, PaymentMethod paymentMethod, ShippingAddress shippingAddress, BillingAddress billingAddress, DiscountCoupon discountCoupon, TaxCalculationRules taxRules) {
```
**Problem:** Long lines require horizontal scrolling and are difficult to review and maintain.

### Inconsistent Naming Conventions
```csharp
public class mixed_naming_Conventions {
    public string firstName;
    public string LastName;
    public string email_address;
    public int AGE;
    public void Process_Data() { }
    public void processUser() { }
    public void CALCULATE_TOTAL() { }
}
```
**Problem:** Mixed naming conventions create confusion and violate language standards.

### Poor Member Organization
```csharp
public class PoorOrganization {
    public string Name { get; set; }
    public void ProcessOrder() { }
    public int Age { get; set; }
    public void CalculateTotal() { }
    private string secretKey;
    public string Email { get; set; }
}
```
**Problem:** Random member ordering makes code difficult to navigate and understand.

### Inconsistent String Formatting
```csharp
var message1 = "Hello " + name + ", you are " + age.ToString() + " years old";
var message2 = $"Hello {name}";
var message3 = string.Format("Your email is {0}", email);
var message4 = "Welcome, " + name + "! Your email: " + email;
```
**Problem:** Mixed concatenation styles create inconsistency and reduce maintainability.

## Good Examples

### Consistent Indentation and Spacing
```csharp
public class GoodExample
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool IsActive { get; set; }

    public void ProcessData(List<string> data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        for (int i = 0; i < data.Count; i++)
        {
            var item = data[i];
            if (item != null && item.Length > 0)
            {
                Console.WriteLine($"Processing: {item}");
            }
        }
    }
}
```
**Benefit:** Consistent indentation and spacing make code structure clear and professional.

### Proper Line Breaking
```csharp
public void ProcessUserOrder(
    string userName,
    string userEmail,
    List<OrderItem> orderItems,
    PaymentMethod paymentMethod,
    ShippingAddress shippingAddress,
    BillingAddress billingAddress,
    DiscountCoupon discountCoupon,
    TaxCalculationRules taxRules)
{
    // Method implementation
}
```
**Benefit:** Breaking long parameter lists improves readability and makes parameters easier to modify.

### Consistent Naming Conventions
```csharp
public class ConsistentNaming
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    
    public void ProcessData() { }
    public void ProcessUser() { }
    public void CalculateTotal() { }
}
```
**Benefit:** Following C# conventions (PascalCase for public members) creates predictable, professional code.

### Logical Member Organization
```csharp
public class WellOrganized
{
    // Properties grouped together
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;

    // Private fields grouped together
    private readonly ILogger _logger;

    // Constructor
    public WellOrganized(ILogger logger)
    {
        _logger = logger;
    }

    // Public methods grouped together
    public void ProcessOrder() { }
    public void CalculateTotal() { }

    // Private methods grouped together
    private void InternalMethod() { }
}
```
**Benefit:** Logical grouping makes code easier to navigate and understand.

### Clean LINQ Formatting
```csharp
var result = users
    .Where(u => u.IsActive)
    .Where(u => u.Age > 21)
    .Select(u => new { u.Name, u.Email })
    .OrderBy(u => u.Name)
    .ToList();
```
**Benefit:** Each LINQ operation on its own line shows data flow clearly.

### Modern Switch Expressions
```csharp
public string GetDescription(int value) => value switch
{
    1 => "One",
    2 => "Two",
    3 => "Three",
    _ => "Unknown"
};
```
**Benefit:** Modern switch expressions are more concise and expressive than traditional switch statements.

### Consistent String Interpolation
```csharp
var message1 = $"Hello {name}, you are {age} years old";
var message2 = $"Hello {name}";
var message3 = $"Your email is {email}";
var message4 = $"Welcome, {name}! Your email: {email}";
```
**Benefit:** Uniform approach improves readability and performance.

## Formatting Guidelines

### Indentation
- Use 4 spaces (or consistent tabs) for each indentation level
- Align related code at the same indentation level
- Indent content within braces consistently

### Spacing
- Space around operators: `x + y`, not `x+y`
- Space after keywords: `if (condition)`, not `if(condition)`
- Space after commas: `Method(a, b, c)`, not `Method(a,b,c)`
- No space inside parentheses: `Method(param)`, not `Method( param )`

### Line Length
- Keep lines under 100-120 characters when possible
- Break long parameter lists across multiple lines
- Break long LINQ chains into multiple lines
- Break long conditional expressions logically

### Brace Placement
- Opening braces on new line (Allman style) for C#
- Consistent brace placement throughout codebase
- Always use braces for single-statement blocks

### Member Organization
1. Constants and static fields
2. Instance fields (private first)
3. Constructors
4. Properties (public first)
5. Public methods
6. Protected methods
7. Private methods
8. Nested types

### Naming Conventions
- **PascalCase**: Classes, methods, properties, namespaces
- **camelCase**: Local variables, parameters
- **_camelCase**: Private instance fields (with underscore prefix)
- **UPPER_CASE**: Constants
- **IPascalCase**: Interfaces (with 'I' prefix)

## Modern C# Features

### String Interpolation
```csharp
var message = $"Hello {name}, you are {age} years old";
```

### Object Initializers
```csharp
var person = new Person
{
    Name = "John",
    Age = 30,
    Email = "john@example.com"
};
```

### Pattern Matching
```csharp
var result = obj switch
{
    string s when s.Length > 10 => "Long string",
    string s => "Short string",
    int i when i > 0 => "Positive number",
    _ => "Something else"
};
```

### Using Declarations
```csharp
public async Task<string> ReadFileAsync(string path)
{
    using var reader = new StreamReader(path);
    return await reader.ReadToEndAsync();
}
```

## Tool Integration

### EditorConfig
Use `.editorconfig` files to enforce consistent formatting across IDEs:
```ini
[*.cs]
indent_style = space
indent_size = 4
trim_trailing_whitespace = true
insert_final_newline = true
```

### Code Formatters
- **Visual Studio**: Built-in formatter with customizable rules
- **Rider**: Comprehensive formatting and style inspection
- **dotnet format**: Command-line formatter for CI/CD integration

### Style Analyzers
- **StyleCop**: Enforces style and consistency rules
- **EditorConfig**: Cross-platform style configuration
- **Roslyn Analyzers**: Custom rules and style enforcement

## Team Practices

### Establish Standards
- Document formatting conventions
- Use automated tools to enforce standards
- Include formatting checks in code review process
- Set up CI/CD to validate formatting

### Code Review Focus
- Consistent indentation and spacing
- Proper naming conventions
- Logical organization of code
- Adherence to line length limits
- Use of modern language features

## Impact

**Good formatting improves:**
- Code readability and comprehension
- Team productivity and collaboration
- Code review efficiency
- Professional appearance
- Onboarding of new team members

**Poor formatting causes:**
- Increased cognitive load
- Slower code reviews
- Difficulty in debugging
- Reduced team productivity
- Unprofessional appearance

Consistent formatting is an investment in code quality that pays dividends in reduced maintenance time, improved team collaboration, and professional software development practices.