---
title: Complex saga finding logic
summary: Use IFindSaga to write custom code that resolves sagas.
reviewed: 2016-04-11
tags:
- Saga
related:
- samples/saga/ravendb-custom-sagafinder
- samples/saga/nh-custom-sagafinder
- nservicebus/sagas
- nservicebus/sagas/concurrency
---

As described in [Correlating messages to a saga](/nservicebus/sagas/#correlating-messages-to-a-saga), every saga definition needs to specify how the existing saga instance should be found for an incoming message. The mechanisms described in that section should be the default choice and are suitable for many real-life scenarios. However, sometimes the mapping logic is too complex and it's necessary to define a custom saga finder.


### [NHibernate](/nservicebus/nhibernate/) Saga Finder

snippet:nhibernate-saga-finder


### [RavenDB](/nservicebus/ravendb/) Saga Finder

snippet:ravendb-saga-finder

If a saga can't be found return `null`.

include: non-null-task

Many finders may exist for a a given saga or message type. If a saga can't be found and if the saga specifies that it is to be started for that message type, NServiceBus will know that a new saga instance is to be created. The above example uses NServiceBus extension for NHibernate that allows both framework and user code to share the same NHibernate session. Similar extension point exists for RavenDB.

NOTE: When using custom saga finders users are expected to configure any additional indexes needed using the tooling of the selected storage engine.
