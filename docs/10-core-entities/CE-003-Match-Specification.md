# Match Specification

| Field | Value |
|------|------|
| **Document Type** | Core Entity Specification |
| **Document ID** | CE-003 |
| **Document Name** | Match Specification |
| **Version** | 1.0 |
| **Status** | Draft |
| **Review Status** | Pending Architecture Review |
| **Owner** | Domain Architecture |

---

# Purpose

Defines the Match entity of the Hellenic American Pool History Platform.

This specification establishes the responsibilities, relationships, business rules and invariants governing a Match throughout its lifecycle.

---

# Scope

This specification defines the Match as a Core Domain Entity.

It covers:

- match identity;
- match lifecycle;
- relationships with tournaments and players;
- business constraints.

Implementation details are intentionally excluded.

# Domain Definition

A Match is the core domain entity representing a competitive contest between Players within a Tournament.

A Match records the competitive event, its outcome and its progression within the Tournament structure.

A Match maintains its own identity independent of the participating Players and the Tournament to which it belongs.

# Responsibilities

The Match entity is responsible for:

- maintaining a persistent match identity;
- representing a single competitive contest;
- maintaining match-specific attributes;
- recording the outcome of the competition;
- maintaining the match lifecycle;
- preserving the historical integrity of the match.

The Match entity is not responsible for:

- managing player identity;
- managing tournament identity;
- registering tournament participation;
- calculating player rankings;
- maintaining tournament structure beyond the match itself.

# Relationships

The Match maintains the following domain relationships.

## Tournament

Each Match belongs to exactly one Tournament.

A Match cannot exist independently of a Tournament.

---

## Player

A Match involves two or more Players, depending on the competition format.

Players participate in a Match through their Tournament Participation.

The Match does not own Player entities.

---

## Tournament Participation

Player participation in a Match shall be consistent with the corresponding Tournament Participation.

A Match shall not reference Players who are not registered participants of the Tournament.

---

## Value Objects

The Match uses approved Value Objects where appropriate to represent immutable domain concepts.

---

## External Relationships

The Match does not own external entities.

Relationships are maintained through the corresponding domain entities.

# Attributes

The Match entity maintains the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Match ID | Identifier | Yes | Unique identifier of the Match. |
| Round | Round (VO) | Yes | Tournament round in which the Match is played. |
| Status | Match Status (VO) | Yes | Current administrative status of the Match. |
| Score | Score (VO) | No | Official match score when available. |
| Scheduled At | DateTime | No | Scheduled date and time of the Match. |
| Completed At | DateTime | No | Date and time the Match was completed. |
| Created At | DateTime | Yes | Date and time the Match record was created. |
| Updated At | DateTime | Yes | Date and time of the last modification. |

## Attribute Constraints

- Match ID shall be immutable.
- Round shall use the approved Round Value Object.
- Status shall use the approved Match Status Value Object.
- Score, when available, shall use the approved Score Value Object.
- Completed At shall not precede Scheduled At when both are specified.
- Created At shall be immutable.
- Updated At shall reflect the latest approved modification.

# Value Objects

The Match entity uses the following approved Value Objects:

| Value Object | Purpose |
|-------------|---------|
| Round | Represents the tournament round of the Match. |
| Match Status | Represents the administrative status of the Match. |
| Score | Represents the official result of the Match. |

No additional Value Objects are currently required by this specification.

# Business Rules

### BR-001 – Unique Match Identity

Each Match shall have one unique and persistent identifier within the platform.

---

### BR-002 – Tournament Membership

Every Match shall belong to exactly one Tournament.

---

### BR-003 – Registered Participants

Only Players registered through Tournament Participation may participate in a Match.

---

### BR-004 – Valid Round

Every Match shall belong to one valid Tournament Round.

---

### BR-005 – Match Status

Every Match shall always have one valid Match Status.

---

### BR-006 – Official Score

When a Match has been completed, the official result shall be represented using the approved Score Value Object.

---

### BR-007 – Historical Integrity

A Match shall remain identifiable regardless of its administrative status, preserving its historical record.

# Domain Invariants

### INV-001 – Persistent Identity

A Match shall always maintain the same Match ID throughout its lifetime.

---

### INV-002 – Tournament Integrity

A Match shall always belong to exactly one Tournament.

---

### INV-003 – Participant Integrity

Every Player participating in a Match shall be a registered participant of the Tournament.

---

### INV-004 – Valid Status

A Match shall always have one valid Match Status.

---

### INV-005 – Historical Preservation

A Match shall never lose its identity after completion or archival.

# Lifecycle

The Match lifecycle consists of the following states:

| State | Description |
|--------|-------------|
| Created | The Match has been created in the platform. |
| Scheduled | The Match has been scheduled but has not yet started. |
| In Progress | The Match is currently being played. |
| Completed | The Match has concluded and the official result has been recorded. |
| Archived | The Match is retained for historical purposes. |

The transition between lifecycle states shall preserve the Match identity and historical integrity.

# Notes

This specification intentionally separates the Match entity from Player and Tournament Participation.

The Match represents the competitive event itself, while player registration and eligibility are managed through Tournament Participation.

This separation preserves clear domain responsibilities and supports future expansion of competition formats.

# References

- GOV-008 – Specification Template
- SL-001 – Specification Library Index
- Domain Model v2.0
- Architecture Guide
- CE-001 – Player Specification
- CE-002 – Tournament Specification
- RE-001 – Tournament Participation Specification
- VO-004 – Round
- VO-005 – Score
- VO-007 – Match Status

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Match Specification is consistent with the Domain Model.

### Maintainability

✔ Responsibilities and relationships are clearly separated.

### Compliance

✔ Complies with GOV-008 Specification Template.

### Traceability

✔ Business Rules and Domain Invariants are uniquely identifiable and traceable.

### Scalability

✔ Supports future expansion of the Match domain without architectural changes.

---

# Review Result

| Field | Value |
|------|------|
| **Architecture Review** | Approved |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |