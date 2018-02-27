---
title: Client side Callbacks
summary: The client (or sending process) has its own queue. When messages arrive in the queue, they are handled by a message handler.
reviewed: 2016-09-21
component: Callbacks
redirects:
- nservicebus/how-do-i-handle-responses-on-the-client-side
- nservicebus/messaging/handling-responses-on-the-client-side
related:
- samples/callbacks
---

Callbacks allow to use messaging behind a synchronous API that can't be changed. A common use case is introducing messaging to existing synchronous Web or WCF applications. The advantage of using callbacks is that they allow to gradually transition applications towards messaging.


## Handling responses in the context of a message being sent

When sending a message, a callback can be registered that will be invoked when a response arrives.

DANGER: If the server process returns multiple responses, NServiceBus cannot know which response message will be the last. To prevent memory leaks, the callback is invoked only for the first response. Callbacks won't survive a process restart (common scenarios are a crash or an [IIS recycle](https://msdn.microsoft.com/en-us/library/ms525803.aspx)) as they are held in memory, so they are less suitable for server-side development where fault-tolerance is required. In those cases, [sagas are preferred](/nservicebus/sagas/).

To handle responses from the processing endpoint, the sending endpoint must have it's own queue. Therefore, the sending endpoint cannot be configured as a [SendOnly endpoint](/nservicebus/hosting/#self-hosting-send-only-hosting). Messages arriving in this queue are handled using a message handler, similar to that of the processing endpoint, as shown:

snippet: EmptyHandler


## Prerequisites for callback functionality

In NServiceBus Version 5 and below callbacks are built into the core NuGet.

In NServiceBus Version 6 and above callbacks are shipped as `NServiceBus.Callbacks` NuGet package. This package has to be referenced by the requesting endpoint.

partial: enabling-callbacks

## Using Callbacks

The callback functionality can be split into three categories based on the type of information being used; integers, enums and objects. Each of these categories involves two parts; send+callback and the response.


### Int

The integer response scenario allows any integer value to be returned in a strong typed manner.

WARNING: This type of callback won't cause response messages to end up in the [error queue](/nservicebus/recoverability) if no callback is registered.

#### Send and Callback

snippet: IntCallback


#### Response

snippet: IntCallbackResponse

partial: int-reply


### Enum

The enum response scenario allows any enum value to be returned in a strong typed manner.


#### Send and Callback

snippet: EnumCallback

partial: enum-reply


#### Response

snippet: EnumCallbackResponse


### Object

The Object response scenario allows an object instance to be returned.


#### The Response message

This feature leverages the message Reply mechanism of the bus and hence the response needs to be a message.

snippet: CallbackResponseMessage


#### Send and Callback

snippet: ObjectCallback

partial: fakeHandler


#### Response

snippet: ObjectCallbackResponse

partial: cancellation


## When to use callbacks

WARNING: Using callbacks in `IHandleMessages<T>` classes can cause deadlocks and/or other unexpected behavior, so **do not call the callback APIs from inside a `Handle` method in an `IHandleMessages<T>` class**.

DANGER: Due to the fact that callbacks won't survive restarts, use callbacks when the data returned is **not business critical and data loss is acceptable**. Otherwise, use [request/response](/samples/fullduplex) with a message handler for the reply messages.

When using callbacks in a ASP.NET Web/MVC/Web API, the NServiceBus callbacks can be used in combination with the async support in Asp.Net to avoid blocking the web server thread and allowing processing of other requests. When response is received, it is handled and returned to the client side. Web clients will still be blocked while waiting for response. This scenario is common when migrating from traditional blocking request/response to messaging.


## Message routing


partial: route
