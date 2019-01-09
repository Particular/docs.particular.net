---
title: Publish and Handle an Event
summary: How to define, publish, and handle events
component: Core
reviewed: 2019-01-09
tags:
- Publish Subscribe
related:
- samples/pubsub
- tutorials/nservicebus-step-by-step
- nservicebus/messaging/messages-events-commands
---


## Defining events

Messages must be declared as an event before they can be published. This can be done with interfaces or using message conventions.


### Via an interface

Add the `IEvent` interface to the message definition:

snippet: EventWithInterface


### Via message conventions

Consider the following message:

snippet: EventWithConvention

This can be declared as an event using the following convention:

snippet: DefiningEventsAs

Learn more about [conventions](/nservicebus/messaging/conventions.md) and [unobtrusive mode](/nservicebus/messaging/unobtrusive-mode.md).


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


## Events as classes or interfaces

NServiceBus messages can be implemented either as classes or [interfaces](/nservicebus/messaging/messages-as-interfaces.md). Since interfaces can not be instantiated directly, use the following API to publish events implemented as interfaces:

snippet: InterfacePublish

When the event message is declared as an interface, NServiceBus will generate a proxy, set properties and publish the message. It's equivalent to the following call:

snippet: InstancePublish