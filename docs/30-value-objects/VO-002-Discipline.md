# Discipline

| Field | Value |
|------|------|
| **Document Type** | Value Object Specification |
| **Document ID** | VO-002 |
| **Document Name** | Discipline |
| **Version** | 1.0 |
| **Status** | Draft |
| **Review Status** | Pending Architecture Review |
| **Owner** | Domain Architecture |

# Purpose

Defines the Discipline Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

The Discipline Value Object represents the official pool discipline associated with a Tournament while ensuring consistency, immutability and reuse across the Domain Model.

# Scope

This specification defines the Discipline Value Object.

It applies wherever a pool discipline must be represented within the Domain Model.

Implementation details are intentionally excluded.

# Domain Definition

Discipline is an immutable Value Object representing the official pool discipline associated with a Tournament.

It provides a standardized and immutable representation that can be reused consistently across the Domain Model.

# Responsibilities

The Discipline Value Object is responsible for:

- representing an official pool discipline;
- providing a standardized domain representation;
- ensuring immutable discipline information.

The Discipline Value Object is not responsible for:

- defining game rules;
- managing tournaments;
- managing matches.

# Relationships

Not Applicable.

Value Objects do not maintain relationships with other domain concepts.

# Lifecycle

Not Applicable.

Value Objects are immutable and therefore do not define lifecycle states.

# Attributes

The Discipline Value Object consists of the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Name | String | Yes | Official name of the pool discipline. |

## Attribute Constraints

- Name shall not be empty.
- Name shall uniquely identify the discipline within the platform.

# Value Objects

Not Applicable.

Discipline is itself a Value Object and does not contain nested Value Objects.

# Business Rules

### BR-001 – Valid Discipline

Every Discipline shall represent an official pool discipline recognized by the platform.

---

### BR-002 – Immutable Definition

A Discipline shall be immutable after creation.

---

### BR-003 – Standardized Representation

Every Discipline shall use a standardized name throughout the Domain Model.

# Domain Invariants

### INV-001 – Immutable Value

A Discipline shall never change after creation.

---

### INV-002 – Valid Discipline

Every Discipline shall represent one valid pool discipline.

---

### INV-003 – Consistent Representation

The discipline name shall always represent the same pool discipline throughout the Domain Model.

# Notes

Discipline is intended for standardized representation of official pool disciplines across the Domain Model.

The Value Object promotes consistency, reuse and interoperability between Specifications.

# References

- GOV-008 – Specification Template
- GOV-011 – Domain Glossary
- CE-002 – Tournament Specification

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Discipline Value Object is consistent with the Domain Model.

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