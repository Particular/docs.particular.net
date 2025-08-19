---
title: Client-side callbacks
summary: Gradually transition applications from synchronous to a messaging architecture
reviewed: 2024-12-06
component: Callbacks
redirects:
- nservicebus/how-do-i-handle-responses-on-the-client-side
- nservicebus/messaging/handling-responses-on-the-client-side
related:
- samples/callbacks
---

Callbacks enable the use of messaging behind a synchronous API that can't be changed. A common use case is introducing messaging to existing synchronous web or WCF applications. The advantage of using callbacks is that they allow gradually transitioning applications towards messaging.

## When to use callbacks

> [!WARNING]
> **Do not call the callback APIs from inside a `Handle` method in an `IHandleMessages<T>` class** as this can cause deadlocks or other unexpected behavior.

> [!CAUTION]
> Because callbacks won't survive restarts, use callbacks when the data returned is **not business critical and data loss is acceptable**. Otherwise, use [request/response](/samples/fullduplex) with a message handler for the reply messages.

When using callbacks in an ASP.NET Web/MVC/Web API application, the NServiceBus callbacks can be used in combination with the async support in ASP.NET to avoid blocking the web server thread and allowing processing of other requests. When a response is received, it is handled and returned to the client. Web clients will still be blocked while waiting for response. This scenario is common when migrating from traditional blocking request/response to messaging.

## Handling responses in the context of a message being sent

When sending a message, a callback can be registered that will be invoked when a response arrives.

> [!CAUTION]
> If the server process returns multiple responses, NServiceBus does not know which response message will be last. To prevent memory leaks, the callback is invoked only for the first response. Callbacks won't survive a process restart (e.g. a crash or an [IIS recycle](https://msdn.microsoft.com/en-us/library/ms525803.aspx)) as they are held in memory, so they are less suitable for server-side development where fault-tolerance is required. In those cases, [sagas are preferred](/nservicebus/sagas/).

To handle responses from the processing endpoint, the sending endpoint must have its own queue. Therefore, the sending endpoint cannot be configured as a [SendOnly endpoint](/nservicebus/hosting/#self-hosting-send-only-hosting). Messages arriving in this queue are handled using a message handler, similar to that of the processing endpoint, as shown:

snippet: EmptyHandler

## Prerequisites for callback functionality

In NServiceBus version 5 and below, callbacks are built into the core NuGet.

In NServiceBus version 6 and above, callbacks are shipped as a separate `NServiceBus.Callbacks` NuGet package. This package has to be referenced by the requesting endpoint.

partial: enabling-callbacks

## Using callbacks

The callback functionality can be split into three categories based on the type of information being used: integers, enums and objects. Each of these categories involves two parts: send+callback and the response.

### Integers

The integer response scenario allows any integer value to be returned in a strongly-typed manner.

> [!WARNING]
> This type of callback won't cause response messages to end up in the [error queue](/nservicebus/recoverability) if no callback is registered.

#### Send and callback

snippet: IntCallback

#### Response

snippet: IntCallbackResponse

partial: int-reply

### Enum

The enum response scenario allows any enum value to be returned in a strongly-typed manner.

#### Send and Callback

snippet: EnumCallback

partial: enum-reply

#### Response

snippet: EnumCallbackResponse

### Object

The object response scenario allows an object instance to be returned.

#### The response message

This feature leverages the message reply mechanism of the bus, so the response must be a message.

snippet: CallbackResponseMessage

#### Send and callback

snippet: ObjectCallback

#### Response

snippet: ObjectCallbackResponse

partial: cancellation

## Message routing

partial: route

## Discarding reply messages

partial: discard
