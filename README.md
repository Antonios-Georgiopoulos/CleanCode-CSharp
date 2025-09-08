# Clean Code Examples in C#

[![CI](https://github.com/Antonios-Georgiopoulos/CleanCode-CSharp/actions/workflows/ci.yml/badge.svg)](https://github.com/Antonios-Georgiopoulos/CleanCode-CSharp/actions/workflows/ci.yml)

A collection of tiny, focused Clean Code examples in C# showing **Bad vs Good** side by side — _no unit tests, zero scaffolding_.

## 🎯 Purpose

Keep examples **minimal** and **self‑explanatory**, so readers can grasp each idea at a glance without digging through frameworks or test code.

## 📚 Categories

- [x] **Naming Conventions** — Meaningful names for variables, methods, and classes
- [x] **Functions/Methods** — Single responsibility, parameter management
- [x] **Classes** — Cohesion, encapsulation, proper design
- [x] **SOLID Principles** — Five fundamental principles of OOP
- [x] **Comments** — When and how to comment effectively
- [x] **Error Handling** — Exceptions, validation, defensive programming
- [x] **Code Formatting** — Consistent style and structure

## 🚀 Getting Started

```bash
git clone https://github.com/Antonios-Georgiopoulos/CleanCode-CSharp.git
cd CleanCode-CSharp
dotnet restore
dotnet build
```

> There are **no tests** in this repo by design. The CI only restores and builds to ensure examples compile.

## 📖 How to Use

Each category folder contains:

- `Bad*.cs` — anti‑pattern(s), intentionally flawed
- `Good*.cs` — clean alternative(s)
- `README.md` — a short guide, _no references to testing_

## ✅ Authoring Guidelines (PRs welcome)

- Keep each example **< 60 LOC** and focused on _one idea_.
- Prefer **plain C#** — no third‑party packages or frameworks.
- Avoid infrastructure (DI containers, logging frameworks, etc.).
- Use **clear naming, consistent casing**, and **named constants** over magic numbers.
- No test files or helpers — this repo is about _reading_ code, not _testing_ it.

## 🤝 Contributing

Issues and PRs are welcome. Please follow the **Authoring Guidelines** above.

## 📄 License

MIT License
