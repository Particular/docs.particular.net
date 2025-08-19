---
title: Handlers
summary: Write a class to handle messages in NServiceBus.
component: Core
reviewed: 2024-05-05
redirects:
- nservicebus/how-do-i-handle-a-message
---

NServiceBus will take a message from the queue and hand it over to one or more message handlers. To create a message handler, write a class that implements `IHandleMessages<T>` where `T` is the message type:

snippet: CreatingMessageHandler

For scenarios that involve changing the application state via data access code in the handler, see [accessing data](/nservicebus/handlers/accessing-data.md).

include: non-null-task

If using the Request-Response or Full Duplex pattern, handlers will probably do the work it needs to do, such as updating a database or calling a web service, then creating and sending a response message. See [How to Reply to a Message](/nservicebus/messaging/reply-to-a-message.md).

If handling a message in a publish-and-subscribe scenario, see [How to Publish/Subscribe to a Message](/nservicebus/messaging/publish-subscribe/).

## Mapping to name

Incoming messages will be mapped to a type using [Assembly Qualified Name](https://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx). This is the default behavior for sharing assemblies among endpoints. When a message cannot be mapped based on Assembly Qualified Name, the mapping will be attempted using [`FullName`](https://msdn.microsoft.com/en-us/library/system.type.fullname.aspx). The following is an example of how NServiceBus gets the type information.

```cs
var fqn = message.GetType().AssemblyQualifiedName;
var fallback = message.GetType().FullName;
```

## No handler for a message

Receiving a message for which there are no message handlers is considered an error and the received message will be forwarded to the configured error queue.

## Multiple handlers for a single message

Handling a single message in a given endpoint is treated as a single unit of work, regardless of how many handlers handle that message. If one handler fails, the message is retried according to the [recoverability policy](/nservicebus/recoverability) of the endpoint. When the message is retried, _all_ matching handlers are invoked again, including any that successfully handled the message during previous attempts.

For this reason, multiple handlers for the same message must either roll back their operations if any of them fail, or they must be idempotent and handle multiple invocations without any side-effects.

### Alternatives

If it is not possible to design multiple handlers in one of the ways described above, a separate message must be used for each handler. This has the additional benefit of the handlers being invoked in parallel. When a single message is used, the handlers are invoked sequentially.

There are a number of techniques for making this change, depending on the type of message:

##### Original message is published as an event

If the original message is [published as an event](/nservicebus/messaging/publish-subscribe/), the handlers must be hosted in separate endpoints. Each endpoint receives its own copy of the original message, isolating the failure of one handler from the others.

##### Original message is sent to an endpoint

If the original message is not published as an event, but rather [sent](/nservicebus/messaging/send-a-message.md) to a specific endpoint, the following techniques may be used (listed from simplest to most complex):

- Continue hosting the handlers in one endpoint, but create a new message type for each one and change each handler to handle one of those new messages instead of the original message. Then, either:
  - Create the new messages at the destination:
    - Create a new handler in the same endpoint as the others which handles the original message type.
    - In the new handler, invoke `SendLocal` for each new message type.
  - Or, create the new messages at the source:
    - Instead of sending a single message, send an instance of each new message.
- Host each handler in a separate endpoint:
  - Send a copy of the original message to each endpoint.
  - This provides the greatest degree of isolation and provides more granularity for retry policy customization and scaling, greater visibility, better monitoring, and other benefits.

## Unit testing

Unit testing handlers is supported by [the `NServiceBus.Testing` library](/nservicebus/testing/#testing-a-handler).
