---
title: Sagas Not Found
summary: How a message is handled when it could be executed by a saga but no saga could be found.
component: Core
reviewed: 2017-10-04
tags:
- Saga
related:
- samples/saga
---

The messages which are handled by sagas can either start a new saga (if handled by `IAmStartedByMessages<T>`) or update the existing saga (if handled by `IHandleMessages<T>`). If the incoming message should be handled by a saga, but is not expected to start a new one, then NServiceBus uses provided [correlation rules](/nservicebus/sagas/#correlating-messages-to-a-saga) to find an existing saga. If no existing saga can be found then all implementations of `IHandleSagaNotFound` are executed. If no implementation can be found, the message is discarded without additional notifications.

snippet: saga-not-found

Note that in the example above the message will be considered successfully processed and sent to the audit queue even if no saga was found. Throw an exception from the `IHandleSagaNotFound` implementation to cause the message to end up in the error queue.

include: non-null-task

The ability to provide implementation for `IHandleSagaNotFound` is especially useful if compensating actions need to be taken for messages that are handled by the saga which arrive after the saga has been marked as complete. This is a very common scenario when using timeouts inside the saga.

For example the saga might be used for managing the registration process on the website. After somebody registers, they receive an email with a confirmation link. The system will wait for confirmation for a specific period of time, e.g. 24 hours. If the user doesn't click the link within 24 hours, their data is removed from the system and saga is completed. However, they might decide to click the confirmation link a few days later. In such a case the related saga instance can't be found and an exception will be thrown. By implementing `IHandleSagaNotFound` it is possible to handle the situation differently, e.g. redirect the user to the registration website and ask to fill the form again.

The implementation of `IHandleSagaNotFound` should be driven by the business requirements for a specific situation. In some cases the message might be ignored, in others it might be useful to track whenever that situation happens (e.g. by logging or sending another message), yet in others it might make sense to perform a custom compensating action.
