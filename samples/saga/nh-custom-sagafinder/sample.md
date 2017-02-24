---
title: NHibernate Custom Saga Finding Logic
summary: Perform custom saga finding logic based on custom query logic when the Saga storage is a relational database using NHibernate as the O/RM.
component: NHibernate
reviewed: 2017-02-24
tags:
- Saga
- SagaFinder
related:
- nservicebus/sagas
- nservicebus/nhibernate
---

include: sagafinder-into


### NHibernate setup

This sample requires [NHibernate persistence](https://www.nuget.org/packages/NServiceBus.NHibernate/) package and a running Microsoft SQL Server instance configured accordingly. The sample NHibernate setup can be configured according to the environment:

snippet: NHibernateSetup


include: sagafinder-thesaga

snippet: TheSagaNHibernate

include: sagafinder-process

snippet:CustomSagaFinderNHibernate

include: sagafinder-ifindsagas

include: sagafinder-configurehowtofindsaga