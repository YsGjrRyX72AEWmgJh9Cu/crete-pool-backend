# Tournament Status

Version 1.0

---

# Purpose

This document defines the Tournament Status Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

Tournament Status represents the lifecycle state of a Tournament.

It provides a consistent way to preserve the officially recorded lifecycle state of Tournaments throughout the platform.

---

# Definition

Tournament Status represents the current recorded state of a Tournament.

It identifies whether a Tournament is planned, in progress, completed, cancelled or remains in another valid state.

Tournament Status is an immutable Value Object and has no independent identity.

---

# Design Principles

Tournament Status has no independent identity.

Tournament Status is immutable.

Tournament Status preserves the officially recorded lifecycle state of a Tournament.

Tournament Status is reused across multiple Domain Entities.

Tournament Status remains independent of implementation details.

---

# Allowed Values

Tournament Status values are defined by the platform's controlled Tournament Status vocabulary.

Typical Tournament Status values include:

- Planned
- Registration Open
- In Progress
- Completed
- Cancelled

---

# Historical Notes

Tournament Status preserves the officially recorded state of a Tournament.

Historical Tournament Status values remain unchanged to accurately reflect the historical record.

---

# Usage

Tournament Status may be reused across multiple Domain Entities, including:

- Tournament

---

# Review Status

Architecture Review: Approved

Specification Version: 1.0

Status: Stable Reference Specification