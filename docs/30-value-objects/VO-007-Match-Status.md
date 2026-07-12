# Match Status

| Field | Value |
|------|------|
| **Document Type** | Value Object Specification |
| **Document ID** | VO-007 |
| **Document Name** | Match Status |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |
| **Review Status** | Architecture Review: Approved |
| **Owner** | Domain Architecture |

# Purpose

Defines the Match Status Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

The Match Status Value Object represents the current state of a match while ensuring consistency, immutability and reuse across the Domain Model.

# Scope

This specification defines the Match Status Value Object.

It applies wherever the current state of a match must be represented using the standardized status values defined by the Domain Model.

Implementation details are intentionally excluded.

# Domain Definition

Match Status is an immutable Value Object representing the current state of a match.

It provides a standardized and immutable representation of the current state of a match across the Domain Model.

# Responsibilities

The Match Status Value Object is responsible for:

- representing the current state of a match;
- providing a standardized representation of match states;
- ensuring immutable match status information.

The Match Status Value Object is not responsible for:

- controlling state transitions;
- determining match outcomes;
- managing tournament progression.

# Relationships

Not Applicable.

Value Objects do not maintain relationships with other domain concepts.

# Lifecycle

Not Applicable.

Value Objects are immutable and therefore do not define lifecycle states.

# Attributes

The Match Status Value Object consists of the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Status | String | Yes | Current match status. |

## Attribute Constraints

- Status shall not be empty.
- Status shall be one of the allowed values defined in this specification.

# Allowed Values

The Match Status Value Object shall use one of the following values.

| Value | Description |
|--------|-------------|
| Scheduled | Match has been scheduled but has not started. |
| In Progress | Match is currently being played. |
| Completed | Match has finished normally. |
| Cancelled | Match was cancelled before it started. |
| Abandoned | Match started but did not reach a valid completion. |

No other values are permitted.

# Value Objects

Not Applicable.

Match Status is itself a Value Object and does not contain nested Value Objects.

# Business Rules

### BR-001 – Valid Match Status

Every Match Status shall be one of the allowed values defined in this specification.

---

### BR-002 – Immutable Definition

A Match Status shall be immutable after creation.

---

### BR-003 – Standardized Representation

Match Status shall use a standardized representation throughout the Domain Model.

# Domain Invariants

### INV-001 – Valid Status

A Match Status shall always be one of the allowed values defined in this specification.

---

### INV-002 – Immutable Value

A Match Status shall never change after creation.

---

### INV-003 – Consistent Representation

The Status value shall always represent the same match state throughout the Domain Model.

# Notes

Match Status is intended for standardized representation of match states across the Domain Model.

State transitions are governed by the Match entity and are intentionally excluded from this Value Object.

The Value Object promotes consistency, reuse and interoperability between Specifications.

# References

- GOV-008 – Specification Template
- GOV-010 – Architecture Improvement Log
- GOV-011 – Domain Glossary
- CE-003 – Match Specification

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Match Status Value Object is consistent with the Domain Model.

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