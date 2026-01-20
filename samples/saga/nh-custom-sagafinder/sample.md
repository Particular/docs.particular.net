---
title: NHibernate Custom Saga Finding Logic
summary: Implement custom saga-finding behavior using custom query logic when saga data is stored in a relational database and accessed via the NHibernate ORM
component: NHibernate
reviewed: 2026-01-20
related:
- nservicebus/sagas
- persistence/nhibernate
---

include: sagafinder-intro

## Prerequisites

include: sql-prereq

This sample creates a database named `NsbSamplesNhCustomSagaFinder`.

### NHibernate setup

This sample uses the [NHibernate persistence](/persistence/nhibernate/), configured as follows:

snippet: config

include: sagafinder-thesaga

snippet: saga

include: sagafinder-process

snippet: finder

include: sagafinder-ifindsagas
