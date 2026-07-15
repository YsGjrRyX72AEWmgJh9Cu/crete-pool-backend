# Contributing

First of all, thank you for your interest in contributing to the **Hellenic American Pool History** project.

The goal of this project is to preserve and present the complete history of Hellenic American pool tournaments, players, rankings, and statistics through a clean, maintainable, and scalable software architecture.

Whether you are fixing a bug, improving documentation, or implementing a new feature, your contribution is appreciated.

---

# Development Philosophy

This project follows the following principles:

* Domain Driven Design (DDD)
* Clean Architecture
* Vertical Slice Architecture
* SOLID Principles

The codebase prioritizes:

* correctness
* readability
* maintainability
* testability
* long-term scalability

Avoid unnecessary abstractions and keep implementations simple and explicit.

---

# Project Structure

```
src/

    HellenicAmericanPoolHistory.Domain

    HellenicAmericanPoolHistory.Application

    HellenicAmericanPoolHistory.Infrastructure

    HellenicAmericanPoolHistory.Api

tests/

    HellenicAmericanPoolHistory.Domain.Tests

    HellenicAmericanPoolHistory.Application.Tests

    HellenicAmericanPoolHistory.Infrastructure.Tests

    HellenicAmericanPoolHistory.Api.Tests
```

Dependencies always point inward.

```
API
↓

Application
↓

Domain

↑

Infrastructure
```

The Domain layer must never depend on any other layer.

---

# Development Workflow

1. Create a new branch from the main branch.
2. Implement a single feature or fix.
3. Add or update unit tests.
4. Run the build.
5. Run all tests.
6. Commit your changes using a meaningful commit message.
7. Open a Pull Request.

Keep every commit focused on a single responsibility.

---

# Coding Standards

## General

* One public class per file.
* File name must match the class name.
* Use file-scoped namespaces.
* Add XML documentation to every public type and member.
* Prefer immutable objects whenever possible.
* Validate constructor arguments.
* Use `ArgumentNullException.ThrowIfNull()`.
* Use `ArgumentException.ThrowIfNullOrWhiteSpace()` where appropriate.

## Domain

Business rules belong inside Domain entities.

Value Objects should:

* be immutable
* compare by value
* validate their own state

Entities should:

* inherit from `Entity<TId>`
* own business behavior
* expose immutable identities

Strongly typed identifiers should inherit from `ValueObject`.

---

# Testing

Every new Domain object should include unit tests.

Typical workflow:

```
Create class

↓

Create tests

↓

dotnet build

↓

dotnet test

↓

Commit
```

The repository should always remain in a buildable state.

Before creating a Pull Request, ensure that:

* `dotnet build` succeeds
* `dotnet test` succeeds
* all new functionality is covered by tests

---

# Commit Messages

Use small, focused commits.

Examples:

```
feat(domain): implement Participation entity

feat(domain): implement Match entity

test(domain): add Participation tests

fix(domain): validate tournament dates

docs: update domain model
```

---

# Pull Request Checklist

Before submitting a Pull Request, verify that:

* [ ] The solution builds successfully.
* [ ] All tests pass.
* [ ] New code follows the existing architecture.
* [ ] XML documentation has been added where required.
* [ ] No unnecessary files are included.
* [ ] The change is focused on a single feature or fix.
* [ ] Commit messages follow the project's conventions.

---

# Documentation

Please keep the project documentation up to date whenever necessary.

The following documents describe the project:

* `README.md`
* `PROJECT_CONTEXT.md`
* `DOMAIN_MODEL.md`
* `DEVELOPMENT_LOG.md`
* `CONTRIBUTING.md`

Documentation is considered part of the project and should evolve together with the source code.
