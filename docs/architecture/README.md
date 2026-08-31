# Hellenic American Pool History

# Architecture Overview

Version: 1.0

Status: Active

Last Updated: 2026-06-27

## Purpose

The Hellenic American Pool History platform is designed as a collection of independent components.

Each component has a single responsibility, exposes a small public API, and hides its implementation details.

The objective of this architecture is to produce a platform that is maintainable, testable, and understandable for many years.

---

# Core Principles

The architecture follows the principles defined in:

* Foundation Principle FP-0001
* The Constitution of the Platform
* ADR-0001
* ADR-0002
* ADR-0003 (planned)

---

# Current Architecture

```
                 CueScore
                     │
                     ▼
            CueScoreClient
                     │
        ┌────────────┴────────────┐
        │                         │
     HTML Pages              JSON API
        │                         │
        ▼                         ▼
    Discover                 Importer
        │
        ▼
      Queue

Database integration is planned for a future iteration.
```

---

# Components

## CueScoreClient

**Responsibility**

Communicates with CueScore and returns the requested content.

**Public API**

* get_player()
* get_player_matches()
* get_match()
* get_tournament()

---

## Discover

**Responsibility**

Discovers new entities from CueScore pages.

Current implementation:

* Discover match identifiers from a player's page.

---

## Queue

**Responsibility**

Manages the work that remains to be processed.

Current public API:

* add()
* get()
* empty()

---

## Importer

**Responsibility**

Retrieves entity data that will later be imported into the Historical Archive.

Current implementation:

* Import a match page from CueScore.

Database persistence is intentionally not implemented yet.

---

# Design Philosophy

Every component:

* Has a single responsibility.
* Exposes a minimal public API.
* Hides implementation details.
* Can be tested independently.
* Can evolve without affecting unrelated components.

---

# Current Status

| Component      | Status         |
| -------------- | -------------- |
| CueScoreClient | ✅ Stable       |
| Discover       | ✅ Stable       |
| Queue          | ✅ Stable       |
| Importer       | 🚧 In Progress |

---

# Future Components

The following components are planned but not yet implemented:

* Validator
* Orchestrator
* Database Layer
* Historical Archive
* Statistics Engine
* Public API
* Web Platform

---

This document describes the current architecture of the platform and will evolve together with the project.

---

# Guiding Principle

Architecture exists to serve the mission.

Every component, every refactoring, and every architectural decision
must contribute to the long-term sustainability of the platform.

Technology is a tool.

The Historical Archive is the mission.

If a technical decision does not serve the mission,
it is the wrong decision.
