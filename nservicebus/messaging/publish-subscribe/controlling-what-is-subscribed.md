---
title: Controlling what is subscribed
summary: When applying the publish-subscribe pattern there are several ways to control what messages are subscribed to
---


## Automatic subscriptions

The default mode for managing subscriptions is "auto subscribe". When a subscriber endpoint is started it determines to which events it needs to subscribe. It then sends subscription messages to the [owning endpoint](/nservicebus/messaging/message-owner.md) for those messages.

This happens each time the subscriber is restarted.

Messages matching the following criteria will be auto subscribed at startup.

 1. Defined as an event either using `IEvent` or by the `.DefiningEventsAs` convention.
 1. At least one [message handler and/or saga](/nservicebus/handlers/) exists for the given message


### Transports without native support for publish-subscribe

In case of [transports without native support for publish-subscribe](/nservicebus/messaging/publish-subscribe/#mechanics-persistence-based) it is necessary to satisfy additional conditions:

 1. The project needs to use persistence, as explained in [Persistence Based Publish Subscribe](/nservicebus/messaging/publish-subscribe/#mechanics-persistence-based) section.
 1. An address of the publisher needs to be specified for each event, as described below.

The publisher's address is provided by [specifying a message owner in the message endpoint mappings](/nservicebus/messaging/message-owner.md). For example:

snippet:endpoint-mapping-appconfig

Since Version 6 it is possible to also specify the publishers in code. The following API is only available for transports that do not have native support for publish-subscribe.

snippet:PubSub-CodePublisherMapping


### Exclude sagas from auto subscribe

In Version 3 and below events that are only handled by sagas are not subscribed to by default. In Version 4 and above sagas are treated the same as handlers and will cause an endpoint to subscribe to a given event. It is possible to opt-in to the old exclude saga event handling behavior using:

snippet:DoNotAutoSubscribeSagas


### Auto subscribe to plain messages

In Version 4 and below all messages not defined as a command using `ICommand` or the `.DefiningCommandsAs` convention are automatically subscribed. In versions 4 and 5 it is possible to opt-in to that legacy behavior using following code

snippet:AutoSubscribePlainMessages

WARNING: This is a bad practice. Subscriptions should only be based on events.

Since Version 6 the ability to auto subscribe to plain messages was removed. Although not recommended, this can be overridden by [manually subscribing](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#how-to-manually-subscribe-to-a-message) to other message types.


### When a subscriber stops or uninstalls

A Subscriber will not unsubscribe when it stops, it will remain registered at the publisher to receive events. The publisher still sends events to the queue of the stopped subscriber. When the subscriber is started it will consume the messages from its queue. The subscriber will never lose an event.


## Disabling auto-subscription

Automatic subscriptions by the infrastructure can be disabled using the configuration API:

snippet:DisableAutoSubscribe


## How to manually subscribe to a message?

To manually subscribe and unsubscribe from a message:

snippet:ExplicitSubscribe
