# Discipline

Version 1.0

---

# Purpose

This document defines the Discipline Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

Discipline represents the game discipline associated with a historical record.

It provides a consistent way to preserve historically recorded game disciplines throughout the platform.

---

# Definition

Discipline represents the game discipline associated with a Domain Entity or historical event.

It identifies the discipline under which a Match or Tournament was played.

Discipline is an immutable Value Object and has no independent identity.

---

# Design Principles

Discipline has no independent identity.

Discipline is immutable.

Discipline preserves the historically recorded game discipline.

Discipline is reused across multiple Domain Entities.

Discipline remains independent of implementation details.

---

# Allowed Values

Discipline values are defined by the platform's controlled discipline vocabulary.

The vocabulary includes all game disciplines recognized by the Domain Model.

---

# Historical Notes

Historical records preserve the discipline under which a Match or Tournament was originally played.

The recorded discipline remains unchanged even if terminology evolves over time.

---

# Usage

Discipline may be reused across multiple Domain Entities, including:

- Match
- Tournament

---

# Review Status

Architecture Review: Approved

Specification Version: 1.0

Status: Stable Reference Specification