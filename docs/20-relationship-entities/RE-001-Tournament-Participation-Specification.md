# Tournament Participation Specification

| Field | Value |
|------|------|
| **Document Type** | Relationship Entity Specification |
| **Document ID** | RE-001 |
| **Document Name** | Tournament Participation Specification |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |
| **Review Status** | Architecture Review: Approved |
| **Owner** | Domain Architecture |

---

# Purpose

Defines the Tournament Participation relationship entity of the Hellenic American Pool History Platform.

This specification establishes the responsibilities, relationships, business rules and invariants governing the participation of a Player within a Tournament.

---

# Scope

This specification defines Tournament Participation as the Relationship Entity connecting Players and Tournaments.

It covers:

- player registration;
- tournament participation;
- participation lifecycle;
- participation-specific attributes;
- business constraints.

Implementation details are intentionally excluded.

# Domain Definition

Tournament Participation is the relationship entity representing a Player's participation in a specific Tournament.

It maintains all information that belongs to the participation itself rather than to the Player or the Tournament.

Tournament Participation preserves the historical relationship between Players and Tournaments independently of Match results.

# Responsibilities

The Tournament Participation entity is responsible for:

- maintaining the relationship between one Player and one Tournament;
- maintaining participation-specific attributes;
- recording the participation lifecycle;
- preserving the historical integrity of the participation;
- providing the basis for Match eligibility within the Tournament.

The Tournament Participation entity is not responsible for:

- managing player identity;
- managing tournament identity;
- recording match results;
- calculating rankings;
- managing tournament structure.

# Relationships

The Tournament Participation maintains the following domain relationships.

## Player

Each Tournament Participation is associated with exactly one Player.

A Player may have zero or more Tournament Participations throughout their competitive history.

---

## Tournament

Each Tournament Participation is associated with exactly one Tournament.

A Tournament may contain zero or more Tournament Participations.

---

## Match

Tournament Participation establishes a Player's eligibility to participate in Matches belonging to the associated Tournament.

Tournament Participation does not own Match entities.

---

## Value Objects

The Tournament Participation uses the approved Participation Status and Final Position Value Objects.

Other approved Value Objects are not applicable to this entity.

---

## External Relationships

The Tournament Participation does not own external entities.

Relationships are maintained through the corresponding domain entities.

# Attributes

The Tournament Participation entity maintains the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Participation ID | Identifier | Yes | Unique identifier of the Tournament Participation. |
| Status | Participation Status (VO) | Yes | Current administrative status of the participation. |
| Final Position | Final Position (VO) | No | Official finishing position when available. |
| Registered At | DateTime | Yes | Date and time the Player was registered in the Tournament. |
| Created At | DateTime | Yes | Date and time the participation record was created. |
| Updated At | DateTime | Yes | Date and time of the last modification. |

## Attribute Constraints

- Participation ID shall be immutable.
- Status shall use the approved Participation Status Value Object.
- Final Position, when available, shall use the approved Final Position Value Object.
- Registered At shall be specified when the participation is created.
- Created At shall be immutable.
- Updated At shall reflect the latest approved modification.

# Value Objects

The Tournament Participation entity uses the following approved Value Objects:

| Value Object | Purpose |
|-------------|---------|
| Participation Status | Represents the administrative status of the participation. |
| Final Position | Represents the official finishing position of the Player within the Tournament. |

No additional Value Objects are currently required by this specification.

# Business Rules

### BR-001 – Unique Participation Identity

Each Tournament Participation shall have one unique and persistent identifier within the platform.

---

### BR-002 – Single Player Association

Each Tournament Participation shall be associated with exactly one Player.

---

### BR-003 – Single Tournament Association

Each Tournament Participation shall be associated with exactly one Tournament.

---

### BR-004 – Unique Participation

A Player shall not have more than one Tournament Participation for the same Tournament.

---

### BR-005 – Participation Status

Every Tournament Participation shall always have one valid Participation Status.

---

### BR-006 – Final Position

When assigned, the Final Position shall be represented using the approved Final Position Value Object.

---

### BR-007 – Match Eligibility

Only a Tournament Participation with the Participating status shall make a Player eligible to participate in Matches belonging to the associated Tournament.

---

### BR-008 – Historical Integrity

Tournament Participation shall preserve the historical relationship between the Player and the Tournament regardless of the current participation status.

# Domain Invariants

### INV-001 – Persistent Identity

A Tournament Participation shall always maintain the same Participation ID throughout its lifetime.

---

### INV-002 – Relationship Integrity

A Tournament Participation shall always reference exactly one Player and exactly one Tournament.

---

### INV-003 – Unique Participation

A Player shall never have more than one Tournament Participation within the same Tournament.

---

### INV-004 – Valid Status

A Tournament Participation shall always have one valid Participation Status.

---

### INV-005 – Historical Preservation

A Tournament Participation shall preserve its identity even after the Tournament has been completed or the Player has become inactive.

# Lifecycle

The Tournament Participation lifecycle consists of the following states:

| State | Description |
|--------|-------------|
| Created | The Tournament Participation has been created in the platform. |
| Registered | The Player has been officially registered for the Tournament. |
| Active | The Player is actively participating in the Tournament. |
| Completed | The Player has completed participation in the Tournament. |
| Archived | The Tournament Participation is retained for historical purposes. |

The transition between lifecycle states shall preserve the Participation identity and historical integrity.

# Notes

This specification intentionally separates Tournament Participation from both Player and Tournament.

Participation-specific information is maintained by this Relationship Entity, preserving clear domain boundaries and preventing duplication of responsibilities across Core Entities.

Tournament Participation serves as the authoritative source for the relationship between a Player and a Tournament.

# References

- GOV-008 – Specification Template
- SL-001 – Specification Library Index
- CE-001 – Player Specification
- CE-002 – Tournament Specification
- CE-003 – Match Specification
- VO-006 – Final Position
- VO-009 – Participation Status

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Tournament Participation Specification is consistent with the Domain Model.

### Maintainability

✔ Responsibilities and relationships are clearly separated.

### Compliance

✔ Complies with GOV-008 Specification Template.

### Traceability

✔ Business Rules and Domain Invariants are uniquely identifiable and traceable.

### Scalability

✔ Supports future expansion of tournament participation without architectural changes.

---

# Review Result

| Field | Value |
|------|------|
| **Architecture Review** | Approved |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |