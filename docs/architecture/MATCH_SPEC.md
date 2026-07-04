# Match Specification

Version 1.0

---

# Purpose

This document defines the Match entity of the Hellenic American Pool History Platform.

It describes what a Match represents within the historical archive and how match information is organized within the Domain Model.

The specification is independent of external data sources, databases and implementation details.

Every imported match is translated into the platform's own Domain Model while preserving references to its originating source.

---

# What is a Match?

A Match is a historical event in which Players compete under defined conditions.

Every Match records a moment in the history of American Pool.

It connects Players, Tournaments, Venues and Results into a single historical record.

Each Match contributes to the historical archive by preserving when, where and how competition took place.

---

# Historical Principle

Every Match matters.

Historical significance is not determined by the importance of the tournament or the outcome of the Match.

Every Match represents a unique moment in the history of American Pool.

Each Match preserves the participation of the Players, the conditions of competition and the recorded result.

The platform preserves every Match with accuracy, objectivity and respect.

---

# Design Principles

A Match is defined by the platform, not by external systems.

A Match always belongs to the historical archive.

A Match connects multiple Domain Entities without duplicating their information.

External identifiers are preserved only as references to their originating source.

A Match becomes immutable once its historical record has been verified, except when corrections are required to preserve historical accuracy.

---

# Match Structure

The following structure defines the information required by the platform to represent a Match within the historical archive.

The structure is defined by the Domain Model.

External data sources are mapped into this structure.

Every field must have both a historical purpose and a business purpose.

---

# Match Components

The Match entity is composed of the following components.

Each component groups together information with a common responsibility within the Domain Model.

- Identity
- Participants
- Context
- Result
- External References
- Metadata

## Participants

The Participants component records the entities that take part in the Match.

Each Participant references a Player while preserving Match-specific information such as role and result.

Participants exist only within the context of a Match.

### Participant

A Participant represents a Player within the context of a specific Match.

A Participant does not exist independently.

Its purpose is to preserve the role and outcome of a Player's participation in a particular Match.

A Participant always references exactly one Player and exists only within a Match.

---

# Business Rules

A Match is identified by the platform through its own Platform Identifier.

A Match always contains at least two Participants.

Every Participant references exactly one Player.

A Match may belong to a Tournament.

A Match preserves the recorded result without modifying the historical facts.

Historical information is never overwritten without preserving its provenance.

---

# Domain Fields

The following fields define the information required by the Domain Model to represent a Match.

Fields are grouped into components according to their responsibility within the entity.

Every field must have a clear historical purpose and a clear business purpose.

## Identity

The Identity component uniquely identifies a Match within the platform and maintains references to external sources.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| platform_id | UUID | Yes | Provides the permanent identity of the Match throughout the platform and its historical archive. | Uniquely identifies the Match across the platform. | Assigned by the platform. |

---

## Participants

The Participants component defines the entities that participate in a Match.

Each Participant references exactly one Player while preserving Match-specific information.

Participants exist only within the context of a Match.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| player | Player Reference | Yes | Identifies the historical participant of the Match. | Links the Participant to the corresponding Player. | References the Player entity. |
| role | Participant Role | Yes | Preserves the Participant's role within the Match. | Distinguishes Participants when necessary. | Uses the Participant Role Value Object. |
| outcome | Match Outcome | Yes | Preserves the recorded outcome for the Participant. | Supports historical reporting and statistics. | Uses the Match Outcome Value Object. |

---

## Context

The Context component describes the historical setting in which a Match takes place.

It records the competition, location and timing that provide meaning to the Match within the historical archive.

Context information connects the Match to other Domain Entities without duplicating their information.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| tournament | Tournament Reference | Yes | Identifies the Tournament in which the Match was played. | Links the Match to its Tournament. | References the Tournament entity. |
| round | Round | Yes | Preserves the stage of the competition. | Supports tournament progression and filtering. | Uses the Round Value Object. |
| match_date | Date | No | Preserves when the Match took place, when historically known. | Supports chronological ordering and historical search. | May be unknown for older records. |
| venue | Venue Reference | No | Preserves where the Match was played. | Links the Match to its Venue. | References the Venue entity when available. |
| discipline | Discipline | Yes | Preserves the discipline under which the Match was played. | Supports filtering and statistics. | Uses the Discipline Value Object defined by the Domain Model. |

---

## Result

The Result component preserves the recorded outcome of the Match.

It records the final competitive result without duplicating participant information.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| winner | Player Reference | No | Identifies the recorded winner of the Match. | Supports rankings and historical statistics. | References the winning Player when applicable. |
| score | Score | No | Preserves the officially recorded Match score. | Supports historical reporting and statistics. | Uses the Score Value Object. |
| status | Match Status | Yes | Preserves the recorded status of the Match. | Distinguishes completed, unfinished or cancelled Matches. | Uses the Match Status Value Object. |

---

## External References

The External References component preserves links between the Match and external historical sources.

External references provide traceability without affecting the platform's own identity.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| source | String | Yes | Records the originating source of the Match information. | Identifies the external source. | Examples: CueScore. |
| source_id | String | Yes | Preserves the original identifier assigned by the external source. | Supports synchronization and traceability. | Never replaces the Platform Identity. |
| source_url | URL | No | Preserves a reference to the original historical record when available. | Supports verification and auditing. | Optional external reference. |

---

## Metadata

The Metadata component preserves information about the lifecycle of the Match record within the platform.

Metadata supports traceability, maintenance and historical integrity.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| created_at | DateTime | Yes | Records when the Match record was created within the platform. | Supports auditing. | Assigned by the platform. |
| updated_at | DateTime | Yes | Records the most recent update to the Match record. | Supports synchronization and maintenance. | Updated automatically. |
| verification_status | Verification Status | Yes | Indicates the current verification state of the historical record. | Supports data quality management. | Uses the Verification Status Value Object. |