---
title: Controlling What Is Subscribed
summary: When applying the publish-subscribe pattern, there are several ways to control what messages are subscribed to
component: Core
reviewed: 2020-05-27
---


## Automatic subscriptions

The default mode for managing subscriptions is *auto-subscribe*. Every time a subscriber endpoint starts, it determines which events it needs to subscribe to and automatically subscribes to them. For more information on how publish and subscribe works, refer to [Publish-Subscribe](/nservicebus/messaging/publish-subscribe).

Messages matching both of the following criteria will be auto-subscribed at startup:

 1. Defined as an event either using `IEvent` or by the `.DefiningEventsAs` convention.
 1. At least one [message handler and/or saga](/nservicebus/handlers/) exists for the given event.

Note: If the selected transport [does not support publish-subscribe natively](/transports/types.md#unicast-only-transports), the publisher for that message must be specified via the [routing](/nservicebus/messaging/routing.md) API.

partial: missing-publisher-info-error

partial: exclude-event-types


### Exclude sagas from auto-subscribe

Sagas are treated the same as handlers and will cause an endpoint to subscribe to a given event. It is possible to opt-in to the old exclude saga event handling behavior using:

snippet: DoNotAutoSubscribeSagas


### Auto-subscribe to plain messages

WARNING: This is a bad practice. Subscriptions should be based on events.

partial: plainmessage


### When a subscriber stops or uninstalls

A subscriber will not unsubscribe when it stops; it will remain registered at the publisher to receive events. The publisher still sends events to the queue of the stopped subscriber. When the subscriber is started, it will consume the messages from its queue. The subscriber will never lose an event.


## Disabling auto-subscription

Automatic subscriptions by the infrastructure can be disabled using the configuration API:

snippet: DisableAutoSubscribe


## Manually subscribing to a message

Events can manually be subscribed and unsubscribed to:

snippet: ExplicitSubscribe

partial: manualsubscriptions
