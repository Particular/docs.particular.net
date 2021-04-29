---
title: Handlers
summary: Write a class to handle messages in NServiceBus.
component: Core
reviewed: 2021-04-29
redirects:
- nservicebus/how-do-i-handle-a-message
---

NServiceBus will take a message from the queue and hand it over to one or more message handlers. To create a message handler, write a class that implements `IHandleMessages<T>` where `T` is the message type:

snippet: CreatingMessageHandler

For scenarios that involve changing the application state via data access code in the handler, see [accessing data](/nservicebus/handlers/accessing-data.md).

To handle messages of all types:

 1. Set up the [message convention](/nservicebus/messaging/conventions.md) to designate which classes are messages. This example uses a namespace match.
 1. Create a handler of type `Object`. This handler will be executed for all messages that are delivered to the queue for this endpoint.

Since this class is setup to handle type `Object`, every message arriving in the queue will trigger it. Note that this might not be a recommended approach as [writing a behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) is often a better solution.

snippet: GenericMessageHandler

include: non-null-task

If using the Request-Response or Full Duplex pattern, handlers will probably do the work it needs to do, such as updating a database or calling a web service, then creating and sending a response message. See [How to Reply to a Message](/nservicebus/messaging/reply-to-a-message.md).

If handling a message in a publish-and-subscribe scenario, see [How to Publish/Subscribe to a Message](/nservicebus/messaging/publish-subscribe/).

### Mapping to name

Incoming messages will be mapped to a type using [Assembly Qualified Name](https://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx). This is the default behavior for sharing assemblies among endpoints. When a message cannot be mapped based on Assembly Qualified Name, the mapping will be attempted using [`FullName`](https://msdn.microsoft.com/en-us/library/system.type.fullname.aspx). The following is an example of how NServiceBus gets the type information.

```cs
var fqn = message.GetType().AssemblyQualifiedName;
var fallback = message.GetType().FullName;
```

## Behavior when there is no handler for a message

Receiving a message for which there are no message handlers is considered an error and the received message will be forwarded to the configured error queue.

partial: behaviorcaveat

## Unit testing

Unit testing handlers is supported by [the `NServiceBus.Testing` library](/nservicebus/testing/#testing-a-handler).
