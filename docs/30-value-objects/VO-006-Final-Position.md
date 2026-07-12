# Final Position

| Field | Value |
|------|------|
| **Document Type** | Value Object Specification |
| **Document ID** | VO-006 |
| **Document Name** | Final Position |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |
| **Review Status** | Architecture Review: Approved |
| **Owner** | Domain Architecture |

# Purpose

Defines the Final Position Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

The Final Position Value Object represents the official final tournament position assigned to a tournament participant while ensuring consistency, immutability and reuse across the Domain Model.

# Scope

This specification defines the Final Position Value Object.

It applies wherever the final ranking of a tournament participant must be represented within the Domain Model.

Implementation details are intentionally excluded.

# Domain Definition

Final Position is an immutable Value Object representing the official final tournament position assigned to a tournament participant.

It provides a standardized and immutable representation of the official final tournament position across the Domain Model.

# Responsibilities

The Final Position Value Object is responsible for:

- representing the official final tournament position of a tournament participant;
- providing a standardized representation of final tournament positions;
- ensuring immutable final position information.

The Final Position Value Object is not responsible for:

- calculating tournament rankings;
- determining player eligibility;
- managing tournament progression.

# Relationships

Not Applicable.

Value Objects do not maintain relationships with other domain concepts.

# Lifecycle

Not Applicable.

Value Objects are immutable and therefore do not define lifecycle states.

# Attributes

The Final Position Value Object consists of the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Position | String | Yes | Official final tournament position (for example: 1, 2, 3, 5-8, 9-16). |

## Attribute Constraints

- Position shall not be empty.
- Position shall represent an official final tournament position recognized by the platform.

# Value Objects

Not Applicable.

Final Position is itself a Value Object and does not contain nested Value Objects.

# Business Rules

### BR-001 – Valid Final Position

Every Final Position shall represent an official final tournament position recognized by the platform.

---

### BR-002 – Immutable Definition

A Final Position shall be immutable after creation.

---

### BR-003 – Standardized Representation

Final Position shall use a standardized representation throughout the Domain Model.

# Domain Invariants

### INV-001 – Immutable Value

A Final Position shall never change after creation.

---

### INV-002 – Valid Position

Every Final Position shall represent one valid official tournament position.

---

### INV-003 – Consistent Representation

The Position value shall always represent the same official final tournament position.

# Notes

Final Position is intended for standardized representation of official final tournament positions across the Domain Model.

The Value Object promotes consistency, reuse and interoperability between Specifications.

# References

- GOV-008 – Specification Template
- GOV-011 – Domain Glossary
- RE-001 – Tournament Participation Specification

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Final Position Value Object is consistent with the Domain Model.

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