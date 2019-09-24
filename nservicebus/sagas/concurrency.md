---
title: Saga concurrency
summary: NServiceBus ensures consistency between saga state and messaging.
component: Core
tags:
- Saga
redirects:
- nservicebus/nservicebus-sagas-and-concurrency
reviewed: 2019-09-24
related:
- persistence/nhibernate/saga-concurrency
- persistence/ravendb/saga-concurrency
- persistence/sql/saga-concurrency
---

An [endpoint](/nservicebus/concept-overview.md#endpoint) may be configured to allow [concurrent handling of messages](/nservicebus/operations/tuning.md#tuning-concurrency). An endpoint may also be [scaled out](/nservicebus/architecture/scaling.md#scaling-out-to-multiple-nodes) to multiple nodes. In these scenarios, multiple messages may be received simultaneously which correlate to a single saga instance. Handling those messages may cause saga state to be created, updated, or deleted, and may cause new messages to be sent.

NOTE: With respect to sagas, "handling" a message refers to the invocation of any saga method that processes a message, such as `IAmStartedByMessages<T>.Handle()`, `IHandleTimeouts<T>.Timeout()`, etc.

To ensure consistency between saga state and messages, NServiceBus ensures that receiving a message, changing saga state, and sending new messages, occurs as a single, atomic operation, and that two message handlers for the same saga instance do not perform this operation simultaneously.

WARNING: This level of consistency is provided automatically when using combinations of transports and persisters that can enlist in a transaction. When using other combinations of transports and persisters, the [outbox](/nservicebus/outbox/) must be used instead.

Saga state is created when a saga is started by a message. It may be updated when the saga handles another message. It is deleted when the saga handles another message which completes the saga. In any of these cases, multiple messages may be received simultaneously.

## Starting a saga

When messages that start the same saga instance are received simultaneously, NServiceBus ensures that only one message starts the saga.

Using optimistic concurrency, only one message handler is allowed to succeed. The saga state is created and sent messages are dispatched. The other handlers fail, roll back, and their messages enter [recoverability](/nservicebus/recoverability/). When those messages are retried, the existing saga state is found. Handling the messages may involve changing the state or completing the saga. See below for how those scenarios are handled.

partial: unique

## Changes to saga state

When using some persisters, messages received simultaneously that correlate to the same existing saga instance are not handled simultaneously. The persister uses pessimistic locking to ensure that message handlers are invoked one after another.

When using other persisters, and simultaneously handling messages which cause changes to the state of the same existing saga instance, NServiceBus ensures that only one message changes the state. Using optimistic concurrency, only one message handler is allowed to succeed. The saga state is updated, and sent messages are dispatched. The other handlers fail, roll back, and their messages enter [recoverability](/nservicebus/recoverability/). When those messages are retried, the process repeats. One or more of those messages may attempt to complete the saga. See below for how that scenario is handled.

See the [documentation for each persister](/persistence/) for details of which method is used.

## Completing a saga

When using some persisters, messages received simultaneously that correlate to the same existing saga instance are not handled simultaneously. The persister use pessimistic locking to ensure that message handlers are invoked one after another.

When using other persisters, and simultaneously receiving messages which cause changes to the state of the same existing saga instance, and at least one of those messages causes the saga to be completed, NServiceBus ensures that only one message either changes the state or completes the saga. Using optimistic concurrency, only one message handler is allowed to succeed. If the message changes the saga, the saga stated is updated, and sent messages are dispatched. If the message completes the saga, the saga state is deleted, and sent messages are dispatched. The other handlers fail, roll back, and their messages enter [recoverability](/nservicebus/recoverability/).

In both cases, if a message completes the saga, when subsequent messages are received, the saga state is not found and those messages are [discarded](/nservicebus/sagas/saga-not-found.md).

See the [documentation for each persister](/persistence/) for details of which method is used.

## High-load scenarios

Under high load, simultaneous attempts to create, update, or delete saga state may lead to a high number of retries. Those retries may be exhausted, resulting in messages being moved to the error queue.

In such scenarios, a re-design may be required.

Further reading is available in _[Reducing NServiceBus Saga load](https://lostechies.com/jimmybogard/2014/02/27/reducing-nservicebus-saga-load/)_ by Jimmy Bogard.
