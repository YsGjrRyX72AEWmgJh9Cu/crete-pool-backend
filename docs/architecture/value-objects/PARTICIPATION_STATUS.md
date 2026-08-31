# Participation Status

Version 1.0

---

# Purpose

This document defines the Participation Status Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

Participation Status represents the lifecycle state of a Participant's involvement in a Tournament.

It provides a consistent way to preserve the officially recorded participation state throughout the platform.

---

# Definition

Participation Status represents the current recorded state of a Participant's involvement in a Tournament.

It identifies whether a Participant is registered, active, withdrawn, disqualified or remains in another valid state.

Participation Status is an immutable Value Object and has no independent identity.

---

# Design Principles

Participation Status has no independent identity.

Participation Status is immutable.

Participation Status preserves the officially recorded participation state.

Participation Status is reused across multiple Domain Entities.

Participation Status remains independent of implementation details.

---

# Allowed Values

Participation Status values are defined by the platform's controlled Participation Status vocabulary.

Typical Participation Status values include:

- Registered
- Active
- Withdrawn
- Disqualified
- Completed

---

# Historical Notes

Participation Status preserves the officially recorded participation state.

Historical Participation Status values remain unchanged to preserve historical accuracy.

---

# Usage

Participation Status may be reused across multiple Domain Entities, including:

- Tournament Participation

---

# Review Status

Architecture Review: Approved

Specification Version: 1.0

Status: Stable Reference Specification