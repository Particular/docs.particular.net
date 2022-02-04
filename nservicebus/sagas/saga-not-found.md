---
title: Sagas Not Found
summary: How a message is handled when it could be executed by a saga but no saga could be found.
component: Core
reviewed: 2021-08-13
related:
- samples/saga
---

The messages which are handled by sagas can either start a new saga (if handled by `IAmStartedByMessages<T>`) or update an existing saga (if handled by `IHandleMessages<T>`). If the incoming message in meant to be handled by a saga but is not expected to start a new one, then NServiceBus uses [correlation rules](/nservicebus/sagas/#correlating-messages-to-a-saga) to find an existing saga. If no existing saga can be found, all implementations of `IHandleSagaNotFound` are executed. If no implementation can be found, the message is discarded without additional notification.

snippet: saga-not-found

Note that in the example above the message will be considered successfully processed and sent to the audit queue even if no saga was found. Throw an exception from the `IHandleSagaNotFound` implementation to move the message to the error queue.

Note: If there are multiple saga types that handle a given message type and one of them is found while others are not, the `IHandleSagaNotFound` handlers **will not be executed**. The `IHandleSagaNotFound` handlers are executed only if no saga instances are invoked. The following table illustrates when the `IHandleSagaNotFound` handlers are invoked in cases when a message is mapped to two different saga types, A and B.

| Saga A found | Saga B found | Not found handler invoked |
|--------|--------|---------|
| ✔️    | ✔️     | ❌     |
| ✔️    | ❌     | ❌     |
| ❌    | ✔️     | ❌     |
| ❌    | ❌     | ✔️     |

include: non-null-task

The ability to provide an implementation for `IHandleSagaNotFound` is especially useful if compensating actions are needed for messages which arrive after the saga has been marked as complete. This is a common scenario when using timeouts inside the saga.

For example, consider a saga that is used for managing the registration process on the website. After a customer registers, they receive an email with a confirmation link. The system will wait for confirmation for a specific period of time, e.g. 24 hours. If the user doesn't click the link within 24 hours, their data is removed from the system and saga is completed. However, they might decide to click the confirmation link a few days later. In this case, the related saga instance can't be found and an exception will be thrown. By implementing `IHandleSagaNotFound` it is possible to handle the situation differently, e.g. redirect the user to the registration website and ask them to fill out the form again.

The implementation of `IHandleSagaNotFound` should be driven by the business requirements for a specific situation. In some cases the message might be ignored; in others, it might be useful to track whenever that situation happens (e.g. by logging or sending another message). In still other cases, it might make sense to perform a custom compensating action.

## Troubleshooting

*Saga not found* Exceptions can occur when:

1. The saga instance does not exist (yet)
2. The saga instance has already been removed

It's not always obvious when saga state does or does not exist. Most often the cause is due to race conditions.

Note: Expect out-of-order delivery if the message flow allows it and expect more-than-once processing.

### Out of order

If the design assumes message A will create the saga and message B updates the saga. If message B is received and processed before message A, the saga doesn't exist yet, and the message will be discarded.

Symptoms:

- Sagas are expected to exist but this does not always seems to be the case. Inspecting storage shows that the saga does exist.

Mitigation:

- Use [IAmStartedBy<>](/nservicebus/sagas/#starting-a-saga) instead of `IHandleMessages<>` for **any** message type which can originate from outside of the saga. No matter in which order messages are delivered, any message type processed first must create the saga.

- Each message handler that can start the saga might need to contain logic to check the saga state and see if it is time to take the next action. 

- Messages which are a result of a saga handler do not need this. i.e. A reply to a request made by a saga instance does not need to be able to create a new instance.

### Concurrency

Messages might be processed concurrently which can result in out-of-order processing.

Symptoms:

- Scatter/gather sagas behave unexpectedly and show saga not found exceptions.

Mitigation:

- Reduce processing concurrency to 1 to achieve sequential processing. This only works when only a single endpoint instance is active.
- Use pessimistic locking on the saga persister, this only works if a storage persister is configured that supports pessimistic locking.

### Initialization

A saga that at creation immediately sends one or more request messages and already is receiving response messages while the handler is still running will result in a response message not able to be correlated if the init handler is still running as the saga state did not yet been persisted

This can happen when:

- Using immediate dispatch
- Another `IMessageSession` is used instead of `context` or using an alternative protocol

Mitigation:

- Do not immediately send messages at saga creation, first send an init message to ensure the saga state exists. When a message is sent to a component that will respond the saga state will now exist.

### More-than-once processing

A message can be processed more-than-once because it was physically sent multiple times or a transport its consistency results in the same message becoming available again. More-than-once processing on a handler that completes the saga will now result in a saga not found for every duplicate. 

Mitigation:

- Use [outbox](/nservicebus/outbox/)
 
### Already completed

A saga that is already completed can result in a saga not found exception.

Mitigation:

- Use a logical complete state
- Use [saga timeouts](timeouts.md) to postpone physically removing the saga state and invoke [`MarkAsComplete`](/nservicebus/sagas/#ending-a-saga).
- Do not send messages and invoke [`MarkAsComplete`](/nservicebus/sagas/#ending-a-saga). Send an additional message to self (local) that will invoke `MarkAsComplete`.
