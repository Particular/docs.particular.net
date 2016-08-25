---
title: Controlling what is subscribed
summary: When applying the publish-subscribe pattern there are several ways to control what messages are subscribed to
component: Core
---


## Automatic subscriptions

The default mode for managing subscriptions is "auto subscribe". When a subscriber endpoint is started it determines to which events it needs to subscribe. It then sends subscription messages to the publisher endpoint for those messages.

This happens each time the subscriber is restarted.

Messages matching the following criteria will be auto subscribed at startup.

 1. Defined as an event either using `IEvent` or by the `.DefiningEventsAs` convention.
 1. At least one [message handler and/or saga](/nservicebus/handlers/) exists for the given message.
 1. If selected transport [does not support publish-subscribe natively](/nservicebus/transports/#types-of-transports-unicast-only-transports), the publisher for that message need to be specified via the [routing](/nservicebus/messaging/routing.md) API.


### Exclude sagas from auto subscribe

partial: exclude


### Auto subscribe to plain messages

WARNING: This is a bad practice. Subscriptions should only be based on events.

partial: plainmessage


### When a subscriber stops or uninstalls

A Subscriber will not unsubscribe when it stops, it will remain registered at the publisher to receive events. The publisher still sends events to the queue of the stopped subscriber. When the subscriber is started it will consume the messages from its queue. The subscriber will never lose an event.


partial:disableautosubscribe


## How to manually subscribe to a message?

To manually subscribe and unsubscribe from a message:

snippet:ExplicitSubscribe
