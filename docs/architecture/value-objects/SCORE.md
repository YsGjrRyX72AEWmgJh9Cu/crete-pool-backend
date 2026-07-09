# Score

Version 1.0

---

# Purpose

This document defines the Score Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

Score represents the recorded result of a Match between its Participants.

It provides a consistent way to preserve historically recorded Match results throughout the platform.

---

# Definition

Score represents the officially recorded result of a Match.

It records the outcome achieved by the Match Participants.

Score is an immutable Value Object and has no independent identity.

---

# Design Principles

Score has no independent identity.

Score is immutable.

Score preserves the officially recorded Match result.

Score is reused across multiple Domain Entities.

Score remains independent of implementation details.

---

# Allowed Values

Score values are defined by the official result of a Match.

The Score format is determined by the rules of the associated Discipline.

---

# Historical Notes

Score preserves the officially recorded result of a historical Match.

Recorded Scores remain unchanged to preserve historical accuracy, even if competition formats evolve over time.

---

# Usage

Score may be reused across multiple Domain Entities, including:

- Match

---

# Review Status

Architecture Review: Approved

Specification Version: 1.0

Status: Stable Reference Specification