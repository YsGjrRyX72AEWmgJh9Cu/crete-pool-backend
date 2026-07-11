# Country

| Field | Value |
|------|------|
| **Document Type** | Value Object Specification |
| **Document ID** | VO-001 |
| **Document Name** | Country |
| **Version** | 1.0 |
| **Status** | Draft |
| **Review Status** | Pending Architecture Review |
| **Owner** | Domain Architecture |

# Purpose

Defines the Country Value Object used throughout the Hellenic American Pool History Platform.

The Country Value Object represents the country associated with a domain concept while ensuring consistency, immutability and reuse across the Domain Model.

# Scope

This specification defines the Country Value Object.

It applies wherever a country must be represented within the Domain Model.

Implementation details are intentionally excluded.

# Domain Definition

Country is an immutable Value Object representing a sovereign state or territory recognized by the platform.

Country provides a standardized representation that can be reused consistently across multiple domain entities.

# Responsibilities

The Country Value Object is responsible for:

- representing a country;
- providing a standardized domain representation;
- ensuring immutable country information.

The Country Value Object is not responsible for:

- identifying entities;
- maintaining historical changes;
- managing geopolitical information.

# Relationships

Not Applicable.

Value Objects do not maintain relationships with other domain concepts.

# Lifecycle

Not Applicable.

Value Objects are immutable and therefore do not define lifecycle states.

# Attributes

The Country Value Object consists of the following attributes.

| Attribute | Type | Required | Description |
|----------|------|----------|-------------|
| Name | String | Yes | Official country name used by the platform. |
| ISO Code | String | Yes | ISO 3166-1 alpha-2 country code. |

## Attribute Constraints

- Name shall not be empty.
- ISO Code shall consist of exactly two uppercase alphabetic characters.
- The combination of Name and ISO Code shall represent the same country.

# Value Objects

Not Applicable.

Country is itself a Value Object and does not contain nested Value Objects.

# Business Rules

### BR-001 – Valid Country

Every Country shall represent a valid country recognized by the platform.

---

### BR-002 – Immutable Definition

A Country shall be immutable after creation.

---

### BR-003 – ISO Compliance

The ISO Code shall comply with ISO 3166-1 alpha-2.

# Domain Invariants

### INV-001 – Immutable Value

A Country shall never change after creation.

---

### INV-002 – Valid ISO Code

Every Country shall contain one valid ISO 3166-1 alpha-2 code.

---

### INV-003 – Consistent Representation

The Name and ISO Code shall always refer to the same country.

# Notes

Country is intended for standardized country representation across the Domain Model.

The Value Object promotes consistency, reuse and interoperability between Specifications.

# References

- GOV-008 – Specification Template
- GOV-011 – Domain Glossary
- CE-001 – Player Specification

# Architecture Review

## Architecture Assessment

### Consistency

✔ The Country Value Object is consistent with the Domain Model.

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

