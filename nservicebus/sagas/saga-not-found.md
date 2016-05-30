---
title: Sagas Not Found
summary: How a message is handled when it could be executed by a saga but no saga could be found.
reviewed: 2016-03-15
tags:
- Saga
related:
- samples/saga
---

The messages which are handled by sagas can either start a new saga (if handled by `IAmStartedByMessages<T>`) or update the existing saga (if handled by `IHandleMessages<T>`). If the incoming message should be handled by a saga, but is not expected to start a new one, then NServiceBus uses provided [correlation rules](/nservicebus/sagas/#correlating-messages-to-a-saga) to find an existing saga. If no existing saga can be found (e.g. because of a mistake in the provided correlation rule), then all implementations of `IHandleSagaNotFound` are executed.

This feature is useful if compensating actions need to be taken for messages that are handled by the saga which arrive after the saga has been marked as complete. The decision on using this feature should be driven by business requirements, in most systems it won't be used very often.

snippet:saga-not-found

Note that in the above situation the message will be considered successfully processed and sent to the audit queue even if no saga was found. Throw an exception from the `IHandleSagaNotFound` implementation to cause the message to end up in the error queue.

include: non-null-task
