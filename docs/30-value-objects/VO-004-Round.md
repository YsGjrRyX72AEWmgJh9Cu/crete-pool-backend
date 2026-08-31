# Round

| Field | Value |
|------|------|
| **Document Type** | Value Object Specification |
| **Document ID** | VO-004 |
| **Document Name** | Round |
| **Version** | 1.0 |
| **Status** | Draft |
| **Review Status** | Pending Architecture Review |
| **Owner** | Domain Architecture |

# Purpose

Defines the Round Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

The Round Value Object represents the official stage of a Tournament while ensuring consistency, immutability and reuse across the Domain Model.

# Scope

This specification defines the Round Value Object.

It applies wherever a tournament round must be represented within the Domain Model.

Implementation details are intentionally excluded.

# Domain Definition

Round is an immutable Value Object representing the official stage of a Tournament.

It provides a standardized and immutable representation that can be reused consistently across the Domain Model.

# Responsibilities

The Round Value Object is responsible for:

- representing an official tournament round;
- providing a standardized domain representation;
- ensuring immutable round information.

The Round Value Object is not responsible for:

- determining tournament progression;
- managing tournament brackets;
- defining competition rules.

# Relationships

Not Applicable.

Value Objects do not maintain relationships with other domain concepts.

# Lifecycle

Not Applicable.

Value Objects are immutable and therefore do not define lifecycle states.

# Attributes

The Round Value Object consists of the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Name | String | Yes | Official name of the tournament round. |

## Attribute Constraints

- Name shall not be empty.
- Name shall uniquely identify the tournament round within the platform.

# Value Objects

Not Applicable.

Round is itself a Value Object and does not contain nested Value Objects.

# Business Rules

### BR-001 – Valid Round

Every Round shall represent an official tournament stage recognized by the platform.

---

### BR-002 – Immutable Definition

A Round shall be immutable after creation.

---

### BR-003 – Standardized Representation

Every Round shall use a standardized name throughout the Domain Model.

# Domain Invariants

### INV-001 – Immutable Value

A Round shall never change after creation.

---

### INV-002 – Valid Round

Every Round shall represent one valid tournament stage.

---

### INV-003 – Consistent Representation

The round name shall always represent the same tournament stage throughout the Domain Model.

# Notes

Round is intended for standardized representation of official tournament stages across the Domain Model.

The Value Object promotes consistency, reuse and interoperability between Specifications.

# References

- GOV-008 – Specification Template
- GOV-011 – Domain Glossary
- CE-002 – Tournament Specification
- CE-003 – Match Specification

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Round Value Object is consistent with the Domain Model.

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