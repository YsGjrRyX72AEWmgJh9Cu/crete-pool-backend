# ADR-0002: Factory Methods for Aggregate Creation

## Status

Accepted

## Date

2026-07-31

## Context

Domain Aggregates should always be created in a valid state.

Allowing public constructors makes it possible to create partially initialized or invalid aggregates and spreads object creation logic throughout the application.

The domain should be responsible for creating its own entities while enforcing business rules from the beginning.

## Decision

Every Aggregate Root will:

- expose a private constructor;
- provide a public static `Create(...)` factory method;
- generate its own identifier when appropriate;
- validate all required business invariants during creation.

Application code must never instantiate aggregates directly using `new`.

## Consequences

### Positive

- Aggregates are always created in a valid state.
- Creation logic is centralized.
- Business rules are enforced consistently.
- The public API of each aggregate remains small and explicit.

### Negative

- Slightly more code compared to public constructors.
- EF Core requires a constructor that can materialize entities.

## Related Decisions

- ADR-0001: Strongly Typed Identifiers
- ADR-0003: Value Objects (Planned)