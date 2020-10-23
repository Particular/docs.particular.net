---
title: Publish and Handle an Event
summary: How to define, publish, and handle events
component: Core
reviewed: 2020-10-24
related:
- samples/pubsub
- tutorials/nservicebus-step-by-step
- nservicebus/messaging/messages-events-commands
---

Event messages need to either implement `IEvent` or match a custom `DefiningEventsAs` convention. See the [message design documentation](/nservicebus/messaging/messages-events-commands.md) for more details.

## Handling an event

In order to handle an event, implement the `IHandleMessages<T>` interface in any [handler](/nservicebus/handlers) or [saga](/nservicebus/sagas) class, where `T` is the specific event type.


## Publishing an event

Call the `Publish` method to publish an event.

There are a few common scenarios for publishing events. Events might be published:

- from a **handler**, when processing another message.

snippet: publishFromHandler

- from a **saga handler**, when processing another message.

snippet: publishFromSaga

- at endpoint startup

snippet: publishAtStartup


## Composing events

In order to support advanced composition scenarios, events can be defined as interfaces. See the [Messages as Interfaces](/nservicebus/messaging/messages-as-interfaces.md) for more details.