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

The messages needs to be declared as an event before it can be published. That can be done in two ways: using marker interfaces or message conventions.


### Via a Marker interface

Add `IEvent` marker interface to the message definition:

snippet:EventWithInterface


### Via Message Conventions

The following message:

snippet:EventWithConvention

can be declared as an event using the following convention:

snippet:DefiningEventsAs


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

Events can be either classes or interfaces.  Since interfaces cannot be constructed there are slightly different semantics for publishing each.


### Publish a class

snippet:InstancePublish


### Publish an interface

If using interfaces to define event contracts set the message properties by passing in a lambda. NServiceBus will then generate a proxy and set those properties.

snippet:InterfacePublish
