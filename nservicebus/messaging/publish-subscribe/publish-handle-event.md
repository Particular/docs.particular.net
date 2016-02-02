---
title: Publish and Handle an Event
summary: How to define a message as an event and publish that message.
tags:
- Publish Subscribe
related:
- samples/pubsub
- samples/step-by-step
- nservicebus/messaging/messages-events-commands
---


## Classifying a message as an event

To publish a message it must be classified as an event. There are two ways of achieving this


### Via a Marker interface

Adding a marker interface to the message definition.

snippet:EventWithInterface


### Via Message Conventions

Using the `EventWithConvention` message convention.

Given a message with the following definition.

snippet:EventWithConvention

It could be treated as an event using the following convention.

snippet:DefiningEventsAs


## Handling a event

An event can be handled by use of the `IHandleMessages` interface on any [Handler](/nservicebus/handlers) or [Saga](/nservicebus/sagas).

## Publishing an event

NOTE: In Version 6, the IBus interface has been deprecated and removed. Use the `Publish` methods on the `IMessageHandlerContext` interface within a handler, or the `IEndpointInstance`/`IBusSession` interface instead.

An event can be published via any instance of `IBus`. However there are several common locations where publishing occurs.


### From a Handler

From a handler in reaction to some other message being handled.

snippet:publishFromHandler


### From a Saga

From a handler in reaction to some other message being handled.

snippet:publishFromSaga


### At endpoint startup

At startup of an endpoint, directly after the bus has started.

snippet:publishAtStartup


## Events as Classes or Interfaces

Events can be either classes or interfaces. Since interfaces cannot be constructed there are slightly different semantics for publishing each.


### Publish a class

snippet:InstancePublish


### Publish an interface

If you are using interfaces to define your event contracts you need to set the message properties by passing in a lambda. NServiceBus will then generate a proxy and set those properties.

snippet:InterfacePublish
