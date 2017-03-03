---
title: Publish and Handle an Event
summary: How to define, publish and handle events.
component: Core
reviewed: 2017-03-02
tags:
- Publish Subscribe
related:
- samples/pubsub
- samples/step-by-step
- nservicebus/messaging/messages-events-commands
---


## Defining events

The messages needs to be declared as an event before they can be published. That can be done with marker interfaces or using message conventions.


### Via a Marker interface

Add `IEvent` marker interface to the message definition:

snippet:EventWithInterface


### Via Message Conventions

The following message:

snippet:EventWithConvention

can be declared as an event using the following convention:

snippet:DefiningEventsAs

Learn more about [conventions](/nservicebus/messaging/conventions.md) and [unobtrusive mode](/nservicebus/messaging/unobtrusive-mode.md).


## Handling an event

In order to handle an event, implement `IHandleMessages<T>` interface in any [Handler](/nservicebus/handlers) or [Saga](/nservicebus/sagas) class, where `T` is the specific event type.


## Publishing an event

In order to publish an event call the `Send` method from inside the event publisher. 

There are a few common scenarios for publishing events. Events might be published:

- from a **handler**, when processing another message.

   snippet:publishFromHandler

- from a **saga handler**, when processing another message.

   snippet:publishFromSaga

- at endpoint startup

   snippet:publishAtStartup


## Events as Classes or Interfaces

NServiceBus messages can be implemented either as classes or [interfaces](/nservicebus/messaging/messages-as-interfaces.md). Since interfaces cannot be instantiated directly, use the following API to send events implemented as interfaces:

snippet:InterfacePublish

NServiceBus will then generate a proxy, set properties and send the message. It's equivalent of the following call:

snippet:InstancePublish