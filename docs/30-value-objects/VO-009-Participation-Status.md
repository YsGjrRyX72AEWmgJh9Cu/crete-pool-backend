# Participation Status

| Field | Value |
|------|------|
| **Document Type** | Value Object Specification |
| **Document ID** | VO-009 |
| **Document Name** | Participation Status |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |
| **Review Status** | Architecture Review: Approved |
| **Owner** | Domain Architecture |

# Purpose

Defines the Participation Status Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

The Participation Status Value Object represents the current state of a tournament participation while ensuring consistency, immutability and reuse across the Domain Model.

# Scope

This specification defines the Participation Status Value Object.

It applies wherever the current state of a tournament participation must be represented using the standardized status values defined by the Domain Model.

Implementation details are intentionally excluded.

# Domain Definition

Participation Status is an immutable Value Object representing the current state of a tournament participation.

It provides a standardized and immutable representation of the current state of a tournament participation across the Domain Model.

# Responsibilities

The Participation Status Value Object is responsible for:

- representing the current state of a tournament participation;
- providing a standardized representation of participation states;
- ensuring immutable participation status information.

The Participation Status Value Object is not responsible for:

- controlling state transitions;
- managing tournament participation;
- determining tournament results.

# Relationships

Not Applicable.

Value Objects do not maintain relationships with other domain concepts.

# Lifecycle

Not Applicable.

Value Objects are immutable and therefore do not define lifecycle states.

# Attributes

The Participation Status Value Object consists of the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Status | String | Yes | Current participation status. |

## Attribute Constraints

- Status shall not be empty.
- Status shall be one of the allowed values defined in this specification.

# Allowed Values

The Participation Status Value Object shall use one of the following values.

| Value | Description |
|--------|-------------|
| Registered | Player is officially registered for the tournament. |
| Participating | Player is actively participating in the tournament. |
| Eliminated | Player has been eliminated from the tournament. |
| Completed | Player has completed participation in the tournament. |
| Withdrawn | Player withdrew before completing participation. |
| Disqualified | Player was disqualified from the tournament. |

No other values are permitted.

# Value Objects

Not Applicable.

Participation Status is itself a Value Object and does not contain nested Value Objects.

# Business Rules

### BR-001 – Valid Participation Status

Every Participation Status shall be one of the allowed values defined in this specification.

---

### BR-002 – Immutable Definition

A Participation Status shall be immutable after creation.

---

### BR-003 – Standardized Representation

Participation Status shall use a standardized representation throughout the Domain Model.

# Domain Invariants

### INV-001 – Valid Status

A Participation Status shall always be one of the allowed values defined in this specification.

---

### INV-002 – Immutable Value

A Participation Status shall never change after creation.

---

### INV-003 – Consistent Representation

The Status value shall always represent the same participation state throughout the Domain Model.

# Notes

Participation Status is intended for standardized representation of tournament participation states across the Domain Model.

State transitions are governed by the Tournament Participation entity and are intentionally excluded from this Value Object.

The Value Object promotes consistency, reuse and interoperability between Specifications.

# References

- GOV-008 – Specification Template
- GOV-010 – Architecture Improvement Log
- GOV-011 – Domain Glossary
- RE-001 – Tournament Participation Specification

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Participation Status Value Object is consistent with the Domain Model.

### Maintainability

✔ The Value Object is simple, immutable and reusable.

### Compliance

✔ Complies with GOV-008 Specification Template.

### Traceability

✔ Referenced by approved Specifications.

### Scalability

✔ Suitable for reuse across future domain concepts.

---

# Review Result

| Field | Value |
|------|------|
| **Architecture Review** | Approved |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |