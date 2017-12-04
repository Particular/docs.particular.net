---
title: Controlling what is subscribed
summary: When applying the publish-subscribe pattern, there are several ways to control what messages are subscribed to
component: Core
tags:
- Publish Subscribe
reviewed: 2016-11-07
---


## Automatic subscriptions

The default mode for managing subscriptions is *auto subscribe*. When a subscriber endpoint is started, it determines  which events it needs to subscribe to. This auto subscription mechanism happens each time the subscriber is restarted. For more information how publish and subscribe works refer to the [Publish-Suscribe](/nservicebus/messaging/publish-subscribe) documentation page. 

Messages matching the following criteria will be auto subscribed at startup.

 1. Defined as an event either using `IEvent` or by the `.DefiningEventsAs` convention.
 1. At least one [message handler and/or saga](/nservicebus/handlers/) exists for the given event.
 1. If the selected transport [does not support publish-subscribe natively](/transports/#types-of-transports-unicast-only-transports), the publisher for that message needs to be specified via the [routing](/nservicebus/messaging/routing.md) API.


### Exclude sagas from auto subscribe

partial: exclude


### Auto subscribe to plain messages

WARNING: This is a bad practice. Subscriptions should always be based on events.

partial: plainmessage


### When a subscriber stops or uninstalls

A Subscriber will not unsubscribe when it stops, it will remain registered at the publisher to receive events. The publisher still sends events to the queue of the stopped subscriber. When the subscriber is started it will consume the messages from its queue. The subscriber will never lose an event.


partial: disableautosubscribe


## Manually subscribing to a message

Events can manually be subscribed and unsubscribed:

snippet: ExplicitSubscribe

partial: manualsubscriptions

