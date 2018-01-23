---
title: Complex saga finding logic
summary: Use IFindSaga to write custom code that resolves sagas.
reviewed: 2018-01-22
component: Core
tags:
- Saga
related:
- samples/saga/nh-custom-sagafinder
- samples/saga/sql-sagafinder
- nservicebus/sagas
- nservicebus/sagas/concurrency
---

A saga can handle multiple messages. When NServiceBus receives a message that should be handled by a saga, it uses the [configured mapping information](/nservicebus/sagas/#correlating-messages-to-a-saga) to determine the correct saga instance that should handle the incoming message. In many cases the correlation logic is simple and can be specified using the provided [mapping function](/nservicebus/sagas/#correlating-messages-to-a-saga), which is the recommended default approach. However, if the correlation logic is very complex it might be necessary to define a custom saga finder.

Custom Saga Finders are created by implementing `IFindSagas`.

snippet: saga-finder


include: non-null-task

Many finders may exist for a given saga or message type. If a saga can't be found and if the saga specifies that it is to be started for that message type, NServiceBus will know that a new saga instance is to be created.

WARNING: When using custom saga finders, users are expected to configure any additional indexes needed to handle [concurrent access to saga instances](/nservicebus/sagas/concurrency.md) properly using the tooling of the selected storage engine. Due to this constraint, persisters are not all able to support custom saga finders to the same degree.
