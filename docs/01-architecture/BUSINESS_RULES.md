# Business Rules

> This document defines the business rules governing the Hellenic American Pool History domain.

Business Rules describe how the system behaves and ensure historical integrity, consistency and correctness across the entire platform.

This document complements the Domain Model and should be considered the authoritative reference for domain behavior.

---

# General Principles

The following principles apply throughout the entire domain.

## Historical Integrity

Historical records represent real events.

Historical data must never be silently altered or lost.

Whenever corrections are required, the integrity of the historical record must be preserved.

---

## Data Consistency

Business rules always take precedence over technical implementation.

No operation should leave the system in an inconsistent state.

---

## Domain First

Every new feature must belong to an existing Aggregate or introduce a new Aggregate through an architectural review.

---

## Calculated Data

Whenever possible, data should be calculated instead of stored.

Examples include:

- Statistics
- Rankings
- Win Percentage
- Head-to-Head records

---

# Player Rules

## Player Registration

A Player represents a real individual.

Duplicate player records should be avoided whenever possible.

---

## Player History

A Player may participate in many tournaments.

Historical participation is derived from Participations.

---

## Player Deletion

A Player that has historical Participations should not be physically deleted.

Historical integrity always takes precedence.

---

# Organization Rules

An Organization may own multiple Tournament Series.

Organizations preserve historical ownership of tournaments.

---

# Tournament Series Rules

A Tournament Series groups recurring tournaments.

Each Tournament belongs to at most one Tournament Series.

---

# Tournament Rules

## Venue

Every Tournament is held at exactly one Venue.

---

## Participations

A Tournament may contain zero or more Participations.

---

## Tournament Lifecycle

A Tournament cannot start without Participants.

A Tournament cannot be completed while unfinished Matches exist.

---

## Tournament Deletion

A Tournament containing Participations should not be deleted.

---

# Participation Rules

Participation represents a Player registered in a Tournament.

---

## Uniqueness

A Player may participate only once in the same Tournament.

---

## Registration

Registration Date is mandatory.

---

## Seed

Seed is optional.

If assigned, it must be unique within the Tournament.

---

## Status

Every Participation must always have a valid Participation Status.

---

## Future Result

A Participation may eventually contain:

- Final Position
- Prize Money
- Awards

---

# Match Rules

A Match always references exactly two Participations.

Matches never reference Players directly.

---

## Winner

The Winner must be one of the two Participations.

---

## Completion

Completed Matches should not be modified.

---

## Tournament Ownership

Every Match belongs to exactly one Tournament through Participations.

---

# Statistics Rules

Statistics are calculated.

Statistics are never manually edited.

Statistics are derived from completed Matches.

Examples include:

- Matches Played
- Wins
- Losses
- Win Percentage
- Titles
- Finals

---

# Ranking Rules

Rankings are calculated.

Rankings are never stored manually.

Rankings are derived from:

- Tournament Results
- Statistics

---

# Venue Rules

A Venue may host many Tournaments.

Historical Venue information should remain preserved.

---

# Future Rules

The following rules will be introduced in future versions.

## Brackets

- Single Elimination
- Double Elimination
- Round Robin

---

## Awards

- MVP
- Best Break
- High Run

---

## Hall of Fame

Hall of Fame members are derived from historical achievements.

---

# Engineering Principles

The following engineering principles guide the implementation of the system.

1. Domain First

2. Historical Integrity

3. Clean Architecture

4. Documentation Before Features

5. Simplicity Over Cleverness

6. Strongly Typed Identifiers

7. Business Rules Before Code

8. Tests Protect the Domain

---

End of Document