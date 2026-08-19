# ADR-0001: Strongly Typed Identifiers

## Status

Accepted

## Date

2026-07-31

## Context

The domain model contains multiple entity identifiers such as PlayerId,
TournamentId, VenueId, OrganizationId and others.

Using primitive Guid values throughout the application increases the risk
of accidentally passing the wrong identifier to constructors, methods and
queries.

The domain should express identity explicitly and provide compile-time
type safety.

## Decision

Every domain entity will use its own Strongly Typed Identifier implemented
as a `readonly record struct`.

Examples include:

- PlayerId
- TournamentId
- VenueId
- OrganizationId

Each identifier exposes a static `New()` factory method responsible for
creating new identifiers.

## Consequences

### Positive

- Compile-time type safety.
- Improved readability.
- Better domain expressiveness.
- Prevents accidental identifier mix-ups.

### Negative

- Requires EF Core value converters.
- Slightly more implementation code than using primitive Guid values.

## Related Decisions

None.