# Score

| Field | Value |
|------|------|
| **Document Type** | Value Object Specification |
| **Document ID** | VO-005 |
| **Document Name** | Score |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |
| **Review Status** | Architecture Review: Approved |
| **Owner** | Domain Architecture |

# Purpose

Defines the Score Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

The Score Value Object represents the official score of a pool match while ensuring consistency, immutability and reuse across the Domain Model.

# Scope

This specification defines the Score Value Object.

It applies wherever a match score must be represented within the Domain Model.

Implementation details are intentionally excluded.

# Domain Definition

Score is an immutable Value Object representing the official score of a pool match.

It provides a standardized and immutable representation of the number of racks won by each player.

# Responsibilities

The Score Value Object is responsible for:

- representing the official score of a pool match;
- providing a standardized domain representation;
- ensuring immutable score information.

The Score Value Object is not responsible for:

- determining the winner of a match;
- managing match progression;
- validating tournament rules.

# Relationships

Not Applicable.

Value Objects do not maintain relationships with other domain concepts.

# Lifecycle

Not Applicable.

Value Objects are immutable and therefore do not define lifecycle states.

# Attributes

The Score Value Object consists of the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Player 1 Score | Integer | Yes | Number of racks won by Player 1. |
| Player 2 Score | Integer | Yes | Number of racks won by Player 2. |

## Attribute Constraints

- Player 1 Score shall be greater than or equal to zero.
- Player 2 Score shall be greater than or equal to zero.
- Both values shall be whole numbers.

# Value Objects

Not Applicable.

Score is itself a Value Object and does not contain nested Value Objects.

# Business Rules

### BR-001 – Non-Negative Scores

Both player scores shall be zero or greater.

---

### BR-002 – Whole Number Scores

Scores shall consist only of whole numbers.

---

### BR-003 – Immutable Definition

A Score shall be immutable after creation.

---

### BR-004 – Standardized Representation

Scores shall always represent the number of racks won by each player.

# Domain Invariants

### INV-001 – Immutable Value

A Score shall never change after creation.

---

### INV-002 – Valid Rack Counts

Both score values shall always be zero or greater.

---

### INV-003 – Whole Numbers Only

Score values shall always be whole numbers.

---

### INV-004 – Consistent Representation

The Score shall always represent the number of racks won by each player.

# Notes

Score is intended for standardized representation of pool match scores across the Domain Model.

The Value Object promotes consistency, reuse and interoperability between Specifications.

# References

- GOV-008 – Specification Template
- GOV-011 – Domain Glossary
- CE-003 – Match Specification

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Score Value Object is consistent with the Domain Model.

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