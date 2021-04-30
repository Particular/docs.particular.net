---
title: NHibernate Custom Saga Finding Logic
summary: Perform custom saga finding logic based on custom query logic when the Saga storage is a relational database using NHibernate as the ORM.
component: NHibernate
reviewed: 2021-04-30
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

include: sagafinder-configurehowtofindsaga
