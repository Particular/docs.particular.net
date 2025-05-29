---
title: Sending messages
reviewed: 2025-05-13
component: Core
redirects:
 - nservicebus/how-do-i-send-a-message
 - nservicebus/containers/injecting-ibus
related:
 - nservicebus/messaging/routing
 - nservicebus/messaging/messages-as-interfaces
---

NServiceBus supports sending different types of messages (see [Messages, Events, and Commands](messages-events-commands.md)) to any endpoint. Messages can be sent either directly from the endpoint or as part of handling an incoming message. When a message arrives at an endpoint, it goes through a [pipeline of processing steps](/nservicebus/pipeline/).

## Message sending/publishing interface summary

| Feature      | IMessageHandlerContext | IEndpointInstance | IMessageSession |
|--------------|------------------------|-------------------|-----------------|
| Can send/publish/reply to messages          | ✅  | ✅ | ✅ |
| Use inside a handler          | ✅ |  |  |
| Use outside of handler |  | ✅ | ✅ |
| Takes part in message handler transaction   | ✅ |  |  |
| Auto-injected into dependency injection container         |  |  | ✅ |
| Access to endpoint lifecycle control          |  | ✅ |  |
|||||

### IMessageHandlerContext

- Used from inside of the message handling pipeline
- Take part in the same transaction as that of the message handler (when using a transaction mode that supports it)
- Provides access to the incoming message being processed.

### IEndpointInstance

- The full endpoint instance, including lifecycle control
- Typically used in Program.cs or service startup for initializing and shutting down NServiceBus
- Should not be used inside a message handler

### IMessageSession

- Interface for sending, replying to and publishing messages
- Automatically registered and injected into the dependency injection container by NServiceBus, but only after the endpoint has been started successfully
- Typically used from within business logic that needs to send messages
- Should not be used inside a message handler

### IUniformSession

[IUniformSession](./uniformsession.md) was introduced in NServiceBus v6 and is an opt-in for a uniform session approach that works seamlessly as a message session outside the pipeline and as a pipeline context inside the message handling pipeline.
It represents either an `IMessageSession` or `IMessageHandlerContext` depending on where it's used.

### ITransactionalSession

[ITransactionalSession](./../transactional-session/) is a stand alone package that helps to achieve consistency when modifying business data and sending messages outside the context of an NServiceBus message handler, such as from an ASP.NET Core controller. When combined with the [outbox](./../outbox/) it guarantees atomic consistency across database and message operations.

## Outside a message handler

In some cases, messages that need to be sent may not be related to an incoming message. Some examples are:

* Sending a command when an HTML form is submitted in an ASP.NET application.
* Publishing an event when the user clicks a button on a GUI application (see [Publish and Handle an Event](publish-subscribe/publish-handle-event.md)).

To send a message directly from the endpoint:

snippet: BasicSend

Unit testing the process of sending a message is supported by [the `NServiceBus.Testing` library](/nservicebus/testing/#testing-message-session-operations).

## Inside the incoming message processing pipeline

Messages are often sent as part of handling an incoming message. When running in a [transaction mode](/transports/transactions.md) that supports it, these send operations take part in the same transaction as that of the message handler, thereby ensuring that the send operation rolls back if the handling of the message fails at any stage of the message processing pipeline.

To send a message from inside a message handler:

snippet: SendFromHandler

> [!WARNING]
> Using `IMessageSession` or `IEndpointInstance` to send messages inside a handler instead of the provided `IMessageHandlerContext` should be avoided.
>
> Some of the dangers when using an `IMessageSession` or `IEndpointInstance` inside a message handler to send or publish messages are:
>
> * Those messages will not participate in the same transaction as that of the message handler. This could result in messages being dispatched or events published even if the message handler resulted in an exception and the operation was rolled back.
> * Those messages will not be part of the [batching operation](/nservicebus/messaging/batched-dispatch.md).
> * Those messages will not contain any important message header information that is available via the `IMessageHandlerContext` interface parameter, e.g., CorrelationId.

> [!NOTE]
> `Send` is an asynchronous operation. When the invocation ends, it does not mean that the message has actually been sent. In scenarios where a large number of messages are sent in a short period, it might be beneficial, from a performance perspective, to limit the number of outstanding send operations pending for completion. Sample approaches that can be used to limit the number of send tasks can be found in [Writing Async Handlers](/nservicebus/handlers/async-handlers.md#concurrency-large-amount-of-concurrent-message-operations).

## Overriding the default routing

The `SendOptions` object can be used to override the default routing.

Using the destination address:

snippet: BasicSendSetDestination

Using the ID of the target instance:

snippet: BasicSendSpecificInstance

## Sending to *self*

Sending a message to the same endpoint, i.e. sending to *self*, can be done in two ways.

An endpoint can send a message to any of its own instances:

snippet: BasicSendToAnyInstance

Or, it can request a message to be routed to itself, i.e. the same instance.

> [!NOTE]
> This option is only possible when an endpoint instance ID has been specified.

Messages are sent via the queueing infrastructure just like a regular Send. This means that it will use batched dispatch and - if configured - outbox.

snippet: BasicSendToThisInstance

## Influencing the reply behavior

When a receiving endpoint replies to a message, the reply message will be routed to any instance of the sending endpoint by default. The sender of the message can also control how reply messages are received.

To send the reply message to the specific instance that sent the initial message:

snippet: BasicSendReplyToThisInstance

To send the reply message to any instance of the endpoint:

snippet: BasicSendReplyToAnyInstance

The sender can also request the reply to be routed to a specific transport address

snippet: BasicSendReplyToDestination

## Dispatching a message immediately

While it's usually best to let NServiceBus [handle all exceptions](/nservicebus/recoverability/), there are some scenarios where messages might need to be sent regardless of whether the message handler succeeds or not, for example, to send a reply notifying that there was a problem with processing the message.

### Usage

This can be done by using the immediate dispatch API:

snippet: RequestImmediateDispatch

> [!NOTE]
> The API behaves the same for `ITransactionalSession` but differently for `IMessageSession`. When invoking message operations on `IMessageSession` the `RequestImmediateDispatch` does not have any effect as messages will always be immediately dispatched.

### More-than-once side effects

Side effects can occur when failures happen after sending the message. The messages could be retried meaning duplicate messages are created if this code is executed more than once.

When messages are sent via immediate disaptch:

1. Ensure that the same [message identifier](/nservicebus/messaging/message-identity.md) gets assigned to them when invoked more than once.
2. Due to failures it could happen that messages are sent that contain state which is inconsistent because of failing operations like a storage modification that didn't occur.

### Bypasses outbox and batching

By specifying immediate dispatch, outgoing messages will not be [batched](/nservicebus/messaging/batched-dispatch.md) or enlisted in the current receive transaction, even if the transport supports transactions or batching. Similarly, when the [outbox](/nservicebus/outbox/) feature is enabled, messages sent using immediate dispatch won't go through the outbox.
