# PROJECT CONTEXT

> This document describes the architecture, coding standards, development workflow and current state of the Hellenic American Pool History project.
>
> Every new ChatGPT conversation should start by reading this document.

---

# Project

**Name**

Hellenic American Pool History

## Goal

Build a modern application that preserves and presents the complete history of Hellenic American pool tournaments, players, rankings and statistics.

The project prioritizes:

- correctness
- maintainability
- clean architecture
- long-term scalability

---

# Technology Stack

- .NET 10
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- xUnit
- Swagger / OpenAPI
- Git
- GitHub

---

# Architecture

The solution follows:

- Domain Driven Design (DDD)
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

The Domain layer never depends on any other layer.

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

# Coding Standards

## General

- One public class per file.
- File name equals class name.
- File-scoped namespaces.
- XML documentation for every public type and member.
- Immutable objects whenever possible.
- Constructor validation.
- Use ArgumentException.ThrowIfNullOrWhiteSpace().
- Use ArgumentNullException.ThrowIfNull() where appropriate.

---

# Value Objects

Every Value Object inherits from:

```
ValueObject
```

Characteristics:

- immutable
- equality by value
- constructor validation

---

# Entities

Every Entity inherits from:

```
Entity<TId>
```

Characteristics:

- identity based
- immutable identity
- business behavior belongs here

---

# Strongly Typed Identifiers

Identifiers inherit from ValueObject.

Examples:

- PlayerId
- TournamentId

Each identifier contains:

- Guid Value
- static New()

---

# Testing

The project uses:

- xUnit

Every Domain object must have unit tests.

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

Build must always succeed.

Tests must always succeed.

---

# Git Workflow

Small commits.

One feature per commit.

Examples:

feat(domain): implement Country value object

feat(domain): implement Tournament entity

fix(domain): validate dates

test(domain): add Tournament tests

---

# Current Progress

## Completed Base Classes

- Entity<TId>
- ValueObject

---

## Completed Value Objects

- Country
- Discipline
- Category

---

## Completed Identifiers

- PlayerId
- TournamentId

---

## Completed Entities

- Player
- Tournament

---

## Current Status

✔ Build succeeds

✔ Tests succeed

✔ Domain foundation completed

---

# Next Planned Work

The next Domain entities are expected to be implemented in the following order:

1. Participation
2. Match
3. Ranking
4. Statistics

---

# Development Philosophy

The assistant should:

- prefer incremental development
- avoid generating huge files
- keep commits small
- always preserve Clean Architecture
- prefer readability over clever code
- avoid unnecessary abstractions
- follow SOLID
- respect DDD boundaries
- never skip build and tests

---

# Conversation Rule

Whenever a new ChatGPT conversation starts:

1. Read this document first.
2. Continue from the current project status.
3. Preserve the existing architecture.
4. Never rewrite completed code unless explicitly requested.