# Common Value Objects

Version 1.0

---

# Purpose

This document defines the common Value Objects used throughout the Domain Model of the Hellenic American Pool History Platform.

Common Value Objects represent immutable historical concepts shared across multiple Domain Entities.

They provide a consistent vocabulary for describing historical information without introducing independent identity.

Every Domain Specification should reuse these Value Objects whenever applicable.

---

# What is a Value Object?

A Value Object represents an immutable concept within the Domain.

It has no independent identity and exists only as part of another Domain concept.

Value Objects describe historical characteristics rather than historical entities.

Two Value Objects are considered equal when all of their values are equal.

---

# Design Principles

Value Objects have no independent identity.

Value Objects are immutable.

Value Objects describe historical characteristics.

Value Objects may be reused across multiple Domain Entities.

Value Objects should remain independent of implementation details.

---

# Common Value Objects

The following Value Objects are shared across multiple Domain Entities.

Each Value Object represents a reusable historical concept within the Domain Model.

Common Value Objects provide a consistent vocabulary throughout the platform while remaining independent of implementation details.

Current Common Value Objects include:

- Country
- Discipline
- Category
- Round
- Score
- Final Position
- Match Status
- Tournament Status
- Participation Status

---

# Value Object Specifications

Each Common Value Object is defined in its own Specification.

This approach keeps individual specifications focused while allowing the Value Object library to grow consistently over time.

Current Value Object Specifications include:

- COUNTRY.md
- DISCIPLINE.md
- ROUND.md

Future Value Object Specifications include:

- CATEGORY.md
- SCORE.md
- FINAL_POSITION.md
- MATCH_STATUS.md
- TOURNAMENT_STATUS.md
- PARTICIPATION_STATUS.md

Future Value Object Specifications will be introduced as the Domain Model evolves.