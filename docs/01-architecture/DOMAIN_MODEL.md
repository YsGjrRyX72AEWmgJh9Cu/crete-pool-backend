# Domain Model

> This document describes the business domain of the Hellenic American Pool History project.

---

# Purpose

The Hellenic American Pool History domain is responsible for preserving the historical record of pocket billiards competitions.

The primary goal of the system is to accurately represent players, tournaments, venues, organizations, tournament series, participations and match results while preserving historical integrity.

The domain is designed following Domain-Driven Design (DDD) principles using:

- Aggregates
- Entities
- Value Objects
- Strongly Typed Identifiers
- Domain Events (planned)

This document is the authoritative description of the business domain.

---

# Core Aggregates

The domain is organized around the following aggregates:

- Organization
- Tournament Series
- Tournament
- Venue
- Player
- Participation
- Match

The following aggregates are planned for future implementation:

- Bracket
- Statistics
- Rankings

---

# Aggregate Relationships

```text
Organization
      │
      ▼
Tournament Series
      │
      ▼
Tournament ─────────────► Venue
      │
      ▼
Participation ──────────► Player
      │
      ▼
Match

            (Future)

Match
      │
      ▼
Statistics
      │
      ▼
Rankings
```

---

# Relationship Rules

- An Organization owns zero or more Tournament Series.
- A Tournament Series contains zero or more Tournaments.
- A Tournament is held at exactly one Venue.
- A Tournament contains zero or more Participations.
- A Participation references exactly one Player.
- A Match references exactly two Participations.
- Statistics are calculated from completed Matches.
- Rankings are calculated from Statistics and Tournament Results.

---

# Organization

Represents an organization responsible for managing tournaments.

Examples

- Hellenic American Pool Association
- Local Pool Club

Responsibilities

- Owns Tournament Series.
- Defines the organizational context.
- Preserves organizational history.

---

# Tournament Series

Represents a recurring collection of tournaments.

Examples

- Monthly Championship
- Summer Open Series

Responsibilities

- Groups related tournaments.
- Provides historical continuity.

---

# Tournament

Represents a single tournament event.

Responsibilities

- Defines tournament information.
- References a Venue.
- Owns Participations.
- Produces Matches.
- Produces Tournament Results.

---

# Venue

Represents the physical location where tournaments are held.

Responsibilities

- Stores venue information.
- Stores geographical information.
- Hosts tournaments.

---

# Player

Represents a person participating in tournaments.

Responsibilities

- Stores player identity.
- Stores personal information.

Historical participation is derived from Participations.

---

# Participation

Represents a player's participation in a tournament.

This aggregate connects Players with Tournaments.

Responsibilities

- References one Player.
- References one Tournament.
- Stores Registration Date.
- Stores Seed.
- Stores Participation Status.

Future responsibilities

- Final Position.
- Prize Money.
- Notes.

---

# Match

Represents a single played match.

Responsibilities

- References exactly two Participations.
- Stores match result.
- Determines Winner.
- Determines Loser.

A Match never references Players directly.

---

# Planned Aggregate — Bracket

Represents the tournament bracket.

Responsibilities

- Organizes Matches.
- Determines tournament progression.
- Supports multiple competition formats.

---

# Planned Aggregate — Statistics

Represents calculated player statistics.

Statistics are derived from completed Matches.

Examples

- Matches Played
- Wins
- Losses
- Win Percentage
- Finals
- Titles

Statistics are never stored manually.

---

# Planned Aggregate — Rankings

Represents calculated player rankings.

Rankings are derived from Tournament Results and Statistics.

Examples

- Historical Ranking
- Seasonal Ranking
- Organization Ranking

Rankings are never stored manually.

---

# Value Objects

The domain uses Value Objects to model concepts that are identified by value rather than identity.

---

## VenueLocation

Represents the physical location of a Venue.

Properties

- Country
- City
- Address (optional)

---

## GameSet

Represents the discipline played during a tournament.

Examples

- 8-Ball
- 9-Ball
- 10-Ball
- Straight Pool

---

## TournamentType

Represents the type of tournament.

Examples

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

Examples

- Draft
- Published
- RegistrationOpen
- InProgress
- Completed
- Cancelled

---

## BracketType

Represents the competition format.

Examples

- Single Elimination
- Double Elimination
- Round Robin

---

# Architectural Decisions

## ADR-0001

Participation is the official aggregate representing a player's registration in a tournament.

The terms Participant and TournamentEntry will not coexist within the domain.

---

## ADR-0002

A Match references Participations instead of Players.

This allows matches to belong to a specific tournament participation rather than directly to a player.

---

## ADR-0003

Statistics are calculated.

They are never stored manually.

---

## ADR-0004

Rankings are calculated.

They are never stored manually.

---

# Future Evolution

The domain is designed to support future expansion without breaking the existing model.

Planned future capabilities include:

- Bracket generation
- Double Elimination
- Round Robin
- Tournament Results
- Historical Statistics
- Historical Rankings
- Hall of Fame
- Awards
- Head-to-Head Records

---

End of Document