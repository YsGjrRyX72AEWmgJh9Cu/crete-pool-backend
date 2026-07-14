# Hellenic American Pool History Platform

Backend implementation of the Hellenic American Pool History Platform.

> Status: Sprint 1 – Domain Foundation Completed

---

## Project Goals

The project aims to provide a reliable historical archive for:

- Players
- Tournaments
- Participations
- Matches
- Rankings
- Statistics

The focus is on clean architecture, maintainability and long-term scalability.

---

# Technology Stack

- .NET 10
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- xUnit
- Swagger / OpenAPI

---

# Architecture

The solution follows:

- Domain-Driven Design (DDD)
- Clean Architecture
- Vertical Slice Architecture
- SOLID Principles

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

---

# Solution Structure

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

---

# Build

```bash
dotnet build
```

---

# Run Tests

```bash
dotnet test
```

---

# Current Status

## Implemented

### Base Classes

- Entity<TId>
- ValueObject

### Value Objects

- Country
- Discipline
- Category

### Identifiers

- PlayerId
- TournamentId

### Entities

- Player
- Tournament

---

# Planned Features

- Participation
- Match
- Ranking
- Statistics
- REST API
- PostgreSQL persistence
- Search
- Historical reports

---

# Development

Development follows:

- Test-Driven Development (TDD)
- Small commits
- Incremental implementation
- Continuous testing

Every feature is implemented in the following order:

1. Domain
2. Tests
3. Build
4. Tests
5. Commit

---

# License

This project is currently under active development.