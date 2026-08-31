# Repository Structure

## Purpose

This document defines the architectural structure of the Hellenic American Pool History Platform repository.

The repository is organized by responsibility.

Every directory has a defined purpose.

Every file belongs to a specific architectural layer.

A clean structure allows the platform to grow without losing its history.

---

## Root Directory

The root directory contains only production entry points and project configuration.

Examples:

- main.py
- database.py
- models.py
- rating.py
- requirements.txt

The root should remain small.

---

## crawler/

The crawler package contains reusable production components.

Examples:

- CueScoreClient
- Discover
- Queue
- Importer
- Parsers
- Validator

No executable scripts belong here.

Every component has a single responsibility.

---

## scripts/

The scripts directory contains development and operational tools.

Scripts may use the crawler library.

The crawler library must never depend on scripts.

Examples:

- discover_matches.py
- discover_all.py
- discover_tournaments.py
- import_cuescore.py
- search_player.py

---

## docs/

Contains architecture, principles and historical documentation.

Documentation explains why the project is designed this way.

Architecture documentation is considered part of the project.

---

## legacy/

Contains previous implementations that have been replaced.

Historical implementations are preserved.

History is never silently deleted.

---

## Repository Principles

Structure enables growth.

A clean repository allows the platform to evolve safely.

Architecture is preserved together with source code.

Every new file must have a clearly defined responsibility before it is created.
