# Tournament Status

| Field | Value |
|------|------|
| **Document Type** | Value Object Specification |
| **Document ID** | VO-008 |
| **Document Name** | Tournament Status |
| **Version** | 1.0 |
| **Status** | Stable Reference Specification |
| **Review Status** | Architecture Review: Approved |
| **Owner** | Domain Architecture |

# Purpose

Defines the Tournament Status Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

The Tournament Status Value Object represents the current state of a tournament while ensuring consistency, immutability and reuse across the Domain Model.

# Scope

This specification defines the Tournament Status Value Object.

It applies wherever the current state of a tournament must be represented using the standardized status values defined by the Domain Model.

Implementation details are intentionally excluded.

# Domain Definition

Tournament Status is an immutable Value Object representing the current state of a tournament.

It provides a standardized and immutable representation of the current state of a tournament across the Domain Model.

# Responsibilities

The Tournament Status Value Object is responsible for:

- representing the current state of a tournament;
- providing a standardized representation of tournament states;
- ensuring immutable tournament status information.

The Tournament Status Value Object is not responsible for:

- controlling state transitions;
- managing tournament operations;
- determining tournament results.

# Relationships

Not Applicable.

Value Objects do not maintain relationships with other domain concepts.

# Lifecycle

Not Applicable.

Value Objects are immutable and therefore do not define lifecycle states.

# Attributes

The Tournament Status Value Object consists of the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Status | String | Yes | Current tournament status. |

## Attribute Constraints

- Status shall not be empty.
- Status shall be one of the allowed values defined in this specification.

# Allowed Values

The Tournament Status Value Object shall use one of the following values.

| Value | Description |
|--------|-------------|
| Planned | Tournament has been created but is not yet open for registration. |
| Registration Open | Player registration is open. |
| Registration Closed | Player registration has ended. |
| In Progress | Tournament is currently being played. |
| Completed | Tournament has finished normally. |
| Cancelled | Tournament was cancelled before completion. |

No other values are permitted.

# Value Objects

Not Applicable.

Tournament Status is itself a Value Object and does not contain nested Value Objects.

# Business Rules

### BR-001 – Valid Tournament Status

Every Tournament Status shall be one of the allowed values defined in this specification.

---

### BR-002 – Immutable Definition

A Tournament Status shall be immutable after creation.

---

### BR-003 – Standardized Representation

Tournament Status shall use a standardized representation throughout the Domain Model.

# Domain Invariants

### INV-001 – Valid Status

A Tournament Status shall always be one of the allowed values defined in this specification.

---

### INV-002 – Immutable Value

A Tournament Status shall never change after creation.

---

### INV-003 – Consistent Representation

The Status value shall always represent the same tournament state throughout the Domain Model.

# Notes

Tournament Status is intended for standardized representation of tournament states across the Domain Model.

State transitions are governed by the Tournament entity and are intentionally excluded from this Value Object.

The Value Object promotes consistency, reuse and interoperability between Specifications.

# References

- GOV-008 – Specification Template
- GOV-010 – Architecture Improvement Log
- GOV-011 – Domain Glossary
- CE-002 – Tournament Specification

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Tournament Status Value Object is consistent with the Domain Model.

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