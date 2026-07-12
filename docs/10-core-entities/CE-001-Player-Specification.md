# Player Specification

| Field | Value |
|------|------|
| **Document Type** | Core Entity Specification |
| **Document ID** | CE-001 |
| **Document Name** | Player Specification |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |
| **Review Status** | Architecture Review: Approved |
| **Owner** | Domain Architecture |

---

# Purpose

Defines the Player entity of the Hellenic American Pool History Platform.

This specification establishes the responsibilities, relationships, business rules and invariants governing a Player throughout the domain.

---

# Scope

This specification defines the Player as a Core Domain Entity.

It covers:

- player identity;
- player lifecycle;
- relationships with tournaments and matches;
- participation history;
- business constraints.

Implementation details are intentionally excluded.

---

# Domain Definition

A Player is the core domain entity representing a person whose competitive cue sports history is managed by the Hellenic American Pool History Platform.

A Player maintains a persistent identity independent of tournaments, matches and participation records.

Competitive activity is represented through relationships with Tournaments, Matches and Tournament Participations rather than being embedded within the Player itself.

# Responsibilities

The Player entity is responsible for:

- maintaining a persistent player identity;
- representing a single competitor within the domain;
- maintaining player-specific attributes;
- serving as the aggregation point for the player's competitive history;
- participating in tournaments through Tournament Participation;
- participating in matches according to tournament progression.

The Player entity is not responsible for:

- tournament organization;
- match scheduling;
- tournament progression;
- ranking calculations;
- participation-specific data.

# Relationships

The Player maintains the following domain relationships:

## Tournament Participation

A Player may participate in zero or more Tournament Participations.

Tournament Participation represents the player's registration and competitive involvement within a specific Tournament.

---

## Tournament

A Player is associated with Tournaments through Tournament Participation.

There is no direct ownership relationship between Player and Tournament.

---

## Match

A Player may participate in zero or more Matches.

Match participation is determined by tournament progression and recorded independently of the Player entity.

---

## Value Objects

The Player uses approved Value Objects where appropriate to represent immutable domain concepts.

The Player currently uses only the Country Value Object.

Other approved Value Objects are not applicable to this entity.

---

## External Relationships

The Player does not own or manage external entities.

Relationships are maintained through the corresponding domain entities.

# Attributes

The Player entity maintains the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Player ID | Identifier | Yes | Unique identifier of the Player. |
| First Name | String | Yes | Player's given name. |
| Last Name | String | Yes | Player's family name. |
| Display Name | String | No | Preferred public display name. |
| Country | Country (VO) | No | Country represented by the Player. |
| Status | Player Status | Yes | Indicates the administrative status of the Player. |
| Created At | DateTime | Yes | Date and time the Player record was created. |
| Updated At | DateTime | Yes | Date and time of the last modification. |

## Attribute Constraints

- Player ID shall be immutable.
- First Name is required.
- Last Name is required.
- Display Name is optional.
- Country shall use the approved Country Value Object.
- Status shall always contain a valid Player Status.
- Created At shall be immutable.
- Updated At shall reflect the latest approved modification.

# Value Objects

The Player entity uses the following approved Value Objects:

| Value Object | Purpose |
|-------------|---------|
| Country | Represents the country associated with the Player. |

No additional Value Objects are currently required by this specification.

# Business Rules

### BR-001 – Unique Player Identity

Each Player shall have one unique and persistent identifier within the platform.

---

### BR-002 – Mandatory Name

A Player shall always have a First Name and a Last Name.

---

### BR-003 – Immutable Identity

The Player ID shall never change after the Player has been created.

---

### BR-004 – Country Representation

When specified, the Player's Country shall be represented using the approved Country Value Object.

---

### BR-005 – Administrative Status

Every Player shall always have a valid Player Status.

---

### BR-006 – Historical Integrity

A Player shall remain identifiable even if no longer active, ensuring the integrity of historical tournament and match records.

# Domain Invariants

### INV-001 – Persistent Identity

A Player shall always maintain the same Player ID throughout its lifetime.

---

### INV-002 – Valid Identity

A Player shall always have a valid First Name and Last Name.

---

### INV-003 – Valid Status

A Player shall always have one valid Player Status.

---

### INV-004 – Historical Preservation

A Player shall never lose its identity due to retirement, inactivity or the absence of current tournament participation.

---

### INV-005 – Relationship Integrity

All Tournament Participations and Match records associated with a Player shall reference the same persistent Player identity.

# Lifecycle

The Player lifecycle consists of the following states:

| State | Description |
|--------|-------------|
| Created | The Player has been registered in the platform. |
| Active | The Player is eligible to participate in competitions. |
| Inactive | The Player remains in the platform but is not currently active. |
| Retired | The Player has permanently ended competitive participation while preserving historical records. |

The transition between lifecycle states shall preserve the Player's identity and historical integrity.

# Notes

This specification intentionally separates the Player entity from Tournament Participation.

Competitive history is represented through relationships rather than embedded collections within the Player entity.

# References

- GOV-008 – Specification Template
- SL-001 – Specification Library Index
- RE-001 – Tournament Participation Specification
- VO-001 – Country

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Player Specification is consistent with the Domain Model.

### Maintainability

✔ Responsibilities and relationships are clearly separated.

### Compliance

✔ Complies with GOV-008 Specification Template.

### Traceability

✔ Business Rules and Domain Invariants are uniquely identifiable and traceable.

### Scalability

✔ Supports future expansion of the Player domain without architectural changes.

---

# Review Result

| Field | Value |
|------|------|
| **Architecture Review** | Approved |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |