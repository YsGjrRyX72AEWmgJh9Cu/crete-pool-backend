# Domain Model

> This document describes the business domain of the Hellenic American Pool History project.

---

# Purpose

The Hellenic American Pool History domain is responsible for preserving the historical record of pocket billiards competitions.

The primary goal of the system is to accurately represent players, tournaments, venues, organizations, participations and match results while preserving historical integrity.

The domain is designed following Domain-Driven Design (DDD) principles using:

- Aggregates
- Entities
- Value Objects
- Strongly Typed Identifiers
- Domain Events (future)

This document is the authoritative description of the business domain.

---

# Core Aggregates

The domain is organized around the following aggregates.

---

# Aggregate Relationships

The aggregates are related as follows:

```text
Organization
      │
      ▼
Tournament Series
      │
      ▼
Tournament ─────────────► Venue
      │
      ├────────────► Participant ───────────► Player
      │
      ▼
Bracket
      │
      ▼
Match
```

## Relationship Rules

- An Organization owns zero or more Tournament Series.
- A Tournament Series contains zero or more Tournaments.
- A Tournament is held at exactly one Venue.
- A Tournament contains zero or more Participants.
- A Participant references exactly one Player.
- A Tournament owns exactly one Bracket.
- A Bracket contains one or more Matches.
- A Match is always part of a single Bracket.

---

## Organization

Represents an organization responsible for managing tournaments.

Examples:

- Hellenic American Pool Association
- Local Pool Club

Responsibilities:

- Owns tournament series.
- Defines organizational context.

---

## Tournament Series

Represents a recurring collection of tournaments.

Examples:

- Monthly Championship
- Summer Open Series

Responsibilities:

- Groups related tournaments.
- Provides historical continuity.

---

## Tournament

Represents a single tournament event.

Responsibilities:

- Defines tournament rules.
- References the venue.
- Contains participants.
- Produces the competition bracket.

---

## Venue

Represents the physical location where tournaments are held.

Responsibilities:

- Stores venue information.
- Defines geographical location.

---

## Player

Represents a person participating in tournaments.

Responsibilities:

- Stores player identity.
- Maintains historical participation.

---

## Participant

Represents a player's participation in a specific tournament.

Responsibilities:

- Connects a player with a tournament.
- Stores tournament-specific information.

---

## Bracket

Represents the tournament bracket.

Responsibilities:

- Organizes matches.
- Determines tournament progression.

---

## Match

Represents a played match between participants.

Responsibilities:

- Stores the result.
- Determines winner and loser.

---

# Value Objects

The domain uses Value Objects to model concepts that are identified by their values rather than by identity.

## VenueLocation

Represents the physical location of a venue.

Properties:

- Country
- City
- Address (optional)

---

## GameSet

Represents the discipline played during a tournament.

Examples:

- 8-Ball
- 9-Ball
- 10-Ball
- Straight Pool

---

## TournamentType

Represents the type of tournament.

Examples:

- Weekly
- Monthly
- Open
- Championship
- Team
- Handicap
- Junior
- Invitational

---

## TournamentStatus

Represents the lifecycle of a tournament.

Examples:

- Draft
- Published
- RegistrationOpen
- InProgress
- Completed
- Cancelled

---

## BracketType

Represents the competition format.

Examples:

- Single Elimination
- Double Elimination
- Round Robin