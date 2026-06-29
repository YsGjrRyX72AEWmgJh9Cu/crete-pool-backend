# Platform Architecture

# Hellenic American Pool History

---

## Purpose

This document describes the high-level architecture of the Platform.

It explains how the different components collaborate to achieve the mission of preserving the competitive history of American Pool in Greece.

The architecture is intentionally simple.

Every component has one responsibility.

---

# High-Level Architecture

```
                CueScore

                    │
                    ▼
             CueScore Client

                    │
                    ▼
               Discovery

                    │
                    ▼
               Validation

                    │
                    ▼
                Importer

                    │
                    ▼
               PostgreSQL

                    │
        ┌───────────┼───────────┐
        ▼           ▼           ▼

     Ratings   Statistics    REST API

                    │
                    ▼

              Web Platform
```

---

# Components

## CueScore Client

Responsible for communicating with CueScore.

It does not know anything about the database.

---

## Discovery

Finds players, matches and tournaments.

---

## Validation

Determines whether imported data belongs to the supported disciplines of the Platform.

---

## Importer

Stores validated data into PostgreSQL.

---

## Database

Stores the historical archive of the Platform.

---

## Rating Engine

Calculates player ratings.

---

## REST API

Provides data to external applications.

---

## Web Platform

Presents the historical archive to the community.

---

# Design Principles

Every component should have a single responsibility.

Components communicate through well-defined interfaces.

The architecture should remain understandable, maintainable and extensible.

---

Hellenic American Pool History

Preserving the History of Greek American Pool
