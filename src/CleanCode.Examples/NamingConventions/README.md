## Naming Conventions

Clear, descriptive names reduce cognitive load and make code self‑documenting. Poor naming forces readers to guess intent.

### Core Rules
- **Intention‑revealing**: Names answer *what/why/how*.
- **No disinformation**: Avoid misleading or type‑hint names.
- **Meaningful distinctions**: Skip noise words like `Data`, `Info`, `Manager`.
- **Pronounceable & searchable**: Avoid `usr`, `pwd`, one‑letter vars (beyond simple loops).
- **Consistent casing**: Stick to PascalCase for types/members, camelCase for locals.

### Bad vs Good
**Bad**
```csharp
public class Calc
{
    public double a, b;                // Single‑letter variables
    public string usr, pwd;            // Cryptic abbreviations
    public int iCount;                 // Hungarian notation
    public void DoStuff() { /* ... */ } // Meaningless method name
}
```

**Good**
```csharp
public class OrderCalculator
{
    public double FirstNumber { get; set; }
    public double SecondNumber { get; set; }
    public bool IsActive { get; set; }

    private const int MinimumValidValue = 5; // Named constant
    private void ProcessCustomer(Customer customer, int timeoutSeconds) { }
}
```

### Checklist (before commit aa single line of code)
- [ ] Would a new teammate understand the purpose from the name alone?
- [ ] Are booleans phrased as questions (e.g., `IsActive`, `HasItems`)?
- [ ] Any magic numbers replaced with named constants?
- [ ] Any abbreviations, type prefixes, or mixed casing to fix?
- [ ] Are methods verbs and classes nouns?
