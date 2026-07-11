# Tournament Specification

| Field | Value |
|------|------|
| **Document Type** | Core Entity Specification |
| **Document ID** | CE-002 |
| **Document Name** | Tournament Specification |
| **Version** | 1.0 |
| **Status** | Draft |
| **Review Status** | Pending Architecture Review |
| **Owner** | Domain Architecture |

---

# Purpose

Defines the Tournament entity of the Hellenic American Pool History Platform.

This specification establishes the responsibilities, relationships, business rules and invariants governing a Tournament throughout its lifecycle.

---

# Scope

This specification defines the Tournament as a Core Domain Entity.

It covers:

- tournament identity;
- tournament lifecycle;
- tournament structure;
- relationships with players, matches and participations;
- business constraints.

Implementation details are intentionally excluded.

# Domain Definition

A Tournament is the core domain entity representing an organized cue sports competition managed by the Hellenic American Pool History Platform.

A Tournament defines the competitive framework within which Players participate, Matches are played and Tournament Participations are recorded.

A Tournament maintains its own identity independent of participating Players, Matches and results.

# Responsibilities

The Tournament entity is responsible for:

- maintaining a persistent tournament identity;
- defining the competitive framework of the competition;
- maintaining tournament-specific attributes;
- organizing player participation through Tournament Participation;
- organizing matches according to the tournament structure;
- maintaining the tournament lifecycle;
- preserving the historical integrity of the competition.

The Tournament entity is not responsible for:

- managing player identity;
- maintaining player-specific information;
- determining match results;
- maintaining participation-specific data;
- calculating player statistics or rankings.

# Relationships

The Tournament maintains the following domain relationships.

## Tournament Participation

A Tournament may contain zero or more Tournament Participations.

Tournament Participation represents the registration and competitive involvement of individual Players within the Tournament.

---

## Player

Players are associated with a Tournament exclusively through Tournament Participation.

There is no direct ownership relationship between Tournament and Player.

---

## Match

A Tournament may contain zero or more Matches.

Each Match belongs to exactly one Tournament.

---

## Value Objects

The Tournament uses approved Value Objects where appropriate to represent immutable domain concepts.

---

## External Relationships

The Tournament does not own external entities.

Relationships are maintained through the corresponding domain entities.

# Attributes

The Tournament entity maintains the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Tournament ID | Identifier | Yes | Unique identifier of the Tournament. |
| Name | String | Yes | Official tournament name. |
| Discipline | Discipline (VO) | Yes | Cue sports discipline of the tournament. |
| Category | Category (VO) | Yes | Tournament category. |
| Status | Tournament Status (VO) | Yes | Current administrative status of the tournament. |
| Start Date | Date | No | Scheduled or actual tournament start date. |
| End Date | Date | No | Scheduled or actual tournament end date. |
| Created At | DateTime | Yes | Date and time the tournament record was created. |
| Updated At | DateTime | Yes | Date and time of the last modification. |

## Attribute Constraints

- Tournament ID shall be immutable.
- Name is required.
- Discipline shall use the approved Discipline Value Object.
- Category shall use the approved Category Value Object.
- Status shall use the approved Tournament Status Value Object.
- Start Date shall not be later than End Date when both are specified.
- Created At shall be immutable.
- Updated At shall reflect the latest approved modification.

# Value Objects

The Tournament entity uses the following approved Value Objects:

| Value Object | Purpose |
|-------------|---------|
| Discipline | Represents the cue sports discipline of the Tournament. |
| Category | Represents the competitive category of the Tournament. |
| Tournament Status | Represents the administrative status of the Tournament. |

No additional Value Objects are currently required by this specification.

# Business Rules

### BR-001 – Unique Tournament Identity

Each Tournament shall have one unique and persistent identifier within the platform.

---

### BR-002 – Mandatory Name

Every Tournament shall have an official name.

---

### BR-003 – Mandatory Discipline

Every Tournament shall specify one valid Discipline.

---

### BR-004 – Mandatory Category

Every Tournament shall specify one valid Category.

---

### BR-005 – Tournament Status

Every Tournament shall always have one valid Tournament Status.

---

### BR-006 – Participation Registration

Players shall participate in a Tournament only through Tournament Participation.

---

### BR-007 – Match Ownership

Every Match shall belong to exactly one Tournament.

---

### BR-008 – Historical Integrity

A Tournament shall remain identifiable regardless of its current administrative status, preserving the historical integrity of all related participations and matches.

# Domain Invariants

### INV-001 – Persistent Identity

A Tournament shall always maintain the same Tournament ID throughout its lifetime.

---

### INV-002 – Valid Definition

A Tournament shall always have a valid Name, Discipline and Category.

---

### INV-003 – Valid Status

A Tournament shall always have one valid Tournament Status.

---

### INV-004 – Historical Preservation

A Tournament shall never lose its identity due to completion, cancellation or archival.

---

### INV-005 – Relationship Integrity

All Tournament Participations and Matches associated with a Tournament shall reference the same persistent Tournament identity.

# Lifecycle

The Tournament lifecycle consists of the following states:

| State | Description |
|--------|-------------|
| Created | The Tournament has been created in the platform. |
| Planned | The Tournament has been scheduled but has not yet started. |
| Active | The Tournament is currently in progress. |
| Completed | The Tournament has concluded and results are final. |
| Archived | The Tournament is retained for historical purposes. |

The transition between lifecycle states shall preserve the Tournament's identity and historical integrity.

# Notes

This specification intentionally separates Tournament from Tournament Participation.

Player registrations, participation-specific data and competitive progression are represented through dedicated domain entities rather than embedded collections within the Tournament entity.

# References

- GOV-008 – Specification Template
- SL-001 – Specification Library Index
- Domain Model v2.0
- Architecture Guide
- CE-001 – Player Specification
- RE-001 – Tournament Participation Specification
- VO-002 – Discipline
- VO-003 – Category
- VO-008 – Tournament Status

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Tournament Specification is consistent with the Domain Model.

### Maintainability

✔ Responsibilities and relationships are clearly separated.

### Compliance

✔ Complies with GOV-008 Specification Template.

### Traceability

✔ Business Rules and Domain Invariants are uniquely identifiable and traceable.

### Scalability

✔ Supports future expansion of the Tournament domain without architectural changes.

---

# Review Result

| Field | Value |
|------|------|
| **Architecture Review** | Approved |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |