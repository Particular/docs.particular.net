---
title: NHibernate Custom Saga Finding Logic
summary: Perform custom saga finding logic based on custom query logic when the Saga storage is a relational database using NHibernate as the O/RM.
component: NHibernate
reviewed: 2017-07-17
tags:
- Saga
- SagaFinder
related:
- nservicebus/sagas
- persistence/nhibernate
---

include: sagafinder-intro


## Prerequisites

include: sql-prereq

The database created by this sample is `NsbSamplesNhCustomSagaFinder`.


### NHibernate setup

This sample uses the [NHibernate persistence](/persistence/nhibernate/) which is configured as follows:

snippet: config


include: sagafinder-thesaga

snippet: saga

include: sagafinder-process

snippet: finder

include: sagafinder-ifindsagas

include: sagafinder-configurehowtofindsaga