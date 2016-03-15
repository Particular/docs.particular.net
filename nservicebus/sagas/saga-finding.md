---
title: Complex saga finding logic
summary: Use IFindSaga to write custom code that resolves sagas.
tags:
- Saga
related:
- samples/saga/ravendb-custom-sagafinder
- samples/saga/nh-custom-sagafinder
- nservicebus/sagas
---

Sometimes a saga handles certain message types without a single simple property that can be mapped to a specific saga instance. In those cases, you'll want finer-grained control of how to find a saga instance, as follows:


### [NHibernate](/nservicebus/nhibernate/) Saga Finder

snippet:nhibernate-saga-finder


### [RavenDB](/nservicebus/ravendb/) Saga Finder

snippet:ravendb-saga-finder

If a saga can't be found return `null`. 

include: non-null-task

You can have as many of these classes as you want for a given saga or message type. If a saga can't be found and if the saga specifies that it is to be started for that message type, NServiceBus will know that a new saga instance is to be created. The above example uses NServiceBus extension for NHibernate that allows both framework and user code to share the same NHibernate session. Similar extension point exists for RavenDB.

NOTE: When using custom saga finders users are expected to configure any additional indexes needed using the tooling of the selected storage engine. See our documentation on [sagas and concurrency](/nservicebus/sagas/concurrency.md) for further details.