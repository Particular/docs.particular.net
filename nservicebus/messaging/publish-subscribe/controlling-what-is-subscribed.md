---
title: Controlling what is subscribed
summary: When applying the publish-subscribe pattern there are several ways to control what messages are subscribed to
tags: []
---


## Automatic subscriptions

The default mode for managing subscriptions is  "auto subscribe". When a subscriber endpoint is started it determines to which events it needs to subscribe. It then sends sends subscription messages to the [owning endpoint](/nservicebus/messaging/message-owner.md) for those messages.

This happens each time the subscriber is restarted.

Messages matching the following criteria will be auto subscribed at startup.

 1. Defined as a event either using `IEvent` or by the `.DefiningEventsAs` convention.
 1. At least one [message handler and/or saga](/nservicebus/handlers/) exists for the given message

### Routing configuration needed

If your selected transport doesn't natively supports publish-subscribe you need to specify the address of the publisher for each event. This is done by [specifying a message owner in the message endpoint mappings](/nservicebus/messaging/message-owner.md). For example:

snippet:endpoint-mapping-appconfig

Since Version 6 you can also specify the publishers in code

snippet:PubSub-CodePublisherMapping

### Exclude sagas from auto subscribe

In Version 3 and lower events that are only handled by sagas are not subscribed to by default. In Version 4 and higher sagas are treated the same as handlers and will cause an endpoint to subscribe to a given event. You can opt-in to the old exclude saga event handling behavior using:

snippet:DoNotAutoSubscribeSagas


### Auto subscribe to plain messages

Before Version 4 all messages not defined as a command using `ICommand` or the `.DefiningCommandsAs` convention where automatically subscribed. In versions 4 and 5 you could opt-into that legacy behavior using following code

snippet:AutoSubscribePlainMessages

Starting with version 6 we removed that possibility as it was encouraging bad practices.

### When a subscriber stops or uninstalls

A Subscriber will not unsubscribe when it stops, it will remain registered at the publisher to receive events. The publisher still sends events to the queue of the stopped subscriber. When the subscriber is started it will consume the messages from its queue. The subscriber will never loose an event.


## Disabling auto-subscription

You can also choose to **not** have the infrastructure automatically subscribe using the configuration API

snippet:DisableAutoSubscribe


## How to manually subscribe to a message?

To manually subscribe and unsubscribe from a message:

snippet:ExplicitSubscribe
