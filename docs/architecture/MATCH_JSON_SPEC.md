# Match JSON Specification

Version 1.0

---

# Purpose

This document describes the CueScore Match JSON model and how it is mapped into the Hellenic American Pool History Platform.

It defines:

- every field returned by the CueScore Match API
- whether the field is used
- the internal domain name
- future implementation notes

This document is the reference specification for all Match-related development.

---

# What is a Match?

A Match is the fundamental historical event of the platform.

It represents a competitive encounter between players within a tournament.

Every statistic, rating, ranking and historical record originates from a Match.

For that reason, the Match is one of the core entities of the Domain Model.

The platform stores only the information that contributes to preserving the historical record of the match.

---

# Match Structure

The following table defines how every field from the CueScore Match API is interpreted by the platform.

Each field is evaluated according to:

- its domain ownership
- whether it is stored
- why it is stored
- how it contributes to the historical archive

| CueScore Field | Type | Domain Entity | Internal Name | Used | Business Purpose | Notes |
|----------------|------|---------------|---------------|------|------------------|-------|

---

## Design Decision

> This decision will become Architecture Decision Record (ADR-001).
The platform defines its own identity for every entity.

External systems provide references, not identities.

Every imported entity stores:

- the platform identifier
- the external identifier
- the source of the external identifier

This design allows the platform to integrate historical data from multiple independent sources while preserving a single Domain Model.