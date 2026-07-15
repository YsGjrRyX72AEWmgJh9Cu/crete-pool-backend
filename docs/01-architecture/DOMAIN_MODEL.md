# DOMAIN MODEL

> This document describes the business domain of the Hellenic American Pool History project.

---

# Domain Overview

The system stores historical information about:

- Players
- Tournaments
- Participations
- Matches
- Rankings
- Statistics

The Domain layer contains only business concepts.

---

# Current Domain Model

## Player

Represents a person participating in pool tournaments.

Properties

- PlayerId
- FirstName
- LastName
- Country

Status

✔ Implemented

---

## Tournament

Represents a tournament.

Properties

- TournamentId
- Name
- Country
- Discipline
- Category
- StartDate
- EndDate

Business Rule

- EndDate cannot be before StartDate.

Status

✔ Implemented

---

# Value Objects

## Country

Represents a country.

Examples

- Greece
- Cyprus
- Germany

Status

✔ Implemented

---

## Discipline

Represents the discipline played.

Examples

- 8-Ball
- 9-Ball
- 10-Ball
- Straight Pool

Status

✔ Implemented

---

## Category

Represents the tournament category.

Examples

- Open
- Women
- Junior
- Senior

Status

✔ Implemented

---

# Strongly Typed Identifiers

## PlayerId

Wraps Guid.

Status

✔ Implemented

---

## TournamentId

Wraps Guid.

Status

✔ Implemented

---

# Entity Relationships

Current

```
Player

Tournament
```

Planned

```
Player
    │
    │ participates in
    ▼
Participation
    │
    ▼
Tournament
```

Later

```
Tournament
      │
      ▼
Match
      │
      ▼
Result
```

Later

```
Player
      │
      ▼
Ranking
```

---

# Planned Entities

The following entities are expected to be implemented.

## Participation

Represents a player's participation in a tournament.

Status

⬜ Planned

---

## Match

Represents a played match.

Status

⬜ Planned

---

## Ranking

Represents tournament standings.

Status

⬜ Planned

---

## Statistics

Represents player statistics.

Status

⬜ Planned

---

# Domain Rules

Current rules

✔ Tournament end date must not be before start date.

Future rules

- A player cannot participate twice in the same tournament.
- Rankings are unique per tournament.
- Match winner must be one of the participating players.
- Statistics are derived from historical results.

---

# Implementation Progress

| Domain Object | Status |
|---------------|--------|
| Entity<TId> | ✔ |
| ValueObject | ✔ |
| PlayerId | ✔ |
| TournamentId | ✔ |
| Country | ✔ |
| Discipline | ✔ |
| Category | ✔ |
| Player | ✔ |
| Tournament | ✔ |
| Participation | ⬜ |
| Match | ⬜ |
| Ranking | ⬜ |
| Statistics | ⬜ |