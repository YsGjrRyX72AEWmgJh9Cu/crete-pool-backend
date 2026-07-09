# Round

Version 1.0

---

# Purpose

This document defines the Round Value Object used throughout the Domain Model of the Hellenic American Pool History Platform.

Round represents the stage of progression within a Tournament.

It provides a consistent way to preserve historically recorded competition rounds throughout the platform.

---

# Definition

Round represents the stage of progression within a Tournament.

It identifies the competitive stage in which a Match is played.

Round is an immutable Value Object and has no independent identity.

---

# Design Principles

Round has no independent identity.

Round is immutable.

Round preserves the historically recorded stage of competition.

Round is reused across multiple Domain Entities.

Round remains independent of implementation details.

---

# Allowed Values

Round values are defined by the platform's controlled competition stage vocabulary.

The vocabulary includes all competition rounds recognized by the Domain Model.

---

# Historical Notes

Round preserves the stage of competition as historically recorded.

Historical tournaments may use different round naming conventions.

The recorded Round remains unchanged to preserve historical accuracy.

---

# Usage

Round may be reused across multiple Domain Entities, including:

- Match
- Tournament

---

# Review Status

Architecture Review: Approved

Specification Version: 1.0

Status: Stable Reference Specification