# Player Specification

Version 1.0

---

# Purpose

This document defines the Player entity of the Hellenic American Pool History Platform.

It describes what a Player represents within the historical archive and how player information is organized within the Domain Model.

The specification is independent of external data sources, databases and implementation details.

Every imported player is translated into the platform's own Domain Model while preserving references to its originating source.

---

# What is a Player?

A Player is a person who becomes part of the history of American Pool through participation in the sport.

Every match played and every tournament entered become part of that player's historical record.

For that reason, the Player is one of the fundamental entities of the Domain Model.

The platform preserves each player's competitive history with accuracy, objectivity and respect.

---

# Historical Principle

Every player matters.

Historical significance is not determined by titles, rankings or victories.

Every player contributes to the history of American Pool simply by participating.

Every match played becomes part of the collective history of the sport.

The platform preserves every player's history with equal accuracy, objectivity and respect.

---

# Design Principles

The Player is defined by the platform, not by external systems.

A Player may exist in multiple external data sources.

The platform assigns its own identity to every Player.

External identifiers are preserved only as references to their originating source.

A Player's historical record grows over time as new verified information becomes available.

---

# Player Structure

The following structure defines the information required by the platform to represent a Player within the historical archive.

The structure is defined by the Domain Model.

External data sources are mapped into this structure.

Every field must have both a historical purpose and a business purpose.

---

# Player Components

The Player entity is composed of the following components.

Each component groups together information with a common responsibility within the Domain Model.

- Identity
- Personal Information
- Competitive History
- External References
- Metadata

---

# Business Rules

A Player is identified by the platform through its own Platform Identifier.

A Player may be associated with one or more external sources.

External identifiers are stored as references and never replace the Platform Identifier.

A Player's historical record grows over time as new verified information becomes available.

Historical information is never overwritten without preserving its provenance.

---

# Domain Fields

The following fields define the information required by the Domain Model to represent a Player.

Fields are grouped into components according to their responsibility within the entity.

Every field must have a clear historical purpose and a clear business purpose.

## Identity

The Identity component uniquely identifies a Player within the platform and maintains references to external sources.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| platform_id | UUID | Yes | Provides the permanent identity of the Player throughout the platform and its historical archive. | Uniquely identifies the Player across the platform. | Assigned by the platform. |
| source | String | Yes | Records the originating source of imported historical information. | Identifies the originating external source. | Examples: CueScore. |
| source_id | String | Yes | Preserves the original identifier assigned by the external source. | Enables synchronization with external systems. | Never replaces the platform identity. |

---

## Personal Information

The Personal Information component describes the information required to identify a Player as a person within the historical archive.

Only information that contributes to the accurate preservation of the Player's historical record belongs to this component.

Personal information is preserved only when it serves a clear historical and business purpose.

| Domain Field | Type | Required | Historical Purpose | Business Purpose | Notes |
|--------------|------|----------|--------------------|------------------|-------|
| first_name | String | Yes | Preserves the player's given name as part of the historical record. | Supports Player identification. | |
| last_name | String | Yes | Preserves the player's family name as part of the historical record. | Supports Player identification. | |
| nationality | Country | No | Preserves the player's nationality when historically relevant. | Supports historical statistics and filtering. | Uses the Country Value Object. |