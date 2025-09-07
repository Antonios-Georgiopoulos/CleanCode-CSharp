# Naming Conventions

## Overview

Clear, descriptive names reduce cognitive load and make code self-documenting. Poor naming forces developers to constantly refer to documentation or guess intent.

## Key Principles

**Use intention-revealing names** - Names should answer why it exists, what it does, and how it's used.

**Avoid disinformation** - Don't use names that obscure meaning or suggest wrong types.

**Make meaningful distinctions** - Avoid noise words like `Data`, `Info`, `Manager`.

**Use pronounceable names** - Code is read more often than written.

**Use searchable names** - Single-letter variables and magic numbers are unsearchable.

## Bad Examples Analysis

### Cryptic Abbreviations
```csharp
string usr, pwd, db;  // What kind of user? Database what?
```

### Hungarian Notation
```csharp
int iCount;     // Type is obvious from declaration
string strName; // Redundant prefix
bool bIsActive; // Outdated practice
```

### Meaningless Names
```csharp
void DoStuff()           // Does what exactly?
object GetData()         // What data?
void Process(object data, int flag) // Process how? Flag for what?
```

### Magic Numbers
```csharp
if (x > 5) { ProcessData(temp, 42); } // Why 5? Why 42?
```

## Good Examples Analysis

### Self-Documenting Names
```csharp
string Username;                    // Clear purpose
string DatabaseConnectionString;    // Specific and descriptive
bool IsAccountActive;              // Boolean intent clear
```

### Meaningful Methods
```csharp
void CalculateAndDisplayResults()               // Clear action
CustomerDataModel GetCustomerDataFromDatabase() // Returns what, from where
void ProcessCustomerData(object data, int timeoutInSeconds) // Clear parameters
```

### Named Constants
```csharp
const int MinimumValidValue = 5;
const int DefaultProcessingTimeout = 42;
```

## Quick Rules

- **Classes**: Nouns (Customer, Account, Parser)
- **Methods**: Verbs (Calculate, Process, Validate)
- **Booleans**: Questions (IsActive, HasPermission, CanAccess)
- **Constants**: Descriptive (MaxRetryCount, DefaultTimeout)
- **Variables**: Intention-revealing (customerEmail, orderTotal)

## Common Mistakes

- Single letter variables (except loop counters)
- Abbreviations (usr, pwd, calc)
- Noise words (DataInfo, ProcessManager)
- Inconsistent casing (userName, user_email, UserAddress)
- Type prefixes (strName, iCount)

## Impact

**Bad naming costs time:** Developers spend extra mental energy deciphering intent, leading to bugs and slower development.

**Good naming saves time:** Code becomes self-documenting, reducing need for comments and documentation lookups.