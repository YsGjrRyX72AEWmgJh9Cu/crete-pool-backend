# Tournament Specification

Version 1.0

---

# Purpose

This document defines the Tournament entity of the Hellenic American Pool History Platform.

It describes what a Tournament represents within the historical archive and how tournament information is organized within the Domain Model.

The specification is independent of external data sources, databases and implementation details.

Every imported Tournament is translated into the platform's own Domain Model while preserving references to its originating source.

---

# What is a Tournament?

A Tournament is an organized competitive event in which Players compete through one or more Matches under defined rules.

A Tournament provides the historical context that connects Matches into a single competition.

It preserves when, where, and under which conditions the competition took place.

Each Tournament contributes to the historical archive by recording a complete competitive event rather than an individual Match.

---

# Historical Principle

Every Tournament matters.

Historical significance is not determined by prestige, prize money or the number of participants.

Every Tournament preserves a complete chapter in the history of American Pool.

By bringing together Players, Matches, Venues, Organizations and recorded results, every Tournament contributes to the collective history of the sport.

The platform preserves every Tournament with accuracy, objectivity and respect.

---

# Design Principles

A Tournament is defined by the platform, not by external systems.

A Tournament represents a complete historical competition rather than a collection of individual Matches.

A Tournament provides the historical context that gives meaning to the Matches it brings together.

External identifiers are preserved only as references to their originating source.

A Tournament's historical record grows over time as new verified information becomes available.

---

# Tournament Structure

The following structure defines the information required by the platform to represent a Tournament within the historical archive.

The structure is defined by the Domain Model.

External data sources are mapped into this structure.

Every field must have both a historical purpose and a business purpose.

---

# Tournament Components

The Tournament entity is composed of the following components.

Each component groups together information with a common responsibility within the Domain Model.

- Identity
- Tournament Information
- Competition Context
- Participants
- Results
- External References
- Metadata

---

# Business Rules

A Tournament is identified by the platform through its own Platform Identifier.

A Tournament may be associated with one or more external sources.

External identifiers are stored as references and never replace the Platform Identifier.

A Tournament provides the historical context for the Matches that belong to it.

A Tournament's historical record grows over time as new verified information becomes available.

Historical facts are preserved even when additional information becomes available.

Historical information is never overwritten without preserving its provenance.

---

# Domain Fields

The following fields define the information required by the Domain Model to represent a Tournament.

Fields are grouped into components according to their responsibility within the entity.

Every field must have a clear historical purpose and a clear business purpose.

## Identity

The Identity component uniquely identifies a Tournament within the platform and maintains references to external sources.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| platform_id | UUID | Yes | Provides the permanent identity of the Tournament throughout the platform and its historical archive. | Uniquely identifies the Tournament across the platform. | Assigned by the platform. |

---

## Tournament Information

The Tournament Information component describes the identity of a Tournament as a historical competition.

It preserves the information required to identify, recognize and distinguish a Tournament within the historical archive.

Only information that contributes to the historical identity of the Tournament belongs to this component.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| name | String | Yes | Preserves the official name of the Tournament as part of the historical record. | Identifies the Tournament. | |
| edition | String | No | Preserves the recorded edition of the Tournament when applicable. | Distinguishes recurring Tournaments. | Examples: "2025", "12th Edition". |
| description | String | No | Preserves additional historical information describing the Tournament. | Provides historical context. | Optional historical description. |

---

## Competition Context

The Competition Context component describes the historical setting in which a Tournament takes place.

It preserves the information that defines when, where and under which conditions the Tournament was conducted.

Context information connects the Tournament to other Domain Entities without duplicating their information.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| organizer | Organization Reference | No | Preserves the organization responsible for the Tournament. | Links the Tournament to its organizing Organization. | References the Organization entity when available. |
| venue | Venue Reference | No | Preserves where the Tournament took place. | Links the Tournament to its Venue. | References the Venue entity when available. |
| start_date | Date | No | Preserves when the Tournament began, when historically known. | Supports chronology and historical search. | May be unknown for older records. |
| end_date | Date | No | Preserves when the Tournament concluded, when historically known. | Supports chronology and historical reporting. | May be unknown for older records. |
| discipline | Discipline | Yes | Preserves the discipline under which the Tournament was played. | Supports historical statistics and filtering. | Uses the Discipline Value Object. |
| category | Category | No | Preserves the competitive category of the Tournament when historically relevant. | Supports filtering and historical analysis. | Uses the Category Value Object. |

---

## Participants

The Participants component records the Players who participated in the Tournament.

Each Participant references a Player while preserving Tournament-specific participation throughout the competition.

Participants exist only within the context of a Tournament.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| player | Player Reference | Yes | Identifies the Player who participated in the Tournament. | Links the Participant to the corresponding Player. | References the Player entity. |
| final_stage | Tournament Stage | No | Preserves the furthest recorded stage reached by the Participant. | Supports historical reporting and analysis. | Uses the Tournament Stage Value Object. |
| final_position | Final Position | No | Preserves the Participant's recorded final standing in the Tournament. | Supports rankings and historical statistics. | Uses the Final Position Value Object. |
| participation_status | Participation Status | Yes | Preserves the recorded status of the Participant's Tournament participation. | Distinguishes completed, withdrawn or disqualified participation. | Uses the Participation Status Value Object. |

---

## Results

The Results component preserves the official recorded outcome of the Tournament.

It preserves the official historical outcome of the Tournament without duplicating participant information.

Only information that represents the Tournament as a whole belongs to this component.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| champion | Player Reference | No | Preserves the recorded winner of the Tournament. | Identifies the Tournament Champion. | References the Player entity. |
| status | Tournament Status | Yes | Preserves the recorded status of the Tournament. | Distinguishes completed, cancelled or unfinished Tournaments. | Uses the Tournament Status Value Object. |

---

## External References

The External References component preserves identifiers assigned by external systems.

External references maintain provenance and synchronization without replacing the Tournament's Platform Identity.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| source | String | Yes | Records the originating source of imported historical information. | Identifies the originating external source. | Examples: CueScore. |
| source_id | String | Yes | Preserves the original identifier assigned by the external source. | Enables synchronization with external systems. | Never replaces the Platform Identity. |

---

## Metadata

The Metadata component preserves information about the lifecycle of the Tournament record within the platform.

Metadata supports historical provenance, auditing and record management.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| created_at | DateTime | Yes | Records when the Tournament record was created within the platform. | Supports auditing and record management. | Assigned by the platform. |
| updated_at | DateTime | Yes | Records when the Tournament record was last updated. | Supports synchronization and auditing. | Updated by the platform. |